﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Game.Main;
using ZeldaOracle.Game.Tiles;
using ZeldaOracle.Game.Entities;
using ZeldaOracle.Game.Entities.Effects;
using ZeldaOracle.Game.Entities.Monsters;
using ZeldaOracle.Game.Entities.Players;
using ZeldaOracle.Game.Entities.Projectiles;
using ZeldaOracle.Game.Control;
using ZeldaOracle.Game.Items;
using ZeldaOracle.Game.Items.Weapons;

namespace ZeldaOracle.Game.Entities.Players.States.SwingStates {

	public class PlayerSwingCaneState : PlayerSwingState {

		
		private const int SPAWN_SOMARIA_BLOCK_DELAY = 15; // Occurs as the player pulls back his lunge.


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public PlayerSwingCaneState() {
			InitStandardSwing(GameData.ANIM_CANE_SWING,
							  GameData.ANIM_CANE_MINECART_SWING);
			AddTimedAction(SPAWN_SOMARIA_BLOCK_DELAY, SpawnSomariaBlock);
		}


		//-----------------------------------------------------------------------------
		// Somaria Block Methods
		//-----------------------------------------------------------------------------

		private void SpawnSomariaBlock() {
			// Cane of Somaria doesn't work while the player is in a minecart.
			if (player.IsInMinecart)
				return;

			ItemCane itemCane = (ItemCane) Weapon;

			// Break any existing somaria block.
			if (itemCane.SomariaBlockTile != null) {
				itemCane.SomariaBlockTile.CancelBreakSound = true;
				itemCane.SomariaBlockTile.Break(false);
				itemCane.SomariaBlockTile = null;
			}
			
			Vector2F pos = player.Center + (Directions.ToVector(SwingDirection) * 19);
			Point2I tileLocation = player.RoomControl.GetTileLocation(pos);
			EffectCreateSomariaBlock effect = new EffectCreateSomariaBlock(tileLocation, player.ZPosition, itemCane);
			player.RoomControl.SpawnEntity(effect);
			
			AudioSystem.PlaySound(GameData.SOUND_MYSTERY_SEED);
		}


		//-----------------------------------------------------------------------------
		// Overridden Methods
		//-----------------------------------------------------------------------------

		public override void OnBegin(PlayerState previousState) {
			if (player.IsInMinecart) {
				weaponSwingAnimation			= GameData.ANIM_CANE_MINECART_SWING;
				playerSwingAnimation			= GameData.ANIM_PLAYER_SWING_NOLUNGE;
				playerSwingAnimationInMinecart	= GameData.ANIM_PLAYER_MINECART_SWING_NOLUNGE;
			}
			else {
				weaponSwingAnimation			= GameData.ANIM_CANE_SWING;
				playerSwingAnimation			= GameData.ANIM_PLAYER_SWING;
				playerSwingAnimationInMinecart	= GameData.ANIM_PLAYER_MINECART_SWING;
			}
			base.OnBegin(previousState);
		}

		public override void OnSwingBegin() {
			AudioSystem.PlayRandomSound(
				GameData.SOUND_SWORD_SLASH_1,
				GameData.SOUND_SWORD_SLASH_2,
				GameData.SOUND_SWORD_SLASH_3);
		}
	}
}
