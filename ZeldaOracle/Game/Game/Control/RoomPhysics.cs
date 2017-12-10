﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Game.Entities;
using ZeldaOracle.Game.Entities.Collisions;
using ZeldaOracle.Game.Entities.Players;
using ZeldaOracle.Game.Main;
using ZeldaOracle.Game.Tiles;

namespace ZeldaOracle.Game.Control {

	public class RoomPhysics {

		private RoomControl roomControl;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public RoomPhysics(RoomControl roomControl) {
			this.roomControl = roomControl;
		}


		//-----------------------------------------------------------------------------
		// Physics Update
		//-----------------------------------------------------------------------------

		public void ProcessPhysics() {
			// Process physics for all entities.
			for (int i = 0; i < roomControl.Entities.Count; i++) {
				Entity entity = roomControl.Entities[i];
				if (entity.Physics != null && entity.Physics.IsEnabled) {
					ProcessEntityPhysics(entity);
				}
			}
		}
		
		// Process physics for a single entity.
		public void ProcessEntityPhysics(Entity entity) {
			bool startedOnGround = entity.IsOnGround;

			// Update Z dynamics
			UpdateEntityZPosition(entity);
			
			// Initialize the collision state for this frame.
			InitPhysicsState(entity);

			if (roomControl.IsSideScrolling && entity.Physics.PreviousCollisionInfo[Directions.Down].Tile != null) {
				// NOTE: this needs some checks before execution.
				Vector2F tileVelocity = entity.Physics.PreviousCollisionInfo[Directions.Down].Tile.Velocity;
				entity.Y += tileVelocity.Y;
				entity.Physics.VelocityX += tileVelocity.X;
			}

			// Check the surface tile beneath the entity.
			CheckSurfaceTile(entity);
			// Resolve collisions with solid objects.
			CheckSolidCollisions(entity);
			// Resolve collisions with the room edge.
			CheckRoomEdgeCollisions(entity);
			// Integrate velocity.
			entity.Position += entity.Physics.Velocity;
			
			// Restore the entity's velocity before collision checks.
			Vector2F newVelocity = entity.Physics.PreviousVelocity;
			for (int axis = 0; axis < 2; axis++) {
				if (Math.Sign(newVelocity[axis]) > 0 && entity.Physics.Velocity[axis] <= newVelocity[axis])
					newVelocity[axis] = entity.Physics.Velocity[axis];
				else if (Math.Sign(newVelocity[axis]) < 0 && entity.Physics.Velocity[axis] >= newVelocity[axis])
					newVelocity[axis] = entity.Physics.Velocity[axis];
			}
			entity.Physics.Velocity = newVelocity;
			
			// Check for landing and falling in side-scrolling mode.
			if (IsSideScrolling) {
				if (!startedOnGround && entity.IsOnGround && entity.Physics.HasGravity)
					LandEntity(entity);
				if (startedOnGround && !entity.IsOnGround && entity.Physics.HasGravity)
					entity.OnBeginFalling();
			}

			// Change velocity if rebounded.
			CheckRebound(entity);
			
			entity.Physics.IsColliding = false;
			for (int i = 0; i < Directions.Count; i++) {
				if (entity.Physics.CollisionInfo[i].IsColliding)
					entity.Physics.IsColliding = true;
			}

			// Update ledge passing.
			CheckLedges(entity);
			// Check if destroyed outside room.
			CheckOutsideRoomBounds(entity);
		}

		// Initialize an entity's physics state for the frame.
		public void InitPhysicsState(Entity entity) {
			entity.Physics.PreviousVelocity		= entity.Physics.Velocity;
			entity.Physics.PreviousZVelocity	= entity.Physics.ZVelocity;
			entity.Physics.IsColliding			= false;

			for (int i = 0; i < Directions.Count; i++) {
				entity.Physics.PreviousCollisionInfo[i] = entity.Physics.CollisionInfo[i];
				entity.Physics.CollisionInfo[i].Clear();
				entity.Physics.MovementCollisions[i] = false;
			}
		}

		
		//-----------------------------------------------------------------------------
		// Solid Collisions
		//-----------------------------------------------------------------------------

		// Check collisions with solid objects (entities and/or tiles).
		private void CheckSolidCollisions(Entity entity) {
			if (entity.Physics.CollideWithWorld || entity.Physics.CollideWithEntities) {
				
				// Handle circular tile collisions.
				if (entity.Physics.CollideWithWorld && entity.Physics.CheckRadialCollisions) {
					Rectangle2F checkArea = Rectangle2F.Union(
						Rectangle2F.Translate(entity.Physics.CollisionBox, entity.Position),
						Rectangle2F.Translate(entity.Physics.CollisionBox, entity.Position + entity.Physics.Velocity));

					foreach (Tile tile in roomControl.TileManager.GetTilesTouching(checkArea)) {
						if (CanCollideWithTile(entity, tile) && tile.CollisionStyle == CollisionStyle.Circular)
							ResolveCircularCollision(entity, tile, tile.Position, tile.CollisionModel);
					}
				}

				// 1. Resolve unresolved collisions from the previous frame.
				ResolvePreviousClipCollisions(entity);
				// 2. Detect collisions.
				DetectClipCollisions(entity);
				// 3. Resolve collisions.
				ResolveClipCollisions(entity);
				// 4. Detect any new unresolved collisions.
				DetectClipCollisions(entity);
				// 5. Clip velocity for all detected collisions.
				ClipVelocity(entity);
				// 6. Check if the entity is being crushed.
				CheckCrush(entity);
				// 7. Detect and resolve collisions with movement.
				ResolveMovementCollisions(entity);
				
				// Check the player's side-scrolling ladder collisions.
				if (roomControl.IsSideScrolling && (entity is Player))
					CheckPlayerLadderClimbing((Player) entity);

				// Set the entity's collision info.
				for (int i = 0; i < Directions.Count; i++) {
					CollisionInfoNew clipCollision = entity.Physics.ClipCollisionInfo[i];

					if (clipCollision.IsCollidingAndNotAllowedClipping) {
						if (!entity.Physics.CollisionInfo[i].IsColliding)
							entity.Physics.CollisionInfo[i].SetCollision(clipCollision.CollidedObject, i);
					}
					else if (!entity.Physics.MovementCollisions[i])
						entity.Physics.CollisionInfo[i].Clear();

					if (entity.Physics.CollisionInfo[i].IsColliding)
						entity.Physics.IsColliding = true;
				}
			}
		}
		
		// Check if the entity should rebound off of its collisions.
		private void CheckRebound(Entity entity) {
			if (!entity.Physics.ReboundSolid && !entity.Physics.ReboundRoomEdge)
				return;

			Vector2F newEntityVelocity = entity.Physics.Velocity;

			for (int i = 0; i < Directions.Count; i++) {
				CollisionInfo collision = entity.Physics.CollisionInfo[i];

				if (collision.IsColliding &&
					((entity.Physics.ReboundSolid && (collision.Type == CollisionType.Tile || collision.Type == CollisionType.Entity)) ||
					(entity.Physics.ReboundRoomEdge && collision.Type == CollisionType.RoomEdge)) &&
					entity.Physics.PreviousVelocity.Dot(Directions.ToVector(i)) > 0.0f)
				{
					int axis = Directions.ToAxis(i);
					newEntityVelocity[axis] = -entity.Physics.PreviousVelocity[axis];
				}
			}
			entity.Physics.Velocity = newEntityVelocity;
		}
		
		
		//-----------------------------------------------------------------------------
		// Circular Collisions
		//-----------------------------------------------------------------------------
		
		private void ResolveCircularCollision(Entity entity, Tile tile, Vector2F modelPos, CollisionModel model) {
			for (int i = 0; i < model.BoxCount; i++) {
				Rectangle2F box = model.Boxes[i];
				box.Point += modelPos;
				ResolveCircularCollision(entity, tile, box);
			}
		}
		
		private void ResolveCircularCollision(Entity entity, object solidObject, Rectangle2F solidBox) {
			Rectangle2F collisionBox = entity.Physics.CollisionBox;
			Rectangle2F entityBoxPrev = entity.Physics.CollisionBox;
			entityBoxPrev.Point += entity.Position;

			// If already colliding with the object, then push away from its center.
			if (entityBoxPrev.Intersects(solidBox)) {
				Vector2F correction = (entity.Position - solidBox.Center).Normalized;
				correction = Vector2F.SnapDirectionByCount(correction, 16);
				entity.Position += 1.0f * correction;
				return;
			}

			// Predict collisions for each axis.
			for (int axis = 0; axis < 2; axis++) {
				Rectangle2F entityBox = entity.Physics.CollisionBox;
				entityBox.Point += entity.Position;
				if (axis == 0)
					entityBox.X += entity.Physics.VelocityX;
				else
					entityBox.Point += entity.Physics.Velocity;

				if (entityBox.Intersects(solidBox)) {
					// Snap the position.
					Vector2F newPos = entity.Position;
					if (entityBox.Center[axis] < solidBox.Center[axis])
						newPos[axis] = solidBox.TopLeft[axis] - collisionBox.BottomRight[axis];
					else
						newPos[axis] = solidBox.BottomRight[axis] - collisionBox.TopLeft[axis];
					entity.Position = newPos;
					
					Vector2F newVelocity = entity.Physics.Velocity;
					if (Math.Abs(entity.Physics.Velocity[1 - axis]) < 0.1f) {
						// Slightly push away from the center of the solid object.
						Vector2F distance = entity.Physics.PositionedCollisionBox.Center - solidBox.Center;
						//Vector2F correction = (entity.Position - solidBox.Center).Normalized;
						//correction = Vector2F.SnapDirectionByCount(correction, 16);
						if (GMath.Abs(distance[1 - axis]) > 1.0f) {
							//distance = GMath.Sqr(GMath.Abs(distance)) * GMath.Sign(distance);
							distance = (distance * distance) * GMath.Sign(distance);
							//newVelocity[1 - axis] += distance[1 - axis] * 0.015f;
							newPos = entity.Position;
							newPos[1 - axis] += distance[1 - axis] * 0.015f;
							if (!IsCollidingAt(entity, newPos, false))
								entity.Position = newPos;
						}
					}
					newVelocity[axis] = 0.0f;
					entity.Physics.Velocity = newVelocity;
				}
			}
		}



		//-----------------------------------------------------------------------------
		// Clip Collision Detection
		//-----------------------------------------------------------------------------
		
		private struct CollisionCheck {
			public object SolidObject { get; set; }
			public Rectangle2F SolidBox { get; set; }
		}

		// Returns an enumerable list of all possible collisions.
		private IEnumerable<CollisionCheck> GetCollisions(Entity entity, Rectangle2F area) {
			// Find nearby solid entities.
			if (entity.Physics.CollideWithEntities) {
				foreach (Entity other in RoomControl.Entities) {
					if (other != entity && CanCollideWithEntity(entity, other)) {
						yield return new CollisionCheck() {
							SolidBox = other.Physics.PositionedCollisionBox,
							SolidObject = other
						};
					}
				}
			}
			// Find nearby solid tiles tiles.
			if (entity.Physics.CollideWithWorld) {
				foreach (Tile tile in RoomControl.TileManager.GetTilesTouching(area)) {
					if (CanCollideWithTile(entity, tile) && tile.CollisionStyle == CollisionStyle.Rectangular) {
						foreach (Rectangle2I box in tile.CollisionModel.Boxes) {
							Rectangle2F tileBox = box;
							tileBox.Point += tile.Position;
							yield return new CollisionCheck() {
								SolidBox = tileBox,
								SolidObject = tile
							};
						}
					}
				}
			}
		}
		
		// Returns true if the entity is able to collide with a tile.
		private bool CanCollideWithTile(Entity entity, Tile checkTile) {
			if (checkTile.CollisionModel == null || !checkTile.IsSolid ||
				(checkTile.IsHalfSolid && entity.Physics.PassOverHalfSolids))
				return false;
			if (checkTile.IsLedge && entity.Physics.PassOverLedges) {
				if (entity.Physics.LedgeAltitude > 0 ||
					entity.Physics.LedgeTileLocation == checkTile.Location ||
					IsMovingDownLedge(entity, checkTile))
					return false;
			}
			if (entity.Physics.CustomTileCollisionCondition != null)
				return entity.Physics.CustomTileCollisionCondition(checkTile);
			return true;
		}
		
		// Returns true if the entity is able to collide with another entity.
		private bool CanCollideWithEntity(Entity entity, Entity checkEntity) {
			return (checkEntity != entity && checkEntity.Physics.IsEnabled && checkEntity.Physics.IsSolid);
		}

		// Detect clip collisions for an entity, optionally restricting clip direction to a given axis.
		private void DetectClipCollisions(Entity entity) {
			Rectangle2F checkArea = entity.Physics.PositionedCollisionBox;
			foreach (CollisionCheck check in GetCollisions(entity, checkArea))
				DetectClipCollision(entity, check.SolidObject, check.SolidBox);
			
			// Determine which collisions can be resolved later.
			for (int i = 0; i < Directions.Count; i++) {
				CollisionInfoNew collision = entity.Physics.ClipCollisionInfo[i];
				if (collision.IsColliding) {
					collision.IsResolvable = CanResolveCollision(entity, collision);
					if (!collision.IsAllowedClipping)
						entity.Physics.IsColliding = true;
				}
			}
		}
		
		// Detect a clip collision between the entity and a solid object.
		private void DetectClipCollision(Entity entity, object other, Rectangle2F solidBox) {
			// Check if there actually is a collision.
			Rectangle2F entityBox = entity.Physics.PositionedCollisionBox;
			if (entityBox.Intersects(solidBox)) {
				// Determine clip direction.
				int clipDirection = GetCollisionClipDirection(entity, other, solidBox);
			
				// Set or replace the collision info for this clip direction.
				if (clipDirection >= 0) {
					CollisionInfoNew oldCollisionInfo = entity.Physics.ClipCollisionInfo[clipDirection];
					CollisionInfoNew newCollisionInfo = CreateCollisionInfo(entity, other, solidBox, clipDirection);
					if (!oldCollisionInfo.IsColliding || (newCollisionInfo.PenetrationDistance > oldCollisionInfo.PenetrationDistance))
						entity.Physics.ClipCollisionInfo[clipDirection] = newCollisionInfo;
				}
			}
		}

		// Determine the clipping direction for a collision.
		private int GetCollisionClipDirection(Entity entity, object other, Rectangle2F solidBox) {
			Rectangle2F entityBox = entity.Physics.PositionedCollisionBox;
			
			// For moving tiles, use the move direction to determine clip direction.
			Tile tile = other as Tile;
			if (tile != null && tile.IsMoving) {
				if ((solidBox.Center - entityBox.Center).Dot(Directions.ToVector(tile.MoveDirection)) < 0.0f)
					return Directions.Reverse(tile.MoveDirection);
			}

			// Get the nearest direction from the collision intersection to the center of the solid box.
			Vector2F intersectionCenter = Rectangle2F.Intersect(entityBox, solidBox).Center;
			int clipDirection = Directions.NearestFromVector(solidBox.Center - intersectionCenter);
			
			// If the collision can't be resolved, then try to use a direction on the opposite axis.
			CollisionInfoNew testCollision = CreateCollisionInfo(entity, other, solidBox, clipDirection);
			if (!testCollision.IsAllowedClipping && !CanResolveCollision(entity, testCollision)) {
				int newClipDirection;
				int a = 1 - Directions.ToAxis(clipDirection);
				if (entityBox.Center[a] < solidBox.Center[a])
					newClipDirection = (a == Axes.X ? Directions.Right : Directions.Down);
				else
					newClipDirection = (a == Axes.X ? Directions.Left : Directions.Up);
				testCollision = CreateCollisionInfo(entity, other, solidBox, newClipDirection);
				if (testCollision.IsAllowedClipping || CanResolveCollision(entity, testCollision))
					clipDirection = newClipDirection;
			}
			
			return clipDirection;
		}
		
		// Create a clip collision info between an entity and solid object with a specified clip direction.
		private CollisionInfoNew CreateCollisionInfo(Entity entity, object other, Rectangle2F solidBox, int clipDirection) {
			return new CollisionInfoNew() {
				IsColliding						= true,
				Entity							= entity,
				CollidedObject					= other,
				CollisionBox					= solidBox,
				PenetrationDirection			= clipDirection,
				PenetrationDistance				= GetClipPenetration(entity.Physics.PositionedCollisionBox, solidBox, clipDirection),
				MaxAllowedPenetrationDistance	= GetAllowedEdgeClipAmount(entity, other)
			};
		}
		
		// Returns the allowable edge clip amount between an entity and solid object.
		private float GetAllowedEdgeClipAmount(Entity entity, object solidObject) {
			if (!entity.Physics.AllowEdgeClipping)
				return 0.0f;
			if ((solidObject is Tile) && ((Tile) solidObject).IsInMotion)
				return 0.0f;
			return entity.Physics.EdgeClipAmount;
		}

		
		//-----------------------------------------------------------------------------
		// Clip Collision Resolution
		//-----------------------------------------------------------------------------

		// Resolve clip collisions that were not resolved from the previous frame.
		private void ResolvePreviousClipCollisions(Entity entity) {
			for (int i = 0; i < Directions.Count; i++) {
				CollisionInfoNew collision = entity.Physics.ClipCollisionInfo[i];

				if (!collision.IsResolved && CanResolveCollision(entity, collision)) {
					float resolveDistance = Math.Max(0.0f, collision.PenetrationDistance - collision.MaxAllowedPenetrationDistance);
					entity.Position -= Directions.ToVector(collision.PenetrationDirection) * resolveDistance;
				}
				collision.Reset();
				collision.Entity = entity;
				collision.PenetrationDirection = i;
			}
		}
		
		// Resolve clip collisions.
		private void ResolveClipCollisions(Entity entity) {
			// Determine which collisions have priority.
			CollisionInfoNew[] collisions = new CollisionInfoNew[2];
			collisions[Axes.X] = GetPriorityCollision(entity,
				entity.Physics.ClipCollisionInfo[Directions.Right],
				entity.Physics.ClipCollisionInfo[Directions.Left]);
			collisions[Axes.Y] = GetPriorityCollision(entity,
				entity.Physics.ClipCollisionInfo[Directions.Up],
				entity.Physics.ClipCollisionInfo[Directions.Down]);

			for (int i = 0; i < 2; i++) {
				CollisionInfoNew collision = collisions[i];
				if (collision != null && collision.IsResolvable) {
					// Resolve the collision.
					entity.Position += GetPositionalCorrection(collision);
					collision.IsResolved = true;

					// Add to the penetration distance of the opposite collision.
					CollisionInfoNew otherCollision = entity.Physics.ClipCollisionInfo[Directions.Reverse(collision.PenetrationDirection)];
					float resolveDistance = Math.Max(0.0f, collision.PenetrationDistance - collision.MaxAllowedPenetrationDistance);
					otherCollision.PenetrationDistance += resolveDistance;
				}
			}
			
			// Check if the collisions can be only resolved together and NOT separately.
			if (collisions[0] != null && collisions[1] != null &&
				!collisions[0].IsResolvable && !collisions[1].IsResolvable &&
				CanResolveCollisionPair(entity, collisions[0], collisions[1]))
			{
				for (int i = 0; i < 2; i++) {
					CollisionInfoNew collision = collisions[i];
					// Resolve the collision.
					entity.Position += GetPositionalCorrection(collision);
					collision.IsResolved = true;

					// Add to the penetration distance of the opposite collision.
					CollisionInfoNew otherCollision = entity.Physics.ClipCollisionInfo[Directions.Reverse(collision.PenetrationDirection)];
					float resolveDistance = Math.Max(0.0f, collision.PenetrationDistance - collision.MaxAllowedPenetrationDistance);
					otherCollision.PenetrationDistance += resolveDistance;
				}
			}
		}
		
		// Returns the clip collision that has resolution priority (or null if neither are colliding).
		private CollisionInfoNew GetPriorityCollision(Entity entity, CollisionInfoNew collision1, CollisionInfoNew collision2) {
			if (!collision1.IsCollidingAndNotAllowedClipping && !collision2.IsCollidingAndNotAllowedClipping)
				return null;
			else if (collision1.IsCollidingAndNotAllowedClipping && !collision2.IsCollidingAndNotAllowedClipping)
				return collision1;
			else if (collision2.IsCollidingAndNotAllowedClipping && !collision1.IsCollidingAndNotAllowedClipping)
				return collision2;

			// Resolvable collisions over non resolvable collisions.
			if (collision1.IsResolvable && !collision2.IsResolvable)
				return collision1;
			else if (collision2.IsResolvable && !collision1.IsResolvable)
				return collision2;

			// Dynamic collisions over static collisions.
			if (IsSolidObjectDynamic(collision1.CollidedObject) && !IsSolidObjectDynamic(collision2.CollidedObject))
				return collision1;
			if (IsSolidObjectDynamic(collision2.CollidedObject) && !IsSolidObjectDynamic(collision1.CollidedObject))
				return collision2;

			Tile tile1 = collision1.CollidedObject as Tile;
			Tile tile2 = collision2.CollidedObject as Tile;
			Entity entity1 = collision1.CollidedObject as Entity;
			Entity entity2 = collision2.CollidedObject as Entity;

			// Entities over tiles.
			if (entity1 != null && tile2 != null)
				return collision1;
			if (entity2 != null && tile1 != null)
				return collision2;

			// Compare two entities.
			if (entity1 != null && entity2 != null) {
				if (entity1.EntityIndex < entity2.EntityIndex)
					return collision1;
				if (entity2.EntityIndex < entity1.EntityIndex)
					return collision2;
			}

			// Compare two tiles.
			if (tile1 != null && tile2 != null) {
				if (tile1.Position.Y > tile2.Position.Y)
					return collision1;
				else if (tile2.Position.Y > tile1.Position.Y)
					return collision2;
				else if (tile1.Position.X > tile2.Position.X)
					return collision1;
				else if (tile2.Position.X > tile1.Position.X)
					return collision2;
				else if (tile1.Layer > tile2.Layer)
					return collision1;
				else if (tile2.Layer > tile1.Layer)
					return collision2;
			}

			return collision1;
		}
		
		// Get the positional correction needed to resolve a collision.
		private Vector2F GetPositionalCorrection(CollisionInfoNew collision) {
			float resolveDistance = Math.Max(0.0f, collision.PenetrationDistance - collision.MaxAllowedPenetrationDistance);
			return -Directions.ToVector(collision.PenetrationDirection) * resolveDistance;
		}
		
		// Returns true if the given collision needs resolution.
		private bool CanResolveCollision(Entity entity, CollisionInfoNew collision) {
			if (!collision.IsColliding || collision.IsAllowedClipping)
				return false;
			
			// Don't resolve collisions if they are within the allowable edge clip amount.
			if (IsSafeClipping(entity, 1 - Directions.ToAxis(collision.PenetrationDirection), entity.Physics.PositionedCollisionBox, collision.CollidedObject, collision.CollisionBox))
				return false;

			// Static collisions cannot resolve into colliding with other static objects.
			Tile tile = collision.CollidedObject as Tile;
			if (tile != null && !tile.IsInMotion) {
				Vector2F newPosition = entity.Position + GetPositionalCorrection(collision);
				if (IsCollidingAt(entity, newPosition, true))
					return false;
			}

			if ((collision.CollidedObject is Tile) && !entity.Physics.CollideWithWorld)
				return false;
			if ((collision.CollidedObject is Entity) && !entity.Physics.CollideWithEntities)
				return false;

			return true;
		}

		// Returns true if the given clip collision needs resolution.
		private bool CanResolveCollisionPair(Entity entity, CollisionInfoNew collision1, CollisionInfoNew collision2) {
			// Static collisions cannot resolve into colliding with other static objects.
			Tile tile1 = collision1.CollidedObject as Tile;
			Tile tile2 = collision2.CollidedObject as Tile;
			if ((tile1 != null && !tile1.IsInMotion) || (tile2 != null && !tile2.IsInMotion)) {
				Vector2F newPosition = entity.Position;
				newPosition += GetPositionalCorrection(collision1);
				newPosition += GetPositionalCorrection(collision2);
				if (IsCollidingAt(entity, newPosition, true))
					return false;
			}
			return true;
		}

		// Returns true if the entity would be clipping if it were placed at the given position.
		private bool IsCollidingAt(Entity entity, Vector2F position, bool onlyStatic, int clipDirection = -1, float clipDistance = 0.0f) {
			Rectangle2F entityBox = entity.Physics.CollisionBox;
			entityBox.Point += position;
			if (entity.Physics.CollideWithRoomEdge && !((Rectangle2F) roomControl.RoomBounds).Contains(entityBox))
				return true;
			foreach (CollisionCheck check in GetCollisions(entity, entityBox)) {
				if (onlyStatic && IsSolidObjectDynamic(check.SolidObject))
					continue;
				float allowedClipAmount = GetAllowedEdgeClipAmount(entity, check.SolidObject);
				Rectangle2F insetSolidBox = check.SolidBox.Inflated(-allowedClipAmount, -allowedClipAmount);
				if (clipDirection >= 0)
					insetSolidBox.ExtendEdge(Directions.Reverse(clipDirection), allowedClipAmount - clipDistance);
				if (entityBox.Intersects(insetSolidBox))
					return true;
			}
			return false;
		}

		// Returns whether a solid object is considered dynamic (moving tiles or entities).
		private bool IsSolidObjectDynamic(object solidObject) {
			if (solidObject is Tile)
				return ((Tile) solidObject).IsInMotion;
			return (solidObject is Entity);
		}

		
		//-----------------------------------------------------------------------------
		// Post-Clip Collision Stage
		//-----------------------------------------------------------------------------
		
		// Restrict an entity's velocity based on its clip collisions.
		private void ClipVelocity(Entity entity) {
			// Check if the entity is in a deadlock (has collisions but cannot resolve any of them).
			/*entity.Physics.IsDeadlocked = false;
			for (int i = 0; i < Directions.Count; i++) {
				CollisionInfoNew collision = entity.Physics.CollisionInfoNew[i];
				if (collision.IsColliding) {
					if (collision.IsResolvable) {
						entity.Physics.IsDeadlocked = false;
						break;
					}
					else
						entity.Physics.IsDeadlocked = true;
				}
			}
			if (entity.Physics.IsDeadlocked)
				entity.Physics.Velocity = Vector2F.Zero;*/

			// Clip velocities for collisions.
			for (int i = 0; i < Directions.Count; i++) {
				CollisionInfoNew collision = entity.Physics.ClipCollisionInfo[i];
				if (collision.IsCollidingAndNotAllowedClipping)
					ClipEntityVelocity(entity, collision.PenetrationDirection);
			}
		}
		
		// Restrict the entity's velocity in the given direction.
		private void ClipEntityVelocity(Entity entity, int direction) {
			if (direction == Directions.Right && entity.Physics.VelocityX > 0.0f)
				entity.Physics.VelocityX = 0.0f;
			if (direction == Directions.Down && entity.Physics.VelocityY > 0.0f)
				entity.Physics.VelocityY = 0.0f;
			if (direction == Directions.Left && entity.Physics.VelocityX < 0.0f)
				entity.Physics.VelocityX = 0.0f;
			if (direction == Directions.Up && entity.Physics.VelocityY < 0.0f)
				entity.Physics.VelocityY = 0.0f;
		}

		// Check if the entity is being crushed.
		private void CheckCrush(Entity entity) {
			if (!entity.Physics.IsCrushable)
				return;

			// Check for crushing along each axis.
			for (int axis = 0; axis < Axes.Count; axis++) {
				CollisionInfoNew collision1 = entity.Physics.ClipCollisionInfo[axis];
				CollisionInfoNew collision2 = entity.Physics.ClipCollisionInfo[axis + 2];

				// Make sure the entity is colliding between two objects along this axis.
				float penetration = 0.0f;
				if (collision1.IsColliding && collision2.IsColliding) {
					if (collision1.IsResolvable)
						penetration = collision1.PenetrationDistance;
					else if (collision2.IsResolvable)
						penetration = collision2.PenetrationDistance;

					// Check if the gap between the solid objects is small enough to crush.
					if (penetration > 0.0f) {
						float gap = entity.Physics.CollisionBox.Size[axis] - penetration;
						if (gap <= entity.Physics.CrushMaxGapSize)
							entity.OnCrush(axis == Axes.X);
					}
				}
			}
		}
		

		//-----------------------------------------------------------------------------
		// Movement Collisions
		//-----------------------------------------------------------------------------
		
		// Resolve any movement collisions caused by an entity's velocity.
		private void ResolveMovementCollisions(Entity entity) {
			Rectangle2F checkArea = Rectangle2F.Union(
				Rectangle2F.Translate(entity.Physics.CollisionBox, entity.Position),
				Rectangle2F.Translate(entity.Physics.CollisionBox, entity.Position + entity.Physics.Velocity));

			for (int axis = 0; axis < 2; axis++) {
				foreach (CollisionCheck check in GetCollisions(entity, checkArea)) {
					ResolveMovementCollision(entity, axis, check.SolidObject, check.SolidBox);
				}
			}
		}
		
		// Resolve any movement collision between the entity and a solid object
		private void ResolveMovementCollision(Entity entity, int axis, object other, Rectangle2F solidBox) {
			// Setup the entity's translated collision box based on which axis we're checking.
			Rectangle2F entityBox;
			if (axis == Axes.X) {
				entityBox = Rectangle2F.Translate(
					entity.Physics.CollisionBox, entity.X + entity.Physics.Velocity.X, entity.Y);
			}
			else {
				entityBox = Rectangle2F.Translate(
					entity.Physics.CollisionBox, entity.Position + entity.Physics.Velocity);
			}
			
			// Check if there actually is a collision.
			if (!entityBox.Intersects(solidBox))
				return;

			// Determine clipping direction.
			int clipDirection = -1;
			if (axis == Axes.X) {
				if (entityBox.Center.X < solidBox.Center.X)
					clipDirection = Directions.Right;
				else
					clipDirection = Directions.Left;
			}
			else {
				if (entityBox.Center.Y < solidBox.Center.Y)
					clipDirection = Directions.Down;
				else
					clipDirection = Directions.Up;
			}

			// Ignore collision if the entity is already colliding in the opposite
			// direction or if the entity is not moving in the clip direction.
			if (entity.Physics.ClipCollisionInfo[Directions.Reverse(clipDirection)].IsCollidingAndNotAllowedClipping ||
				entity.Physics.Velocity.Dot(Directions.ToVector(clipDirection)) <= 0.0f)
				return;
			
			// Ignore collisions that are within the allowed clipping range.
			if (IsSafeClipping(entity, 1 - axis, entityBox, other, solidBox))
				return;
			
			// Ignore the collision if the entity is clipped into a solid object that shares
			// a clip edge with this object and the entity is also moving parallel with that edge.
			for (int i = 0; i < 2; i++) {
				int dir = Axes.GetOpposite(axis) + (i * 2);
				CollisionInfoNew checkCollision = entity.Physics.ClipCollisionInfo[dir];
				if (checkCollision.IsColliding && AreEdgesAligned(
					checkCollision.CollisionBox, solidBox, Directions.Reverse(dir)))
				{
					return;
				}
			}

			// Resolve the collision.
			if (!entity.Physics.IsColliding && !entity.Physics.ClipCollisionInfo[clipDirection].IsColliding) {
				// Determine the penetration.
				float penetrationDistance = GetClipPenetration(entityBox, solidBox, clipDirection);
				float maxAllowedPenetration = 0.0f;
				if (entity.Physics.Velocity[axis] == 0.0f)
					maxAllowedPenetration = GetAllowedEdgeClipAmount(entity, other);
					
				// Snap the entity's position to the edge of the solid object.
				Vector2F newPos = entity.Position;
				newPos[axis] += entity.Physics.Velocity[axis];
				newPos -= Directions.ToVector(clipDirection) * penetrationDistance;
				if (penetrationDistance <= maxAllowedPenetration)
					newPos += Directions.ToVector(clipDirection) * penetrationDistance;
				entity.Position = newPos;

				entity.Physics.CollisionInfo[clipDirection].SetCollision(other, clipDirection);
			}
			
			// Zero the entity's velocity for this axis.
			Vector2F velocity = entity.Physics.Velocity;
			velocity[axis] = 0.0f;
			entity.Physics.Velocity = velocity;
			
			entity.Physics.MovementCollisions[clipDirection] = true;
			if (!entity.Physics.CollisionInfo[clipDirection].IsColliding)
				entity.Physics.CollisionInfo[clipDirection].SetCollision(other, clipDirection);
			
			// Perform collision dodging.
			bool canDodge = true;
			if (entity is Player) // TODO: this won't work for when player is knocked back
				canDodge = ((Player) entity).Movement.AllowMovementControl && Controls.Arrows[clipDirection].IsDown();
			if (entity.Physics.AutoDodges && canDodge) { 
				if (roomControl.IsSideScrolling && axis == Axes.X &&
					(!(entity is Player) || !((Player) entity).Movement.IsOnSideScrollLadder || ((other is Tile) && ((Tile) other).IsInMotion)) &&
					(entity.IsOnGround || entity.Physics.PreviousCollisionInfo[Directions.Down].IsColliding))
				{
					// Step up onto blocks if it is a small enough distance.
					PerformSideScrollCollisionSnap(entity, clipDirection, other, solidBox);
				}
				else {
					// Auto dodging.
					PerformCollisionDodge(entity, clipDirection, other, solidBox);
				}
			}
		}

		// Attempt to dodge a collision.
		private bool PerformCollisionDodge(Entity entity, int direction, object solidObject, Rectangle2F solidBox) {
			// Only dodge if moving perpendicular to the edge.
			int axis = Directions.ToAxis(direction);
			if (Math.Abs(entity.Physics.Velocity[1 - axis]) > 0.001f)
				return false;

			// Can't dodge moving tiles.
			if ((solidObject is Tile) && ((Tile) solidObject).IsMoving)
				return false;

			Rectangle2F entityBox = entity.Physics.PositionedCollisionBox;
			float penetrationDistance = GetClipPenetration(
				entity.Physics.PositionedCollisionBox, solidBox, direction);

			// Check dodging for both edges of the solid object.
			for (int side = 0; side < 2; side++) {
				int moveDirection = (direction + (side == 0 ? 1 : 3)) % 4;
				float distanceToEdge = Math.Abs(entityBox.GetEdge(
					Directions.Reverse(moveDirection)) - solidBox.GetEdge(moveDirection));
				
				// Check if the distance to the edge is within dodge range.
				if (distanceToEdge <= entity.Physics.AutoDodgeDistance) {
					float moveAmount = Math.Min(entity.Physics.AutoDodgeSpeed, distanceToEdge);

					Vector2F nextPosition = GMath.Round(entity.Position) +
						(Directions.ToVector(moveDirection) * moveAmount);
					Vector2F goalPosition = entity.Position + Directions.ToVector(direction) +
						(Directions.ToVector(moveDirection) * distanceToEdge);
					
					// Make sure the entity is not colliding when placed at the solid object's edge.
					if (!IsCollidingAt(entity, nextPosition, false, direction, penetrationDistance) &&
						!IsCollidingAt(entity, goalPosition, false, direction, penetrationDistance))
					{
						entity.Position += Directions.ToVector(moveDirection) * moveAmount;
						entity.Physics.CollisionInfo[direction].IsAutoDodged = true;
						//entity.Physics.MovementCollisions[direction] = false; // TODO: Figure out complications for removing this.
						return true;
					}
				}
			}

			return false;
		}
		
		// Attempt to snap a collision.
		private bool PerformSideScrollCollisionSnap(Entity entity, int direction, object solidObject, Rectangle2F solidBox) {
			Rectangle2F entityBox = entity.Physics.PositionedCollisionBox;
			float penetrationDistance = GetClipPenetration(
				entity.Physics.PositionedCollisionBox, solidBox, direction);
			float distanceToEdge = entityBox.Bottom - solidBox.Top;
			//Vector2F goalPosition = entity.Position + Directions.ToVector(direction) +
				//(Directions.ToVector(Directions.Up) * distanceToEdge);
			
			Vector2F goalPosition = entity.Position +
				Directions.ToVector(direction) -
				new Vector2F(0.0f, distanceToEdge);

			// Check if the distance to the edge is within dodge range.
			// Make sure the entity is not colliding when placed at the solid object's edge.
			if (distanceToEdge <= entity.Physics.AutoDodgeDistance &&
				!IsCollidingAt(entity, goalPosition, false, direction, penetrationDistance))
			{
				entity.Position = goalPosition;
				entity.Physics.MovementCollisions[Directions.Down] = true;
				entity.Physics.CollisionInfo[Directions.Down].SetCollision(solidObject, Directions.Down);
				return true;
			}
			return false;
		}
		
		// Returns true if the entity allowably clipping the given collision.
		private bool IsSafeClipping(Entity entity, int axis, Rectangle2F entityBox, object other, Rectangle2F solidBox) {
			if (entity.Physics.AllowEdgeClipping) {
				float allowedEdgeClipAmount = GetAllowedEdgeClipAmount(entity, other);
				float penetration = Math.Min(
					solidBox.BottomRight[axis] - entityBox.TopLeft[axis],
					entityBox.BottomRight[axis] - solidBox.TopLeft[axis]);
				return (penetration <= allowedEdgeClipAmount);
			}
			return false;
		}

		// Calculate a collision's penetration distance in the given direction.
		public float GetClipPenetration(Rectangle2F entityBox, Rectangle2F solidBox, int clipDirection) {
			if (clipDirection == Directions.Right)
				return entityBox.Right - solidBox.Left;
			else if (clipDirection == Directions.Up)
				return solidBox.Bottom - entityBox.Top;
			else if (clipDirection == Directions.Left)
				return solidBox.Right - entityBox.Left;
			else if (clipDirection == Directions.Down)
				return entityBox.Bottom - solidBox.Top;
			return 0.0f;
		}
		
		// Returns true if two rectangles share an edge in the given direction.
		public static bool AreEdgesAligned(Rectangle2F box1, Rectangle2F box2, int edgeDirection) {
			return Math.Abs(box1.GetEdge(edgeDirection) - box2.GetEdge(edgeDirection)) < 0.1f;
		}
		

		//-----------------------------------------------------------------------------
		// Side Scrolling Ladder Collisions
		//-----------------------------------------------------------------------------

		// Check the player's side-scrolling ladder collisions.
		private void CheckPlayerLadderClimbing(Player player) {
			if (!roomControl.IsSideScrolling)
				return;

			bool onLadderPrev = player.Movement.IsOnSideScrollLadder;

			// Check if the player is touching a ladder, and whether to begin climbing.
			player.Movement.HighestSideScrollLadderTile = 
				GetHighestLadder(player, player.PreviousPosition);
			if (player.Movement.HighestSideScrollLadderTile == null)
				player.Movement.IsOnSideScrollLadder = false;
			else if (player.Physics.VelocityY >= 0.0f && player.Movement.AllowMovementControl && Controls.Up.IsDown())
				player.Movement.IsOnSideScrollLadder = true;
					
			// Collide with ladders.
			Rectangle2F ladderCheckArea = Rectangle2F.Union(
				Rectangle2F.Translate(player.Physics.CollisionBox, player.Position),
				Rectangle2F.Translate(player.Physics.CollisionBox, player.Position + player.Physics.Velocity));
			foreach (Tile tile in RoomControl.TileManager.GetTilesTouching(ladderCheckArea)) {
				if (tile.IsLadder)
					CheckLadderCollision(player, tile);
			}
				
			// Check for climbing off the top of a ladder.
			if (player.Movement.IsOnSideScrollLadder && player.Movement.HighestSideScrollLadderTile != null)
				CheckClimbingOffLadderTop(player, player.Movement.HighestSideScrollLadderTile);

			// Check for climbing to the bottom of a ladder.
			if (player.Physics.CollisionInfo[Directions.Down].IsCollidingAndNotAutoDodged &&
				player.Movement.AllowMovementControl && Controls.Down.IsDown())
				player.Movement.IsOnSideScrollLadder = false;

			// Check if the player is no longer on a ladder.
			if (player.Movement.IsOnSideScrollLadder) {
				player.Movement.HighestSideScrollLadderTile =
					GetHighestLadder(player, player.Position + player.Physics.Velocity);
				if (player.Movement.HighestSideScrollLadderTile == null)
					player.Movement.IsOnSideScrollLadder = false;
			}

			// If the player has begun climbing, set his Y-velocity to zero (no gravity).
			if (!onLadderPrev && player.Movement.IsOnSideScrollLadder)
				player.Physics.VelocityY = 0.0f;

			// If the player has stopped climbing, any downwards safe clipping should be
			// turned into an actual collision now that gravity is affecting the player again.
			if (onLadderPrev && !player.Movement.IsOnSideScrollLadder) {
				if (player.Physics.ClipCollisionInfo[Directions.Down].IsAllowedClipping &&
					!player.Physics.CollisionInfo[Directions.Down].IsColliding)
				{
					player.Physics.MovementCollisions[Directions.Down] = true;
					player.Physics.CollisionInfo[Directions.Down].SetCollision(
						player.Physics.ClipCollisionInfo[Directions.Down].CollidedObject, Directions.Down);
				}
			}
		}
		
		// Return the top-most ladder the player is colliding with when placed at the given position.
		private Tile GetHighestLadder(Player player, Vector2F position) {
			Rectangle2F pollLadderBox = player.Movement.ClimbCollisionBox;
			pollLadderBox.Point += position;
			Tile highestLadder = null;
			foreach (Tile tile in RoomControl.TileManager.GetTilesTouching(pollLadderBox)) {
				if (tile.IsLadder && (highestLadder == null || tile.Bounds.Top < highestLadder.Bounds.Top))
					highestLadder = tile;
			}
			return highestLadder;
		}

		// Check for climbing off the top of the ladder.
		private void CheckClimbingOffLadderTop(Player player, Tile ladderTile) {
			Rectangle2F entityBoxPrev		= player.Physics.PositionedCollisionBox;
			Rectangle2F entityBox			= Rectangle2F.Translate(entityBoxPrev, player.Physics.Velocity);
			Rectangle2F solidBox			= ladderTile.Bounds;
			Rectangle2F pollLadderBoxPrev	= player.Movement.ClimbCollisionBox;
			pollLadderBoxPrev.Point += player.Position;

			if (pollLadderBoxPrev.Intersects(solidBox) &&
				entityBoxPrev.Bottom > solidBox.Top &&
				entityBox.Bottom <= solidBox.Top)
			{
				player.Physics.VelocityY = 0.0f;
				player.Y = solidBox.Top - player.Physics.CollisionBox.Bottom;
				player.Physics.MovementCollisions[Directions.Down] = true;
				if (!player.Physics.CollisionInfo[Directions.Down].IsColliding)
					player.Physics.CollisionInfo[Directions.Down].SetCollision(ladderTile, Directions.Down);
				player.Movement.IsOnSideScrollLadder = false;
			}
		}
		
		private void CheckLadderCollision(Player player, Tile tile) {
			Rectangle2F solidBox			= tile.Bounds;
			Rectangle2F entityBoxPrev		= player.Physics.PositionedCollisionBox;
			Rectangle2F entityBox			= Rectangle2F.Translate(entityBoxPrev, player.Physics.Velocity);
			Rectangle2F pollLadderBox		= player.Movement.ClimbCollisionBox;
			Rectangle2F pollLadderBoxPrev	= pollLadderBox;
			pollLadderBox.Point		+= player.Position + player.Physics.Velocity;
			pollLadderBoxPrev.Point	+= player.Position;
			
			// Check if this tile is a top-most ladder.
			Tile checkAboveTile = player.RoomControl.TileManager.GetSurfaceTile(tile.Location - new Point2I(0, 1));
			bool isTopLadder = (checkAboveTile == null || !checkAboveTile.IsLadder);

			// Check if stepping off of a solid object and onto the ladder.
			// Make sure the player is not on a flat surface aligned with the ladder top.
			if (!player.Movement.IsOnSideScrollLadder &&
				pollLadderBox.Intersects(solidBox) &&
				player.Physics.PreviousCollisionInfo[Directions.Down].IsColliding &&
				player.Physics.PreviousCollisionInfo[Directions.Down].Tile != tile &&
				!player.Physics.CollisionInfo[Directions.Down].IsColliding &&
				player.Physics.VelocityY >= 0.0f &&
					(!isTopLadder || entityBoxPrev.Bottom > solidBox.Top ||
					player.Physics.PreviousCollisionInfo[Directions.Down].Tile == null ||
					player.Physics.PreviousCollisionInfo[Directions.Down].Tile.Bounds.Top != solidBox.Top))
			{
				player.Movement.IsOnSideScrollLadder = true;
				return;
			}
			
			// Make sure the player:
			//  - This tile is a top-most ladder.
			//  - The player isn't already climbing a ladder.
			//  - The player is touching the ladder,
			//  - The player was previously above the ladder,
			//  - The player and isn't standing on something else with clipping.
			if (isTopLadder &&
				!player.Movement.IsOnSideScrollLadder &&
				entityBox.Intersects(solidBox) &&
				entityBoxPrev.Bottom <= solidBox.Top &&
				!player.Physics.ClipCollisionInfo[Directions.Down].IsColliding)
			{
				// If holding the [Down] button, then begin climbing the ladder instead.
				if (player.Movement.AllowMovementControl && Controls.Down.IsDown() && !Controls.Up.IsDown()) {
					player.Movement.IsOnSideScrollLadder = true;
				}
				else {
					// Collide with the top of the ladder.
					player.Physics.VelocityY = 0.0f;
					player.Y = solidBox.Top - player.Physics.CollisionBox.Bottom;
					player.Physics.MovementCollisions[Directions.Down] = true;
					if (!player.Physics.CollisionInfo[Directions.Down].IsColliding)
						player.Physics.CollisionInfo[Directions.Down].SetCollision(tile, Directions.Down);
				}
			}
		}
		
		
		//-----------------------------------------------------------------------------
		// Room Boundaries
		//-----------------------------------------------------------------------------

		// Check collisions with the room's edges.
		private void CheckRoomEdgeCollisions(Entity entity) {
			if (!entity.Physics.CollideWithRoomEdge && !entity.Physics.ReboundRoomEdge)
				return;
			
			Rectangle2F collisionBox = entity.Physics.CollisionBox;
			Rectangle2F entityBox = entity.Physics.CollisionBox;
			entityBox.Point += entity.Position + entity.Physics.Velocity;
			Rectangle2F roomBounds = roomControl.RoomBounds;

			/*
			for (int i = 0; i < Directions.Count; i++) {
				int axis = Directions.ToAxis(i);
				float penetration = GetClipPenetration(roomBounds, entityBox, i);
				float distanceOutside = entityBox.Size[axis] - penetration;

				if (distanceOutside > 0.0f) {
					entity.Position -= Directions.ToVector(i) * distanceOutside;

					entity.Physics.IsColliding = true;
					entity.Physics.CollisionInfo[i].SetRoomEdgeCollision(i);
				}
			}*/

			if (entityBox.Left < roomBounds.Left) {
				entity.Physics.IsColliding = true;
				entity.X = roomBounds.Left - collisionBox.Left;
				if (entity.Physics.ReboundRoomEdge && entity.Physics.ReboundVelocity.X == 0.0f)
					entity.Physics.ReboundVelocity = new Vector2F(-entity.Physics.VelocityX, entity.Physics.ReboundVelocity.Y);
				entity.Physics.VelocityX = 0;
				entity.Physics.CollisionInfo[Directions.Left].SetRoomEdgeCollision(Directions.Left);
			}
			else if (entityBox.Right > roomBounds.Right) {
				entity.Physics.IsColliding = true;
				entity.X = roomBounds.Right - collisionBox.Right;
				if (entity.Physics.ReboundRoomEdge && entity.Physics.ReboundVelocity.X == 0.0f)
					entity.Physics.ReboundVelocity = new Vector2F(-entity.Physics.VelocityX, entity.Physics.ReboundVelocity.Y);
				entity.Physics.VelocityX = 0;
				entity.Physics.CollisionInfo[Directions.Right].SetRoomEdgeCollision(Directions.Right);
			}
			if (entityBox.Top < roomBounds.Top) {
				entity.Physics.IsColliding = true;
				entity.Y = roomBounds.Top - collisionBox.Top;
				if (entity.Physics.ReboundRoomEdge && entity.Physics.ReboundVelocity.Y == 0.0f)
					entity.Physics.ReboundVelocity = new Vector2F(entity.Physics.ReboundVelocity.X, -entity.Physics.VelocityY);
				entity.Physics.VelocityY = 0;
				entity.Physics.CollisionInfo[Directions.Up].SetRoomEdgeCollision(Directions.Up);
			}
			else if (entityBox.Bottom > roomBounds.Bottom) {
				entity.Physics.IsColliding = true;
				entity.Y = roomBounds.Bottom - collisionBox.Bottom;
				if (entity.Physics.ReboundRoomEdge && entity.Physics.ReboundVelocity.Y == 0.0f)
					entity.Physics.ReboundVelocity = new Vector2F(entity.Physics.ReboundVelocity.X, -entity.Physics.VelocityY);
				entity.Physics.VelocityY = 0;
				entity.Physics.CollisionInfo[Directions.Down].SetRoomEdgeCollision(Directions.Down);
			}
		}

		// Check if destroyed outside room.
		private void CheckOutsideRoomBounds(Entity entity) {
			if (entity.Physics.IsDestroyedOutsideRoom &&
				!entity.RoomControl.RoomBounds.Contains(entity.Position))
			{
				entity.Destroy();
				return;
			}
		}
		
		
		//-----------------------------------------------------------------------------
		// Z-Dynamics
		//-----------------------------------------------------------------------------

		public void UpdateEntityZPosition(Entity entity) {

			if (IsSideScrolling) {
				// Convert any nonzero Z-position into Y-position.
				if (entity.ZPosition != 0.0f) {
					entity.Y -= entity.ZPosition;
					entity.ZPosition = 0.0f;
				}
				
				if (entity.Physics.HasGravity && !entity.Physics.OnGroundOverride) {
					// Zero jump speed if the entity is colliding below.
					if (entity.Physics.ZVelocity < 0.0f && entity.Physics.CollisionInfo[Directions.Down].IsColliding)
						entity.Physics.ZVelocity = 0.0f;
					// Zero jump speed if the entity is colliding above.
					// NOTE: this doesn't actually happen in the real game, which is strange.
					//else if (entity.Physics.ZVelocity > 0.0f && entity.Physics.CollisionInfo[Directions.Up].IsColliding)
						//entity.Physics.ZVelocity = 0.0f;
					// Integrate gravity.
					entity.Physics.ZVelocity -= entity.Physics.Gravity;
					// Limit to maximum fall speed.
					if (entity.Physics.ZVelocity < -entity.Physics.MaxFallSpeed && entity.Physics.MaxFallSpeed >= 0.0f)
						entity.Physics.ZVelocity = -entity.Physics.MaxFallSpeed;
					// Convert Z-velocity to Y-velocity.
					entity.Physics.VelocityY = -entity.Physics.ZVelocity;
				}
				else {
					entity.Physics.ZVelocity = 0.0f;
				}
			}
			else if (entity.ZPosition > 0.0f || entity.Physics.ZVelocity != 0.0f) {
				// Integrate gravity acceleration.
				if (entity.Physics.HasGravity && !entity.Physics.OnGroundOverride) {
					entity.Physics.ZVelocity -= entity.Physics.Gravity;
					if (entity.Physics.ZVelocity < -entity.Physics.MaxFallSpeed && entity.Physics.MaxFallSpeed >= 0.0f)
						entity.Physics.ZVelocity = -entity.Physics.MaxFallSpeed;
				}

				// Integrate z-velocity.
				entity.ZPosition += entity.Physics.ZVelocity;

				// Check if landed on the ground.
				if (entity.ZPosition <= 0.0f)
					LandEntity(entity);
			}
			else
				entity.Physics.ZVelocity = 0.0f;
		}

		public void LandEntity(Entity entity) {
			// Check if landed in a hazard surface.
			CheckHazardSurface(entity);
			if (entity.IsDestroyed)
				return;

			if (entity.Physics.Bounces && entity.Physics.ZVelocity < -1.0f) {
				// Bounce back into the air.
				entity.ZPosition = 0.1f;
				entity.Physics.ZVelocity = -entity.Physics.ZVelocity * 0.5f;

				// Half the lateral velocity upon bouncing.
				if (entity.Physics.Velocity.Length > 0.25f)
					entity.Physics.Velocity *= 0.5f;
				else
					entity.Physics.Velocity = Vector2F.Zero;
				
				entity.OnBounce();
			}
			else {
				// Stay on the ground.
				entity.ZPosition = 0.0f;
				entity.Physics.ZVelocity = 0.0f;

				if (entity.Physics.Bounces) {
					entity.Physics.Velocity = Vector2F.Zero;
					entity.OnBounce();
				}

				entity.OnLand();
			}
		}
		

		//-----------------------------------------------------------------------------
		// Surfaces
		//-----------------------------------------------------------------------------
		
		// Check the surface tile beneath the entity.
		private void CheckSurfaceTile(Entity entity) {
			// Find the surface tile underneath the entity.
			entity.Physics.TopTile = roomControl.TileManager
				.GetSurfaceTileAtPosition(entity.Position, entity.Physics.MovesWithPlatforms);

			// Check if the surface is moving.
			if (entity.Physics.TopTile != null) {
				if (entity.Physics.MovesWithPlatforms)
					entity.Physics.Velocity += entity.Physics.TopTile.Velocity;
				if (entity.Physics.MovesWithConveyors && entity.Physics.IsOnGround)
					entity.Physics.Velocity += entity.Physics.TopTile.ConveyorVelocity;
			}
			
			// Check if surface tile is a hazardous (water/lava/hole).
			CheckHazardSurface(entity);
		}
		
		// Check if the entity is sitting on a hazardous surface (water/lava/hole).
		private void CheckHazardSurface(Entity entity) {
			if (entity.Physics.IsInHole)
				entity.OnFallInHole();
			else if (entity.Physics.IsInWater)
				entity.OnFallInWater();
			else if (entity.Physics.IsInLava)
				entity.OnFallInLava();
		}


		//-----------------------------------------------------------------------------
		// Ledge Passing
		//-----------------------------------------------------------------------------
		
		// Update ledge passing, handling changes in altitude.
		public void CheckLedges(Entity entity) {
			if (!entity.Physics.PassOverLedges)
				return;

			Point2I prevLocation = entity.RoomControl.GetTileLocation(entity.PreviousPosition + entity.Physics.CollisionBox.Center);
			Point2I location = entity.RoomControl.GetTileLocation(entity.Position + entity.Physics.CollisionBox.Center);

			// When moving over a new tile, check its ledge state.
			if (location != prevLocation) {
				entity.Physics.LedgeTileLocation = new Point2I(-1, -1);

				if (entity.RoomControl.IsTileInBounds(location)) {
					Tile tile = entity.RoomControl.GetTopTile(location);

					if (tile != null && tile.IsLedge) {
						entity.Physics.LedgeTileLocation = location;
						// Adjust ledge altitude.
						if (IsMovingUpLedge(entity, tile))
							entity.Physics.LedgeAltitude--;
						else if (IsMovingDownLedge(entity, tile))
							entity.Physics.LedgeAltitude++;
					}
				}
			}
		}

		// Returns true if the entity moving down the ledge.
		public bool IsMovingDownLedge(Entity entity, Tile ledgeTile) {
			return entity.Physics.Velocity.Dot(Directions.ToVector(ledgeTile.LedgeDirection)) > 0.0f;
		}

		// Returns true if the entity moving up the ledge.
		public bool IsMovingUpLedge(Entity entity, Tile ledgeTile) {
			return entity.Physics.Velocity.Dot(Directions.ToVector(ledgeTile.LedgeDirection)) < 0.0f;
		}

		
		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public RoomControl RoomControl {
			get { return roomControl; }
		}

		public bool IsSideScrolling {
			get { return roomControl.IsSideScrolling; }
		}
	}
}