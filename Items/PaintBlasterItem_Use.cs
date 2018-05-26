using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public override bool CanUseItem( Player player ) {
			return !this.IsModeSelecting && this.GetCurrentPaintItem() != null;
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Item paint_item = this.GetCurrentPaintItem();
			if( paint_item == null ) { return false; }

			var myitem = (ColorCartridgeItem)paint_item.modItem;

			Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, myitem.MyColor, 1f );

			return false;
		}


		public void CheckUse() {
			if( Main.mouseRight ) {
				this.IsModeSelecting = true;
			} else if( this.IsModeSelecting ) {
				this.IsModeSelecting = false;
			}

			if( Main.mouseLeft ) {
				Item paint_item = this.GetCurrentPaintItem();

				if( paint_item != null ) {
					var myitem = (ColorCartridgeItem)paint_item.modItem;

					Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;
					int x = (int)tile_pos.X;
					int y = (int)tile_pos.Y;

					var myworld = this.mod.GetModWorld<BetterPaintWorld>();
					myworld.AddColor( myitem.MyColor, x, y );

					myitem.SetTimesUsed( myitem.TimesUsed + 1 );
				}
			}
		}
	}
}
