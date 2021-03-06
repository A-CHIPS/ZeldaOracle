﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Game.Worlds;
using ZeldaOracle.Game.Tiles;

namespace ZeldaEditor.Tools {
	public class ToolSquare : EditorTool {
		private Point2I dragBeginTileCoord;
		private bool isCreatingSelectionBox;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public ToolSquare() {
			name = "Square Tool";
		}

		
		//-----------------------------------------------------------------------------
		// Selection
		//-----------------------------------------------------------------------------

		public override void Cut() {
			//Deselect();
		}
		
		public override void Copy() {

		}
		
		public override void Paste() {
			//Deselect();
		}
		
		public override void Delete() {
			//Deselect();
		}

		public override void SelectAll() {
			isCreatingSelectionBox = false;
			LevelDisplayControl.SetSelectionBox(Point2I.Zero,
				EditorControl.Level.RoomSize * EditorControl.Level.Dimensions);
		}

		public override void Deselect() {
			isCreatingSelectionBox = false;
			LevelDisplayControl.ClearSelectionBox();
		}

		private void ActivateTile(MouseButtons mouseButton, Point2I levelTileCoord) {
			Room room = editorControl.Level.GetRoomAt(levelTileCoord / editorControl.Level.RoomSize);
			Point2I tileCoord = levelTileCoord % editorControl.Level.RoomSize;

			if (mouseButton == MouseButtons.Left) {
				TileData tileData = editorControl.SelectedTilesetTileData as TileData;
				if (tileData != null) {
					room.CreateTile(
						tileData,
						tileCoord.X, tileCoord.Y, editorControl.CurrentLayer
					);
				}

			}
			else if (mouseButton == MouseButtons.Right) {
				/*if (editorControl.CurrentLayer == 0) {
					room.CreateTile(
						editorControl.Tileset.DefaultTileData,
						tileCoord.X, tileCoord.Y, editorControl.CurrentLayer
					);
				}
				else {*/
				room.RemoveTile(tileCoord.X, tileCoord.Y, editorControl.CurrentLayer);
				//}
			}
		}

		
		//-----------------------------------------------------------------------------
		// Overridden Methods
		//-----------------------------------------------------------------------------

		public override void Initialize() {

		}

		public override void OnBegin() {
			isCreatingSelectionBox = false;
		}

		public override void OnEnd() {
			isCreatingSelectionBox = false;
		}

		public override void OnMouseDragBegin(MouseEventArgs e) {
			// Draw a new selecion box.
			if (e.Button == MouseButtons.Left) {
				isCreatingSelectionBox = true;
				Point2I mousePos	= new Point2I(e.X, e.Y);
				dragBeginTileCoord	= LevelDisplayControl.SampleLevelTileCoordinates(mousePos);
				LevelDisplayControl.SetSelectionBox(dragBeginTileCoord, Point2I.One);
			}
		}

		public override void OnMouseDragEnd(MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && isCreatingSelectionBox) {
				isCreatingSelectionBox = false;
				Point2I mousePos  = new Point2I(e.X, e.Y);
				Point2I tileCoord = LevelDisplayControl.SampleLevelTileCoordinates(mousePos);
				Point2I minCoord  = GMath.Min(dragBeginTileCoord, tileCoord);
				Point2I maxCoord  = GMath.Max(dragBeginTileCoord, tileCoord);
				Point2I totalSize	= editorControl.Level.Dimensions * editorControl.Level.RoomSize;
				for (int x = minCoord.X; x <= maxCoord.X && x < totalSize.X; x++) {
					for (int y = minCoord.Y; y <= maxCoord.Y && y < totalSize.Y; y++) {
						ActivateTile(e.Button, new Point2I(x, y));
					}
				}
				LevelDisplayControl.ClearSelectionBox();
			}
		}

		public override void OnMouseDragMove(MouseEventArgs e) {
			// Update selection box.
			if (e.Button == MouseButtons.Left && isCreatingSelectionBox) {
				Point2I mousePos  = new Point2I(e.X, e.Y);
				Point2I tileCoord = LevelDisplayControl.SampleLevelTileCoordinates(mousePos);
				Point2I minCoord  = GMath.Min(dragBeginTileCoord, tileCoord);
				Point2I maxCoord  = GMath.Max(dragBeginTileCoord, tileCoord);
				LevelDisplayControl.SetSelectionBox(minCoord, maxCoord - minCoord + Point2I.One);
			}
		}
	}
}
