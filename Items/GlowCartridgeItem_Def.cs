using System.Collections.Generic;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint.Items {
	partial class GlowCartridgeItem : ModItem {
		public const int Width = 12;
		public const int Height = 16;

		public static Texture2D OverlayTex { get; internal set; }
		public static Texture2D CartridgeTex { get; internal set; }

		static GlowCartridgeItem() {
			GlowCartridgeItem.OverlayTex = null;
			GlowCartridgeItem.CartridgeTex = null;
		}


		////////////////
		
		public float Quantity { get; private set; }


		////////////////

		public override bool CloneNewInstances { get { return true; } }

		public override ModItem Clone() {
			var clone = (GlowCartridgeItem)base.Clone();
			clone.Quantity = this.Quantity;
			return clone;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Color Cartridge" );
			this.Tooltip.SetDefault( "Needs a paint blaster to use" + '\n' +
				"Make cartridges with paint at a paint mixer" + '\n' +
				"Blend cartidges together at a paint mixer" );

			if( GlowCartridgeItem.OverlayTex == null ) {
				GlowCartridgeItem.OverlayTex = this.mod.GetTexture( "Items/GlowCartridgeItem_Color" );
				GlowCartridgeItem.CartridgeTex = this.mod.GetTexture( "Items/GlowCartridgeItem" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					GlowCartridgeItem.OverlayTex = null;
					GlowCartridgeItem.CartridgeTex = null;
				} );
			}
		}


		public override void SetDefaults() {
			var mymod = (BetterPaintMod)this.mod;

			this.Quantity = mymod.Config.PaintCartridgeCapacity;

			this.item.width = GlowCartridgeItem.Width;
			this.item.height = GlowCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 0, 30, 0 );
			this.item.rare = 1;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var mymod = (BetterPaintMod)this.mod;
			float percent = this.Quantity / mymod.Config.PaintCartridgeCapacity;
			
			var tip = new TooltipLine( this.mod, "BetterPaint: Capacity", "Capacity: " + (int)( percent * 100 ) + "%" ) {
				overrideColor = ColorCartridgeItem.GetCapacityColor( percent )
			};
			
			tooltips.Add( tip );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( tag.ContainsKey( "quantity" ) ) {
				this.Quantity = tag.GetFloat( "quantity" );
			}
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "quantity", this.Quantity }
			};
		}

		////////////////
		
		
		public void SetQuantity( float amount ) {
			this.Quantity = amount;
		}


		public void ConsumeQuantity( float amount ) {
			this.Quantity = this.Quantity - amount;
			if( this.Quantity < 0 ) { this.Quantity = 0; }
		}
	}
}
