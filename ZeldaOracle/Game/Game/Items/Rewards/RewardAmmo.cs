﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Common.Graphics.Sprites;
using ZeldaOracle.Game.Control;

namespace ZeldaOracle.Game.Items.Rewards {
	public class RewardAmmo : Reward {

		protected string ammoID;
		protected int amount;


		//-----------------------------------------------------------------------------
		// Constructors
		//-----------------------------------------------------------------------------

		public RewardAmmo(string id, string ammoID, int amount, string message, ISprite sprite) {
			InitSprite(sprite);

			this.id				= id;
			this.message		= message;
			this.hasDuration	= true;
			this.holdType		= RewardHoldTypes.Raise;
			this.isCollectibleWithItems	= true;
			this.onlyShowMessageInChest = true;

			this.ammoID			= ammoID;
			this.amount			= amount;
		}


		//-----------------------------------------------------------------------------
		// Overridden Methods
		//-----------------------------------------------------------------------------

		public override void OnCollect(GameControl gameControl) {
			gameControl.Inventory.GetAmmo(ammoID).Amount += amount;
			AudioSystem.PlaySound(GameData.SOUND_GET_ITEM);
		}
		
		public override bool IsAvailable(GameControl gameControl) {
			return gameControl.Inventory.IsAmmoAvailable(ammoID);
		}

		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

	}
}
