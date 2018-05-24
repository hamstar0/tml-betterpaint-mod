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

		public static Texture2D Overlay { get; internal set; }


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Color Cartridge" );
			this.Tooltip.SetDefault( "Needs a paint blaster to use" + '\n' +
				"Mix cartridges at a paint mixer station" );

			ColorCartridgeItem.Overlay = this.mod.GetTexture( "Items/ColorCartridgeItem_Color" );

			TmlLoadHelpers.AddModUnloadPromise( () => {
				ColorCartridgeItem.Overlay = null;
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

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			var data = this.item.GetGlobalItem<ColorCartridgeItemData>();
			var mymod = (BetterPaintMod)this.mod;
			
			sb.Draw( ColorCartridgeItem.Overlay, pos, frame, data.MyColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_color, float rotation, float scale, int whoAmI ) {
			var data = this.item.GetGlobalItem<ColorCartridgeItemData>();
			var mymod = (BetterPaintMod)this.mod;
			var pos = new Vector2( this.item.position.X - Main.screenPosition.X, this.item.position.Y - Main.screenPosition.Y );

			sb.Draw( ColorCartridgeItem.Overlay, pos, light_color.MultiplyRGBA( data.MyColor ) );
		}


		////////////////

		public ColorCartridgeItemData GetData() {
			return this.item.GetGlobalItem<ColorCartridgeItemData>();
		}
	}
}
