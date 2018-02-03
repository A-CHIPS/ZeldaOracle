﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ZeldaOracle.Common.Input;
using ZeldaOracle.Common.Content;
using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Scripting;
using ZeldaOracle.Game.Entities;
using ZeldaOracle.Game.Entities.Monsters;
using ZeldaOracle.Game.Entities.Players;
using ZeldaOracle.Game.GameStates;
using ZeldaOracle.Game.GameStates.RoomStates;
using ZeldaOracle.Game.GameStates.Transitions;
using ZeldaOracle.Game.Control.Menus;
using ZeldaOracle.Game.Debug;
using ZeldaOracle.Game.Items.Rewards;
using ZeldaOracle.Game.Main;
using ZeldaOracle.Game.Tiles;
using ZeldaOracle.Game.Tiles.Custom;
using ZeldaOracle.Game.Tiles.ActionTiles;
using ZeldaOracle.Game.Worlds;
using ZeldaOracle.Game.Tiles.Internal;

namespace ZeldaOracle.Game.Control {

	[Flags]
	public enum RoomDrawing {
		None = 0,
		DrawBelow = 0x1,
		DrawAbove = 0x2,
		DrawAll = 0x3
	}

	// Handles the main Zelda gameplay within a room.
	public class RoomControl : GameState, ZeldaAPI.Room {

		private Room				room;
		private Point2I				roomLocation;
		private Dungeon				dungeon;
		private List<Entity>		entities;
		private List<ActionTile>	actionTiles;
		private ViewControl			viewControl;
		private int					requestedTransitionDirection;
		private int					entityCount;
		private RoomGraphics		roomGraphics;
		private RoomPhysics			roomPhysics;
		private TileManager			tileManager;
		private bool				allMonstersDead;
		private int					entityIndexCounter;
		private bool				isSideScrolling;
		private bool				isUnderwater;
		private RoomVisualEffect	visualEffect;
		private RoomVisualEffect	visualEffectUnderwater;
		private bool				disableVisualEffect;

		private event Action<Player>	eventPlayerRespawn;
		private event Action<int>		eventRoomTransitioning;

		private Palette				tilePaletteOverride;
		private Palette				entityPaletteOverride;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public RoomControl() {
			room					= null;
			dungeon					= null;
			roomLocation			= Point2I.Zero;
			entities				= new List<Entity>();
			actionTiles				= new List<ActionTile>();
			viewControl				= new ViewControl();
			tileManager				= new TileManager(this);
			roomGraphics			= new RoomGraphics(this);
			roomPhysics				= new RoomPhysics(this);
			requestedTransitionDirection = 0;
			eventPlayerRespawn		= null;
			eventRoomTransitioning	= null;
			entityCount				= 0;
			entityIndexCounter		= 0;
			isSideScrolling			= false;
			isUnderwater			= true;
			visualEffect			= null;
			disableVisualEffect		= false;
			tilePaletteOverride		= null;
			entityPaletteOverride	= null;

			visualEffectUnderwater	= new RoomVisualEffect();
			visualEffectUnderwater.RoomControl = this;
		}
		

		//-----------------------------------------------------------------------------
		// Tile Accessors
		//-----------------------------------------------------------------------------

		// Return an enumerable list of tiles.
		public IEnumerable<Tile> GetTiles() {
			return tileManager.GetTiles();
		}
		
		// Return an enumerable list of tiles in the given grid based area.
		public IEnumerable<Tile> GetTilesInArea(Rectangle2I area) {
			return tileManager.GetTilesInArea(area);
		}
		
		// Return an enumerable list of top tiles in the given grid based area.
		public IEnumerable<Tile> GetTopTilesInArea(Rectangle2I area) {
			return tileManager.GetTopTilesInArea(area);
		}
		
		// Return the tile at the given location (can return null).
		public Tile GetTile(Point2I location, int layer) {
			return tileManager.GetTile(location, layer);
		}

		// Return the tile at the given location (can return null).
		public Tile GetTile(int x, int y, int layer) {
			return tileManager.GetTile(x, y, layer);
		}

		// Return the tile at the given location that's on the highest layer.
		public Tile GetTopTile(int x, int y) {
			return tileManager.GetTopTile(x, y);
		}

		// Return the tile at the given location that's on the highest layer.
		public Tile GetTopTile(Point2I location) {
			return tileManager.GetTopTile(location);
		}
		
		// Return true if the given tile location is inside the room.
		public bool IsTileInBounds(Point2I location, int layer = 0) {
			return tileManager.IsTileInBounds(location, layer);
		}

		// Return the tile location that the given position in pixels is situated in.
		public Point2I GetTileLocation(Vector2F position) {
			return tileManager.GetTileLocation(position);
		}

		// inflateAmount inflates the output rectangle.
		public Rectangle2I GetTileAreaFromRect(Rectangle2F rect, int inflateAmount = 0) {
			return tileManager.GetTileAreaFromRect(rect, inflateAmount);
		}

		public ActionTile FindActionTile(ActionTileDataInstance data) {
			for (int i = 0; i < actionTiles.Count; i++) {
				if (actionTiles[i].ActionData == data)
					return actionTiles[i];
			}
			return null;
		}

		public bool IsTileSpawned(TileDataInstance tileDataInstance) {
			return GetTiles().Any(t => t.TileDataOwner == tileDataInstance);
		}

		public bool IsActionTileSpawned(ActionTileDataInstance actionTileDataInstance) {
			return actionTiles.Any(t => t.ActionData == actionTileDataInstance);
		}
		

		//-----------------------------------------------------------------------------
		// Tile & Entity Management
		//-----------------------------------------------------------------------------
		
		// Use this for spawning entites at runtime.
		public void SpawnEntity(Entity e) {
			e.EntityIndex = ++entityIndexCounter;
			e.Initialize(this);
			entities.Add(e);
		}
		
		// Use this for spawning entites at a position at runtime.
		public void SpawnEntity(Entity e, Vector2F position) {
			e.Position = position;
			SpawnEntity(e);
		}
		
		// Use this for spawning entites at a position at runtime.
		public void SpawnEntity(Entity e, Vector2F position, float zPosition) {
			e.Position	= position;
			e.ZPosition	= zPosition;
			SpawnEntity(e);
		}
		
		// Spawn a tile if it isn't already spawned.
		public void SpawnTile(TileDataInstance tileData, bool staySpawned) {
			if (!IsTileSpawned(tileData)) {
				TileSpawnOptions spawnOptions = tileData.SpawnOptions;
				if (spawnOptions.PoofEffect) {
					Tile tile = new AppearingTile(tileData, spawnOptions);
					PlaceTile(tile, tileData.Location, tileData.Layer);
				}
				else {
					Tile tile = Tile.CreateTile(tileData);
					PlaceTile(tile, tileData.Location, tileData.Layer);
				}
			}
			if (staySpawned)
				tileData.Properties.Set("enabled", true);
		}
		
		// Spawn an action tile if it isn't already spawned.
		public void SpawnActionTile(ActionTileDataInstance actionTileData, bool staySpawned) {
			if (!IsActionTileSpawned(actionTileData)) {
				ActionTile tile = ActionTile.CreateAction(actionTileData);
				AddActionTile(tile);
			}
			if (staySpawned)
				actionTileData.Properties.Set("enabled", true);
		}
		
		// Place a tile in the tile grid at the given location and layer.
		public void PlaceTile(Tile tile, Point2I location, int layer) {
			tileManager.PlaceTile(tile, location, layer);
		}
		
		// Place a tile in highest empty layer at the given location.
		// Returns true if there was an empty space to place the tile.
		public bool PlaceTileOnHighestLayer(Tile tile, Point2I location) {
			return tileManager.PlaceTileOnHighestLayer(tile, location);
		}

		// Use this for placing tiles at runtime.
		public void PlaceTile(Tile tile, int x, int y, int layer, bool initializeTile = true) {
			tileManager.PlaceTile(tile, x, y, layer, initializeTile);
		}

		// Remove a tile from the room.
		public void RemoveTile(Tile tile) {
			tileManager.RemoveTile(tile);
		}

		// Put an action tile into the room.
		public void AddActionTile(ActionTile actionTile) {
			actionTile.Initialize(this);
			actionTiles.Add(actionTile);
		}

		// Move the given tile to a new location.
		public void MoveTile(Tile tile, Point2I newLocation, int newLayer) {
			tileManager.MoveTile(tile, newLocation, newLayer);
		}


		//-----------------------------------------------------------------------------
		// Room Setup & Destroy
		//-----------------------------------------------------------------------------
		
		public void BeginRoom() {
			BeginRoom(room);
		}

		public void BeginRoom(Room room) {
			this.room				= room;
			this.roomLocation		= room.Location;
			this.dungeon			= room.Dungeon;
			this.isSideScrolling	= room.Zone.IsSideScrolling;
			this.isUnderwater		= room.Zone.IsUnderwater;
			if (this.isUnderwater)
				visualEffect = visualEffectUnderwater;
			else
				visualEffect = null;

			// Discover the room.
			room.IsDiscovered = true;
			room.Level.IsDiscovered = true;

			// Clear action tiles.
			actionTiles.Clear();

			// Clear all entities from the old room (except for the player).
			entities.Clear();
			if (Player != null) {
				Player.Initialize(this);
				Player.EntityIndex = 0;
				entities.Add(Player);
			}

			// Create the tile grid.
			tileManager.Initialize(room);
			for (int x = 0; x < room.Width; x++) {
				for (int y = 0; y < room.Height; y++) {
					for (int i = 0; i < room.LayerCount; i++) {
						TileDataInstance data = room.TileData[x, y, i];

						if (data != null && data.IsAtLocation(x, y) &&
							data.ModifiedProperties.GetBoolean("enabled", true))
						{
							// Place the tile.
							Tile tile = Tile.CreateTile(data);
							PlaceTile(tile, x, y, i, false);
						}
					}
				}
			}

			// Create the action tiles.
			actionTiles.Capacity = room.ActionData.Count;
			for (int i = 0; i < room.ActionData.Count; i++) {
				ActionTileDataInstance data  = room.ActionData[i];
				if (data.Properties.GetBoolean("enabled", true)) {
					ActionTile actionTile = ActionTile.CreateAction(data);
					actionTiles.Add(actionTile);
				}
			}
			
			// Initialize the tiles.
			tileManager.InitializeTiles();
			
			// Initialize the action tiles.
			for (int i = 0; i < actionTiles.Count; i++) {
				actionTiles[i].Initialize(this);
			}
			
			tileManager.PostInitializeTiles();

			entityCount = entities.Count;
			
			viewControl.Bounds = RoomBounds;
			viewControl.ViewSize = GameSettings.VIEW_SIZE;

			// Fire the RoomStart event.
			GameControl.FireEvent(room, "room_start");

			allMonstersDead = false;
		}

		// Set all entities to destroyed (except the player).
		public void DestroyRoom() {
			for (int i = 0; i < entities.Count; i++) {
				if (entities[i] != Player)
					entities[i].IsDestroyed = true;
			}
			foreach (Tile tile in GetTiles()) {
				tile.OnRemoveFromRoom();
			}
		}
		

		//-----------------------------------------------------------------------------
		// Room Transitions
		//-----------------------------------------------------------------------------

		// Request to transition to an adjacent room.
		public void RequestRoomTransition(int transitionDirection) {
			requestedTransitionDirection = transitionDirection;
		}

		// Cancel any requested room transitions.
		// This should be called in the event 'RoomTransitioning'
		public void CancelRoomTransition() {
			requestedTransitionDirection = -1;
		}
		
		// Transition to another rooom.
		public void TransitionToRoom(Room nextRoom, RoomTransition transition) {
			TransitionToRoom(nextRoom, transition, null, null, null);
		}
		
		// Transition to another room through warp points.
		public void Warp(WarpAction startPoint, ActionTileDataInstance endPoint) {
			TransitionToRoom(endPoint.Room, 
				startPoint.CreateTransition(endPoint),
				startPoint.CreateExitState(),
				null, endPoint);
		}

		// Transition to a room adjacent to the current one.
		public void EnterAdjacentRoom(int direction) {
			Point2I nextLocation = roomLocation + Directions.ToPoint(direction);

			// Transition to the room.
			if (Level.ContainsRoom(nextLocation)) {
				Room nextRoom = Level.GetRoomAt(nextLocation);
				TransitionToRoom(nextRoom, new RoomTransitionPush(direction));
			}
		}
		
		public void TransitionToRoom(Room nextRoom, RoomTransition transition,
			GameState exitState, GameState enterState, ActionTileDataInstance warpTile)
		{
			// Create the new room control.
			RoomControl newControl = new RoomControl();
			newControl.gameManager	= gameManager;
			newControl.room			= nextRoom;
			newControl.roomLocation	= nextRoom.Location;

			// Also set this here to prevent flickering of small keys in HUD
			newControl.dungeon = nextRoom.Dungeon;
			
			//               [Exit]                       [Enter]
			// [RoomOld] -> [RoomOld] -> [Transition] -> [RoomNew] -> [RoomNew]
			
			// Create the sequence of game states for the transition.
			GameState postTransitionState = new GameStateAction(delegate(GameStateAction actionState) {
				gameManager.PopGameState(); // Pop the queue state.
				gameManager.PushGameState(newControl); // Push the new room control state.

				// Find the warp action were warping to and grab its enter-state.
				if (warpTile != null) {
					WarpAction actionTile = newControl.FindActionTile(warpTile) as WarpAction;
					if (actionTile != null)
						enterState = actionTile.CreateEnterState();
				}
				if (enterState != null)
					gameManager.PushGameState(enterState); // Push the enter state.
			});
			GameState preTransitionState = new GameStateAction(delegate(GameStateAction actionState) {
				GameControl.RoomControl = newControl;
				gameManager.PopGameState(); // Pop the queue state.
				gameManager.PopGameState(); // Pop the room control state.
				gameManager.PushGameState(new GameStateQueue(transition, postTransitionState));
				newControl.FindActionTile(warpTile);
			});

			if (warpTile != null) {
				transition.NewRoomSetup += delegate(RoomControl roomControl) {
					// Find the warp action were warping to.
					WarpAction actionTile = newControl.FindActionTile(warpTile) as WarpAction;
					if (actionTile != null)
						actionTile.SetupPlayerInRoom();
				};
			}

			// Create the game state for the transition sequence.
			GameState state = preTransitionState;
			if (exitState != null)
				state = new GameStateQueue(exitState, preTransitionState);

			// Begin the transition.
			transition.OldRoomControl = this;
			transition.NewRoomControl = newControl;
			gameManager.PushGameState(state);
		}


		//-----------------------------------------------------------------------------
		// Events
		//-----------------------------------------------------------------------------

		public void OnPlayerRespawn() {
			if (eventPlayerRespawn != null)
				eventPlayerRespawn.Invoke(Player);
		}


		//-----------------------------------------------------------------------------
		// Overridden methods
		//-----------------------------------------------------------------------------

		public override void OnBegin() {
			requestedTransitionDirection = 0;
		}
		
		public override void OnEnd() {
			
		}

		private void UpdateObjects() {
			requestedTransitionDirection = -1;

			// Update entities.
			entityCount = entities.Count;
			for (int i = 0; i < entities.Count; i++) {
				if (entities[i].IsAlive && entities[i].IsInRoom && i < entityCount) {
					if (GameControl.UpdateRoom)
						entities[i].Update();
					if (GameControl.AnimateRoom)
						entities[i].UpdateGraphics();

					if (requestedTransitionDirection >= 0)
						break;
				}
			}

			// Remove destroyed entities.
			for (int i = 0; i < entities.Count; i++) {
				if (!entities[i].IsAlive || !entities[i].IsInRoom) {
					entities.RemoveAt(i--);
				}
			}

			//if (requestedTransitionDirection >= 0)
			//	return;
			
			// Update tiles.
			tileManager.UpdateTiles();

			// Update the action tiles.
			for (int i = 0; i < actionTiles.Count; i++) {
				actionTiles[i].Update();
			}
			
			// Process Physics.
			if (GameControl.UpdateRoom) {
				roomPhysics.ProcessPhysics();
				Player.CheckRoomTransitions();
			}

			bool nextAllMonstersDead = AllMonstersDead();
			if (nextAllMonstersDead && !allMonstersDead)
				GameControl.FireEvent(room, "all_monsters_dead");
			allMonstersDead = nextAllMonstersDead;
		}

		public override void Update() {
			GameControl.RoomTicks++;

			RoomState roomState		= GameControl.CurrentRoomState;
			GameControl.UpdateRoom	= roomState.UpdateRoom;
			GameControl.AnimateRoom	= roomState.AnimateRoom;

			viewControl.ShakeOffset = Vector2F.Zero;
			
			// Update the current visual effect.
			if (visualEffect != null && !disableVisualEffect)
				visualEffect.Update();
			
			// Update entities, tiles, and action tiles.
			UpdateObjects();

			// Update view to follow player.
			viewControl.PanTo(Player.Center + Player.ViewFocusOffset);
			
			if (requestedTransitionDirection >= 0) {
				// Call the event RoomTransitioning.
				// This event has a chance to cancel the room transition.
				if (eventRoomTransitioning != null) {
					Delegate[] invocationList = eventRoomTransitioning.GetInvocationList();
					for (int i = 0; i < invocationList.Length; i++) {
						invocationList[i].DynamicInvoke(requestedTransitionDirection);
						if (requestedTransitionDirection < 0)
							break;
					}
				}
				if (requestedTransitionDirection >= 0) {
					EnterAdjacentRoom(requestedTransitionDirection);
					requestedTransitionDirection = -1;
				}
			}
			
			// Update HUD and current room state.
			GameControl.HUD.Update();
			GameControl.UpdateRoomState();

			if (GameControl.UpdateRoom) {
				// [Start] Open inventory.
				if (Controls.Start.IsPressed())
					GameControl.OpenMenu(GameControl.MenuWeapons);
				// [Select] Open map screen.
				else if (Controls.Select.IsPressed())
					GameControl.OpenMapScreen();
			}
		}

		public void DrawRoom(Graphics2D g, Vector2F position, RoomDrawing roomDrawing) {
			g.PushTranslation(position);

			if (roomDrawing.HasFlag(RoomDrawing.DrawBelow)) {
				// Draw background (in the color of the HUD).
				Rectangle2I viewRect = new Rectangle2I(0, 0, GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT);
				g.DrawSprite(GameData.SPR_HUD_BACKGROUND, viewRect);
			}

			Vector2F viewTranslation = -GMath.Round(viewControl.ViewPosition);

			g.PushTranslation(viewTranslation);

			if (roomDrawing.HasFlag(RoomDrawing.DrawBelow)) {
				StartVisualEffect(g, position);

				// Draw tiles.
				roomGraphics.Clear();
				tileManager.DrawTiles(roomGraphics);
				roomGraphics.DrawAll(g);

				EndVisualEffect(g, position);

				// DEBUG: Draw debug information over tiles.
				GameDebug.DrawRoomTiles(g, this);

				// Draw entities in reverse order (because newer entities are drawn below older ones).
				roomGraphics.Clear();
				tileManager.DrawEntityTiles(roomGraphics);
				for (int i = entities.Count - 1; i >= 0; i--)
					entities[i].Draw(roomGraphics);
				roomGraphics.SortDepthLayer(DepthLayer.PlayerAndNPCs); // Sort dynamic depth layers.
				roomGraphics.DrawAll(g);

				// Draw action tiles in reverse order.
				for (int i = actionTiles.Count - 1; i >= 0; i--)
					actionTiles[i].Draw(g);
			}
			if (roomDrawing.HasFlag(RoomDrawing.DrawAbove)) {
				// Draw the tile parts that display above the player and all entities
				StartVisualEffect(g, position);

				// Draw above tiles.
				roomGraphics.Clear();
				tileManager.DrawTilesAbove(roomGraphics);
				roomGraphics.DrawAll(g);

				EndVisualEffect(g, position);
			}

			// DEBUG: Draw debug information.
			GameDebug.DrawRoom(g, this);

			g.PopTranslation(2); // position + viewTranslation
		}

		public override void AssignPalettes() {
			GameData.PaletteShader.TilePalette = TilePalette;
			GameData.PaletteShader.EntityPalette = EntityPalette;
		}

		public void AssignLerpPalettes() {
			GameData.PaletteShader.LerpTilePalette = TilePalette;
			GameData.PaletteShader.LerpEntityPalette = EntityPalette;
		}

		public override void Draw(Graphics2D g) {
			DrawRoom(g, new Vector2F(0, GameSettings.HUD_HEIGHT), RoomDrawing.DrawAll);	// Draw the room (offset to make room for the HUD).
			GameControl.HUD.Draw(g);			// Draw the HUD.
			GameControl.DrawRoomState(g);		// Draw the current room state.
		}

		private void StartVisualEffect(Graphics2D g, Vector2F position) {
			// If drawing a visual effect over the room, then
			// begin rendering to the temp render target.
			if (visualEffect != null && !disableVisualEffect) {
				visualEffect.Begin(g, position);
				//g.End();
				//g.PopTranslation();
				//g.SetRenderTarget(GameData.RenderTargetGameTemp);
				//g.Clear(Color.Transparent);
				//g.Begin(GameSettings.DRAW_MODE_DEFAULT);
			}
		}

		private void EndVisualEffect(Graphics2D g, Vector2F position) {
			// Now render the visual effect.
			if (visualEffect != null && !disableVisualEffect) {
				visualEffect.End(g, position);
				//g.End();
				//g.PushTranslation(position);
				//g.SetRenderTarget(GameData.RenderTargetGame);
				//g.Begin(GameSettings.DRAW_MODE_DEFAULT);
				//visualEffect.Render(g, GameData.RenderTargetGameTemp);
			}
		}
		
		
		//-----------------------------------------------------------------------------
		// Scripting API
		//-----------------------------------------------------------------------------

		public void OpenAllDoors(bool instantaneous = false, bool rememberState = false) {
			foreach (TileDoor door in GetTilesOfType<TileDoor>())
				door.Open(instantaneous, rememberState);
		}

		public void CloseAllDoors(bool instantaneous = false, bool rememberState = false) {
			foreach (TileDoor door in GetTilesOfType<TileDoor>())
				door.Close(instantaneous, rememberState);
		}
		
		public void SetDoorStates(ZeldaAPI.DoorState state, bool rememberState = false) {
			foreach (TileDoor door in GetTilesOfType<TileDoor>()) {
				if (state == ZeldaAPI.DoorState.Opened)
					door.Open(true, rememberState);
				else
					door.Close(true, rememberState);
			}
		}

		public IEnumerable<T> GetTilesOfType<T>() where T : class {
			foreach (Tile tile in GetTiles()) {
				if (tile is T)
					yield return tile as T;
			}
		}

		public void SpawnTile(string id, bool staySpawned = false) {
			foreach (TileDataInstance tileData in Room.GetTiles(id))
				SpawnTile(tileData, staySpawned);
			foreach (ActionTileDataInstance actionTileData in Room.ActionData)
				SpawnActionTile(actionTileData, staySpawned);
		}
		
		public ZeldaAPI.Tile GetTileById(string id) {
			foreach (Tile tile in GetTiles()) {
				if (tile.Properties.Get("id", "") == id)
					return (tile as ZeldaAPI.Tile);
			}
			return null;
		}
		
		public IEnumerable<T> GetEntitiesOfType<T>() where T : class {
			foreach (Entity entity in entities) {
				T t = entity as T;
				if (t != null)
					yield return t;
			}
		}
		
		public bool AllMonstersDead() {
			return !GetEntitiesOfType<Monster>().Any(m => m.IsAlive);
		}

		
		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		// The current level that contains the room the player is in.
		public Level Level {
			get { return room.Level; }
		}

		// The current room the player is in.
		public Room Room {
			get { return room; }
		}

		// The player entity (NOTE: this can be null)
		public Player Player {
			get { return GameControl.Player; }
		}

		// Get the list of entities.
		public List<Entity> Entities {
			get { return entities; }
		}

		// Get the number of entities (use this to iterate the entity list).
		public int EntityCount {
			get { return entityCount; }
		}

		// Get the size of the room in pixels.
		public Rectangle2I RoomBounds {
			get { return new Rectangle2I(Point2I.Zero, room.Size * GameSettings.TILE_SIZE); }
		}

		// Get the room's location within the level.
		public Point2I RoomLocation {
			get { return roomLocation; }
		}

		public ViewControl ViewControl {
			get { return viewControl; }
		}

		public TileManager TileManager {
			get { return tileManager; }
		}

		public Dungeon Dungeon {
			get { return dungeon; }
		}

		public RoomPhysics RoomPhysics {
			get { return roomPhysics; }
		}

		// Called after the player respawns.
		public event Action<Player> PlayerRespawn {
			add { eventPlayerRespawn += value; }
			remove { eventPlayerRespawn -= value; }
		}

		// Called as the room is about to transition.
		public event Action<int> RoomTransitioning {
			add { eventRoomTransitioning += value; }
			remove { eventRoomTransitioning -= value; }
		}

		// Are we in a side-scrolling room?
		public bool IsSideScrolling {
			get { return isSideScrolling; }
			set { isSideScrolling = value; }
		}

		// Are we in an underwater room?
		public bool IsUnderwater {
			get { return isUnderwater; }
			set { isUnderwater = value; }
		}

		// Are we in an underwater room?
		public bool DisableVisualEffect {
			get { return disableVisualEffect; }
			set { disableVisualEffect = value; }
		}

		public Zone Zone {
			get { return room.Zone; }
		}

		public Palette TilePalette {
			get { return tilePaletteOverride ?? Zone.Palette; }
		}

		public Palette EntityPalette {
			get { return entityPaletteOverride ?? GameData.PAL_ENTITIES_DEFAULT; }
		}

		public Palette TilePaletteOverride {
			get { return tilePaletteOverride; }
			set {
				tilePaletteOverride = value;
				if (tilePaletteOverride != null && tilePaletteOverride.PaletteType != PaletteTypes.Tile)
					throw new ArgumentException("Palette is not a tile palette!");
			}
		}

		public Palette EntityPaletteOverride {
			get { return entityPaletteOverride; }
			set {
				entityPaletteOverride = value;
				if (entityPaletteOverride != null && entityPaletteOverride.PaletteType != PaletteTypes.Entity)
					throw new ArgumentException("Palette is not an entity palette!");
			}
		}
	}
}
