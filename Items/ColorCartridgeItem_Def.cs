using System.Collections.Generic;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public const int Width = 12;
		public const int Height = 16;

		public static Texture2D OverlayTex { get; internal set; }
		public static Texture2D CartridgeTex { get; internal set; }

		static ColorCartridgeItem() {
			ColorCartridgeItem.OverlayTex = null;
			ColorCartridgeItem.CartridgeTex = null;
		}


		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Color Cartridge" );
			this.Tooltip.SetDefault( "Needs a paint blaster to use" + '\n' +
				"Mix cartridges at a paint mixer station" );

			if( ColorCartridgeItem.OverlayTex == null ) {
				ColorCartridgeItem.OverlayTex = this.mod.GetTexture( "Items/ColorCartridgeItem_Color" );
				ColorCartridgeItem.CartridgeTex = this.mod.GetTexture( "Items/ColorCartridgeItem" );
			}

			TmlLoadHelpers.AddModUnloadPromise( () => {
				ColorCartridgeItem.OverlayTex = null;
				ColorCartridgeItem.CartridgeTex = null;
			} );
		}


		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 0, 10, 0 );
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			var mymod = (BetterPaintMod)this.mod;
			var data = this.item.GetGlobalItem<ColorCartridgeItemData>();
			float capacity = mymod.Config.PaintCartridgeCapacity;
			float percent = (capacity - (float)data.Uses) / capacity;

			var tip1 = new TooltipLine( this.mod, "BetterPaint: Color Indicator", "Color value: " + data.MyColor.ToString() ) {
				overrideColor = data.MyColor
			};
			var tip2 = new TooltipLine( this.mod, "BetterPaint: Capacity", "Capacity: " + (int)( percent * 100 ) + "%" ) {
				overrideColor = percent > 0.15f ? Color.White : Color.Red
			};

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}


		////////////////

		public ColorCartridgeItemData GetData() {
			return this.item.GetGlobalItem<ColorCartridgeItemData>();
		}
	}
}
