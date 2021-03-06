﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Graphics.Sprites;

namespace ZeldaOracle.Common.Content.ResourceBuilders {

	public class SpriteBuilder {
		private SpriteOld sprite;
		private SpriteOld spriteBegin;
		private SpriteSheet sheet;


		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		public SpriteBuilder() {
			sheet		= null;
			sprite		= null;
			spriteBegin	= null;
		}


		//-----------------------------------------------------------------------------
		// Begin/End
		//-----------------------------------------------------------------------------

		public SpriteBuilder Begin(SpriteOld sprite) {
			this.sprite = sprite;
			this.spriteBegin = sprite;
			return this;
		}
		
		public SpriteBuilder Begin() {
			sprite = new SpriteOld();
			spriteBegin = sprite;
			return this;
		}

		public SpriteOld End() {
			SpriteOld temp = spriteBegin;
			sprite = null;
			spriteBegin = null;
			return temp;
		}


		//-----------------------------------------------------------------------------
		// Building
		//-----------------------------------------------------------------------------

		public SpriteBuilder AddPart(int sheetX, int sheetY, int offsetX = 0, int offsetY = 0) {
			sprite.NextPart = new SpriteOld(sheet, sheetX, sheetY, offsetX, offsetY);
			sprite = sprite.NextPart;
			return this;
		}


		//-----------------------------------------------------------------------------
		// Modifications
		//-----------------------------------------------------------------------------

		public SpriteBuilder SetSheet(SpriteSheet sheet) {
			this.sheet = sheet;
			return this;
		}
		
		public SpriteBuilder SetSize(int width, int height) {
			sprite.SourceRect = new Rectangle2I(sprite.SourceRect.Point, new Point2I(width, height));
			return this;
		}

		public SpriteBuilder SetSize(Point2I size) {
			sprite.SourceRect = new Rectangle2I(sprite.SourceRect.Point, size);
			return this;
		}
		
		public SpriteBuilder Offset(int x, int y) {
			for (SpriteOld spr = spriteBegin; spr != null; spr = spr.NextPart) {
				spr.DrawOffset += new Point2I(x, y);
			}
			return this;
		}

		public SpriteBuilder Offset(Point2I offset) {
			for (SpriteOld spr = spriteBegin; spr != null; spr = spr.NextPart) {
				spr.DrawOffset += offset;
			}
			return this;
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public SpriteSheet SpriteSheet {
			get { return sheet; }
			set { sheet = value; }
		}

		public SpriteOld Sprite {
			get { return sprite; }
			set { sprite = value; }
		}
	}
}
