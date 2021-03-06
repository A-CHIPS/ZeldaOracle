﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaOracle.Common.Geometry;
using ZeldaOracle.Common.Graphics;
using ZeldaOracle.Game.Entities;
using ZeldaOracle.Game.Entities.Players;
using ZeldaOracle.Game.Entities.Players.States;
using ZeldaOracle.Game.Entities.Players.States.SwingStates;
using ZeldaOracle.Game.Control;
using ZeldaOracle.Common.Input;

namespace ZeldaOracle.Game.Items {

	public abstract class ItemWeapon : ItemEquipment {

		// The flags describing the item.
		protected ItemFlags	flags;
		private int			equipSlot;
		protected int		currentAmmo;
		protected Ammo[]	ammo;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		public ItemWeapon() {
			this.flags			= ItemFlags.None;
			this.equipSlot		= 0;
			this.currentAmmo	= -1;
			this.ammo			= null;
		}
		

		//-----------------------------------------------------------------------------
		// Equipping
		//-----------------------------------------------------------------------------

		public void Equip(int slot) {
			equipSlot = slot;
			base.Equip();
		}
		

		//-----------------------------------------------------------------------------
		// Ammo
		//-----------------------------------------------------------------------------
		
		// Return true if the given amount of ammo exists for the current type.
		public bool HasAmmo(int amount = 1) {
			return (ammo[currentAmmo].Amount >= amount);
		}

		// Use up the given amount of ammo of the current type.
		public void UseAmmo(int amount = 1) {
			ammo[currentAmmo].Amount -= amount;
		}

		public Ammo GetAmmoAt(int index) {
			return ammo[index];
		}


		//-----------------------------------------------------------------------------
		// Buttons
		//-----------------------------------------------------------------------------
		
		public bool IsButtonDown() {
			if (IsTwoHanded)
				return inventory.GetSlotButton(0).IsDown() ||
					inventory.GetSlotButton(1).IsDown();
			return inventory.GetSlotButton(equipSlot).IsDown();
		}
		
		public bool IsButtonPressed() {
			if (IsTwoHanded)
				return inventory.GetSlotButton(0).IsPressed() ||
					inventory.GetSlotButton(1).IsPressed();
			return inventory.GetSlotButton(equipSlot).IsPressed();
		}


		//-----------------------------------------------------------------------------
		// Virtual
		//-----------------------------------------------------------------------------
		
		public virtual bool IsUsable() {
			if (Player.StateParameters.ProhibitWeaponUse)
				return false;
			if (Player.IsInMinecart && !flags.HasFlag(ItemFlags.UsableInMinecart))
				return false;
			else if (Player.IsInAir && !flags.HasFlag(ItemFlags.UsableWhileJumping))
				return false;
			else if (Player.Physics.IsInHole && !flags.HasFlag(ItemFlags.UsableWhileInHole))
				return false;
			else if ((Player.WeaponState is PlayerHoldSwordState ||
				Player.WeaponState is PlayerSwingState ||
				Player.WeaponState is PlayerSeedShooterState ||
				Player.WeaponState is PlayerSpinSwordState) &&
				!flags.HasFlag(ItemFlags.UsableWithSword))
			{
				return false;
			}
			else if (Player.IsSwimmingUnderwater)
				return flags.HasFlag(ItemFlags.UsableUnderwater);
			else
				return true;
		}
		
		public virtual void OnWeaponEquip() { }

		public virtual void OnWeaponUnequip() { }

		// Immediately interrupt this item (ex: if the player falls in a hole).
		public virtual void Interrupt() {}

		// Called when the items button is down (A or B).
		public virtual void OnButtonDown() {}
		
		// Called when the items button is pressed (A or B).
		public virtual bool OnButtonPress() { return false; }
		
		// Update the item.
		public virtual void Update() { }

		// Draws under link's sprite.
		public virtual void DrawUnder() { }

		// Draws over link's sprite.
		public virtual void DrawOver() { }
		

		//-----------------------------------------------------------------------------
		// Accessors
		//-----------------------------------------------------------------------------

		// Gets if the item has the specified flags.
		public bool HasFlag(ItemFlags flags) {
			return this.flags.HasFlag(flags);
		}
		

		//-----------------------------------------------------------------------------
		// Overridden methods
		//-----------------------------------------------------------------------------

		protected virtual void DrawAmmo(Graphics2D g, Point2I position) {
			g.DrawString(GameData.FONT_SMALL, ammo[currentAmmo].Amount.ToString("00"), position + new Point2I(8, 8), EntityColors.Black);
		}
		
		// Draws the item inside the inventory.
		public override void DrawSlot(Graphics2D g, Point2I position) {
			DrawSprite(g, position);
			if (maxLevel > Item.Level1)
				DrawLevel(g, position);
		}


		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------
		
		// Gets or sets the item flags.
		public ItemFlags Flags {
			get { return flags; }
			set { flags = value; }
		}

		public int CurrentEquipSlot {
			get { return equipSlot; }
			set { equipSlot = value; }
		}

		public InputControl CurrentControl {
			get { return inventory.GetSlotButton(equipSlot); }
		}

		public bool IsTwoHanded {
			get { return flags.HasFlag(ItemFlags.TwoHanded); }
		}

		public int NumAmmos {
			get {
				if (ammo != null)
					return ammo.Length;
				return 0;
			}
		}

		public int CurrentAmmo {
			get { return currentAmmo; }
			set { currentAmmo = GMath.Clamp(value, 0, ammo.Length - 1); }
		}
	}
}
