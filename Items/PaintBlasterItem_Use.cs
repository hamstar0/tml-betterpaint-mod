using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Item mypaint_item = this.GetCurrentPaintItem();
			if( mypaint_item == null ) { return false; }

			var mypaint = (ColorCartridgeItem)mypaint_item.modItem;

			Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, mypaint.MyColor, 1f );

			return false;
		}


		public void CheckUse() {
			if( Main.mouseRight ) {
				this.IsModeSelecting = true;
			} else if( this.IsModeSelecting ) {
				this.IsModeSelecting = false;
			}

			if( Main.mouseLeft ) {
				Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;
				int x = (int)tile_pos.X;
				int y = (int)tile_pos.Y;

				if( !BetterPaintTile.Colors.ContainsKey( x ) ) {
					BetterPaintTile.Colors[x] = new Dictionary<int, Color>();
				}
				BetterPaintTile.Colors[x][y] = Color.Red;
			}
		}
	}
}
