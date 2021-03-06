﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Graphics.Sprites;
using ZeldaOracle.Game.Entities.Collisions;
using ZeldaOracle.Game.Entities.Effects;
using ZeldaOracle.Game.Entities.Units;
using ZeldaOracle.Game.Items;
using ZeldaOracle.Game.Main;
using ZeldaOracle.Game.Tiles;

namespace ZeldaOracle.Game.Entities.Players.States {
	public class PlayerHoldSwordState : PlayerState {
		
		// Collision boxes for the the sword.
		private readonly Rectangle2I[] SWORD_COLLISION_BOXES = {
			new Rectangle2I(8 - 2, 0, 14, 8),
			new Rectangle2I(-8, -8 - 12, 8, 14),
			new Rectangle2I(-8 - 12, 0, 14, 8),
			new Rectangle2I(-1, 8, 8, 14)
		};

		private Animation weaponAnimation;
		private ItemWeapon weapon;
		private int chargeTimer;
		private Direction direction;
		protected UnitTool playerTool;


		//-----------------------------------------------------------------------------
		// Constants
		//-----------------------------------------------------------------------------

		private const int ChargeTime = 40;


		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		public PlayerHoldSwordState() {
			weaponAnimation	= GameData.ANIM_SWORD_HOLD;
			weapon			= null;
			chargeTimer		= 0;
			direction		= Direction.Right;
			playerTool		= null;

			StateParameters.ProhibitEnteringMinecart	= true;
			StateParameters.EnableStrafing				= true;
			StateParameters.ProhibitPushing				= true;
		}
		
		
		//-----------------------------------------------------------------------------
		// Internal methods
		//-----------------------------------------------------------------------------

		public void Stab(bool continueHoldingSword) {
			player.SwordStabState.Weapon = weapon;
			player.SwordStabState.ContinueHoldingSword = continueHoldingSword;
			player.BeginWeaponState(player.SwordStabState);
		}

		private void StabTile(Tile tile) {
			if (player.IsOnGround) {
				tile.OnSwordHit(weapon);

				// Create cling effect
				if (!tile.IsDestroyed && tile.ClingWhenStabbed) {
					Effect clingEffect = new EffectCling(true);
					Vector2F pos = player.Center + direction.ToVector(13);
					player.RoomControl.SpawnEntity(clingEffect, pos);
					AudioSystem.PlaySound(GameData.SOUND_EFFECT_CLING);
				}
			}

			// Begin the player stab state
			player.SwordStabState.Weapon = weapon;
			player.SwordStabState.ContinueHoldingSword = true;
			player.BeginWeaponState(player.SwordStabState);
		}

		private void OnStopHolding() {
			if (chargeTimer >= ChargeTime) {
				player.SpinSwordState.Weapon = weapon;
				player.BeginWeaponState(player.SpinSwordState);
			}
			else
				End();
		}


		//-----------------------------------------------------------------------------
		// Overridden methods
		//-----------------------------------------------------------------------------

		public override void OnBegin(PlayerState previousState) {
			// Equip the sword tool
			player.EquipTool(player.ToolSword);
			player.ToolSword.Interactions.InteractionType = InteractionType.Sword;
			player.ToolSword.AnimationPlayer.SubStripIndex = direction;

			chargeTimer = 0;
			direction = player.Direction;
		}
		
		public override void OnEnd(PlayerState newState) {
			// Unequip the sword tool
			if (newState != player.SpinSwordState)
				player.UnequipTool(player.ToolSword);
		}

		public override void Update() {
			base.Update();
			
			direction = player.Direction;

			Rectangle2I box = SWORD_COLLISION_BOXES[direction];
			box.Point += (Point2I) player.CenterOffset;
			player.ToolSword.CollisionBox = box;

			// Charge up the sword
			chargeTimer++;
			if (chargeTimer == ChargeTime) {
				player.ToolSword.AnimationPlayer.SetAnimation(
					GameData.ANIM_SWORD_CHARGED);
				AudioSystem.PlaySound(GameData.SOUND_SWORD_CHARGE);
			}

			// Release the sword button (spin if charged)
			if ((!weapon.IsEquipped || !weapon.IsButtonDown()) && 
				!player.StateParameters.ProhibitReleasingSword)
			{
				OnStopHolding();
			}
			
			// Check for tiles to stab
			else if (player.Movement.IsMoving) {
				Collision collision = player.Physics
					.GetCenteredCollisionInDirection(player.Direction);
				if (collision != null && collision.IsTile)
					StabTile(collision.Tile);
			}
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public Animation WeaponAnimation {
			get { return weaponAnimation; }
			set { weaponAnimation = value; }
		}

		public ItemWeapon Weapon {
			get { return weapon; }
			set { weapon = value; }
		}
	}
}
