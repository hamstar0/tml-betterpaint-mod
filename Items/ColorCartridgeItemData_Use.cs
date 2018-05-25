using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItemData : GlobalItem {
		public void CheckInteractions() {
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
