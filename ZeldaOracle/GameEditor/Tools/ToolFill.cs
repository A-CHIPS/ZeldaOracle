﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeldaOracle.Common.Content;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Game;
using ZeldaOracle.Game.Worlds;
using ZeldaOracle.Game.Tiles;
using ZeldaOracle.Game.Tiles.ActionTiles;
using ZeldaEditor.Undo;
using ZeldaOracle.Common.Graphics;
using Key = System.Windows.Input.Key;

namespace ZeldaEditor.Tools {
	public class ToolFill : EditorTool {

		private static readonly Cursor FillCursor = LoadCursor("Fill");

		private ActionPlace action;

		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public ToolFill() : base("Fill Tool", Key.F) {
			AddOption("RoomOnly");
		}


		//-----------------------------------------------------------------------------
		// Overridden State Methods
		//-----------------------------------------------------------------------------

		protected override void OnInitialize() {
			MouseCursor = FillCursor;
		}

		protected override void OnBegin() {
			EditorControl.HighlightMouseTile = true;
		}


		//-----------------------------------------------------------------------------
		// Overridden Mouse Methods
		//-----------------------------------------------------------------------------

		protected override void OnMouseDown(MouseEventArgs e) {
			Point2I mousePos	= new Point2I(e.X, e.Y);
			Room room			= LevelDisplay.SampleRoom(mousePos);
			Point2I tileCoord	= LevelDisplay.SampleTileCoordinates(mousePos);
			Point2I target		= LevelDisplay.SampleLevelTileCoordinates(mousePos);
			
			if (!EditorControl.ActionMode && e.Button.IsLeftOrRight()) {
				if ((EditorControl.CurrentLayer == 0 || GetTileAt(target) != null) && GetTileDataSize() == Point2I.One) {
					// Fill tiles.
					TileData fillData = EditorControl.SelectedTileData as TileData;
					if (fillData != null) {
						if (e.Button == MouseButtons.Right)
							fillData = null;
						action = ActionPlace.CreateFillAction(Level, EditorControl.CurrentLayer, fillData);
						Fill(target, fillData);
						EditorControl.PushAction(action, ActionExecution.None);
						action = null;
					}
				}
			}
		}


		//-----------------------------------------------------------------------------
		// Overridden Drawing Methods
		//-----------------------------------------------------------------------------

		public override void DrawTile(Graphics2D g, Room room, Point2I position, Point2I levelCoord, int layer) {
			if (!EditorControl.ActionMode && layer == EditorControl.CurrentLayer) {
				if (levelCoord == LevelDisplay.CursorTileLocation) {
					TileDataInstance tile = CreateDrawTile();
					if (tile != null)
						LevelDisplay.DrawTile(g, room, CreateDrawTile(), position, LevelDisplay.FadeAboveColor);
				}
			}
		}

		
		//-----------------------------------------------------------------------------
		// Internal Methods
		//-----------------------------------------------------------------------------

		private TileDataInstance GetTileAt(Point2I levelTileCoord) {
			return Level.GetRoomAt(levelTileCoord / Level.RoomSize)
				.GetTile(levelTileCoord % Level.RoomSize, EditorControl.CurrentLayer);
		}

		private bool Matches(TileDataInstance targetTile, Point2I targetRoom, Point2I current) {
			if (EditorControl.ToolOptionRoomOnly &&
				targetRoom != (current / Level.RoomSize))
			{
				return false;
			}
			TileDataInstance currentTile = GetTileAt(current);
			return Matches(targetTile, GetTileAt(current));
		}

		private bool Matches(TileDataInstance tile1, TileDataInstance tile2) {
			if (tile1 == null) {
				return (tile2 == null);
			}
			else if (tile2 != null) {
				return (tile1.TileData == tile2.TileData &&
					(!EditorControl.ToolOptionRoomOnly || tile1.Room == tile2.Room));
			}
			return false;
		}

		private void Fill(Point2I target, TileData fillData) {
			Point2I totalSize = Level.Dimensions * Level.RoomSize;
			TileDataInstance targetTile = GetTileAt(target);
			Point2I point;
			Point2I targetRoom = target / Level.RoomSize;

			// Don't fill in the same tiles.
			if (fillData == null && targetTile == null)
				return;
			if (targetTile != null && targetTile.TileData == fillData)
				return;

			Queue<Point2I> nodes = new Queue<Point2I>();
			nodes.Enqueue(target);

			while (nodes.Count > 0) {
				point = nodes.Dequeue();
				if (Matches(targetTile, targetRoom, point)) {
					int width = 1;
					// Travel as far left as possible.
					for (; point.X - 1 >= 0 && Matches(targetTile, targetRoom, point - new Point2I(1, 0)); point.X--, width++) ;
					// Travel as far right as possible.
					for (; point.X + width < totalSize.X && Matches(targetTile, targetRoom, point + new Point2I(width, 0)); width++) ;

					// Fill the row of tiles.
					for (int i = 0; i < width; i++)
						ReplaceTile(point + new Point2I(i, 0), fillData);

					// This continue makes sure that one node is added for every range of pixels in a row.
					bool northContinue = false, southContinue = false;
					for (int i = 0; i < width; i++) {
						if (point.Y - 1 >= 0) {
							if (Matches(targetTile, targetRoom, point + new Point2I(i, -1))) {
								if (!northContinue) {
									nodes.Enqueue(point + new Point2I(i, -1));
									northContinue = true;
								}
							}
							else {
								northContinue = false;
							}
						}
						if (point.Y + 1 < totalSize.Y) {
							if (Matches(targetTile, targetRoom, point + new Point2I(i, 1))) {
								if (!southContinue) {
									nodes.Enqueue(point + new Point2I(i, 1));
									southContinue = true;
								}
							}
							else {
								southContinue = false;
							}
						}
					}
				}
			}
		}

		private void ReplaceTile(Point2I levelTileCoord, TileData tileData) {
			Room room = Level.GetRoomAt(levelTileCoord / Level.RoomSize);
			Point2I tileCoord = levelTileCoord % Level.RoomSize;

			TileDataInstance oldTile = room.GetTile(tileCoord, EditorControl.CurrentLayer);
			action.AddOverwrittenTile(levelTileCoord, oldTile);
			action.AddPlacedTile(levelTileCoord);

			room.CreateTile(tileData, tileCoord, EditorControl.CurrentLayer);
		}

		private TileDataInstance CreateDrawTile() {
			TileData tileData = GetTileData();
			if (tileData != null)
				return new TileDataInstance(tileData);
			return null;
		}
		private TileData GetTileData() {
			return EditorControl.SelectedTileData as TileData;
		}

		private Point2I GetTileDataSize() {
			if (GetTileData() != null)
				return GMath.Max(Point2I.One, GetTileData().Size);
			return Point2I.One;
		}

	}
}
