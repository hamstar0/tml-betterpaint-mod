using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		/*public override void PostUpdate() {
			if( Main.mouseRight ) {
				if( !this.IsModeSwitching ) {
					this.IsModeSwitching = true;
					
					this.SetMode( ( this.Mode + 1 ) % 3 );
				}
			} else {
				this.IsModeSwitching = false;

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
		}*/


		////////////////

		public void SetMode( int mode ) {
			this.Mode = mode;
		}
	}
}
