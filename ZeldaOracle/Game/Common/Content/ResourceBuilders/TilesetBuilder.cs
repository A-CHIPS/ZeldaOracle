﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Graphics.Sprites;
using ZeldaOracle.Common.Scripting;
using ZeldaOracle.Game.Tiles;

namespace ZeldaOracle.Common.Content.ResourceBuilders {

	public class TilesetBuilder {
		
		private Tileset		tileset;
		private TileData	tileData;


		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		public TilesetBuilder() {
			tileset		= null;
			tileData	= null;
		}


		//-----------------------------------------------------------------------------
		// Begin/End
		//-----------------------------------------------------------------------------

		public TilesetBuilder Begin(int tileX, int tileY) {
			tileData = tileset.TileData[tileX, tileY];
			if (tileData == null)
				tileData = new TileData();

			tileData.SheetLocation = new Point2I(tileX, tileY);
			tileData.Tileset = tileset;
			tileData.Sprite = tileset.SpriteSheet.GetSprite(tileX, tileY);

			tileset.TileData[tileX, tileY] = tileData;
			return this;
		}
		
		public TilesetBuilder Begin(TileData tileData) {
			this.tileData = tileData;
			this.tileData.Tileset = tileset;
			return this;
		}


		//-----------------------------------------------------------------------------
		// Building
		//-----------------------------------------------------------------------------

		public TilesetBuilder SetModel(CollisionModel model) {
			tileData.CollisionModel	= model;
			return this;
		}
		
		public TilesetBuilder SetSolidModel(CollisionModel model) {
			tileData.SolidType		= TileSolidType.Solid;
			tileData.CollisionModel	= model;
			return this;
		}
		
		public TilesetBuilder CreateLedge(CollisionModel model, int ledgeDirection) {
			tileData.CollisionModel	= model;
			tileData.LedgeDirection	= ledgeDirection;
			tileData.SolidType		= TileSolidType.Ledge;
			return this;
		}
		
		public TilesetBuilder SetSprite(ISprite sprite) {
			tileData.Sprite = sprite;
			return this;
		}
		
		public TilesetBuilder SetSprites(params ISprite[] spriteAnims) {
			ISprite[] spriteAnimations = new ISprite[spriteAnims.Length];
			for (int i = 0; i < spriteAnims.Length; i++)
				spriteAnimations[i] = spriteAnims[i];
			return this;
		}

		public TilesetBuilder AddFlags(TileFlags flags) {
			tileData.Flags |= flags;
			return this;
		}
		
		public TilesetBuilder SetFlags(TileFlags flags) {
			tileData.Flags = flags;
			return this;
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------
		
		public Tileset Tileset {
			get { return tileset; }
			set { tileset = value; }
		}
		
		public TileData TileData {
			get { return tileData; }
			set { tileData = value; }
		}
	}
}
