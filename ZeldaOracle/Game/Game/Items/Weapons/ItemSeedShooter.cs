﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Input;
using ZeldaOracle.Game.Entities;
using ZeldaOracle.Game.Entities.Projectiles;
using ZeldaOracle.Game.Entities.Effects;
using ZeldaOracle.Game.Entities.Players;
using ZeldaOracle.Game.Items.Ammos;
using ZeldaOracle.Game.Entities.Projectiles.Seeds;
using ZeldaOracle.Common.Graphics.Sprites;

namespace ZeldaOracle.Game.Items.Weapons {

	public class ItemSeedShooter : SeedBasedItem {
		
		private EntityTracker<SeedProjectile> seedTracker;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public ItemSeedShooter() {
			this.id				= "item_seed_shooter";
			this.name			= new string[] { "Seed Shooter" };
			this.description	= new string[] { "Used to bounce seeds around." };
			this.currentAmmo	= 0;
			this.sprite			= new ISprite[] { GameData.SPR_ITEM_ICON_SEED_SHOOTER };
			this.spriteEquipped	= new ISprite[] { GameData.SPR_ITEM_ICON_SEED_SHOOTER_EQUIPPED };
			this.flags			=
				ItemFlags.UsableInMinecart |
				ItemFlags.UsableUnderwater |
				ItemFlags.UsableWhileJumping |
				ItemFlags.UsableWhileInHole;
			this.seedTracker	= new EntityTracker<SeedProjectile>(1);
		}


		//-----------------------------------------------------------------------------
		// Overridden methods
		//-----------------------------------------------------------------------------

		// Called when the items button is pressed (A or B).
		public override bool OnButtonPress() {
			if (HasAmmo()) {
				Player.SeedShooterState.Weapon = this;
				Player.BeginWeaponState(Player.SeedShooterState);
				return true;
			}
			return false;
		}

		// Draws the item inside the inventory.
		public override void DrawSlot(Graphics2D g, Point2I position) {
			DrawSprite(g, position);
			DrawAmmo(g, position);
			g.DrawSprite(ammo[currentAmmo].Sprite, position + new Point2I(8, 0));
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		public EntityTracker<SeedProjectile> SeedTracker {
			get { return seedTracker; }
		}
	}
}
