using BetterPaint.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public int AddForegroundColor( Color color, int size, PaintMode mode, int x, int y ) {
			if( !BetterPaintTile.Colors.ContainsKey( x ) ) {
				BetterPaintTile.Colors[x] = new Dictionary<int, Color>();
			}
			BetterPaintTile.Colors[x][y] = color;
		}


		public int AddBackgroundColor( Color color, int size, PaintMode mode, int x, int y ) {
			if( !BetterPaintTile.Colors.ContainsKey( x ) ) {
				BetterPaintTile.Colors[x] = new Dictionary<int, Color>();
			}
			BetterPaintTile.Colors[x][y] = color;
		}


		public bool HasColor( Color color, int x, int y ) {

		}
	}
}
