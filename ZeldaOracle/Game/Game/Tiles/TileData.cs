﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Collision;

namespace ZeldaOracle.Game.Tiles {

	public class TileData {

		private Type			type;
		private TileFlags		flags;
		private Sprite			sprite;
		private Animation		animation;
		private CollisionModel	collisionModel;
		private Point2I			sheetLocation;
		private Point2I			size;

		// TODO: Properties
		// TODO: Tileset?


		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		public TileData() {
			type			= null;
			flags			= TileFlags.Default;
			sprite			= null;
			animation		= null;
			collisionModel	= null;
			sheetLocation	= Point2I.Zero;
			size			= Point2I.One;
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public Type Type {
			get { return type; }
			set { type = value; }
		}

		public TileFlags Flags {
			get { return flags; }
			set { flags = value; }
		}

		public Sprite Sprite {
			get { return sprite; }
			set { sprite = value; }
		}

		public Animation Animation {
			get { return animation; }
			set { animation = value; }
		}

		public CollisionModel CollisionModel {
			get { return collisionModel; }
			set { collisionModel = value; }
		}

		public Point2I SheetLocation {
			get { return sheetLocation; }
			set { sheetLocation = value; }
		}
		
		public Point2I Size {
			get { return size; }
			set { size = value; }
		}
		
	}
}
