using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public void AddColor( Color color, int x, int y ) {
			if( !BetterPaintTile.Colors.ContainsKey( x ) ) {
				BetterPaintTile.Colors[x] = new Dictionary<int, Color>();
			}
			BetterPaintTile.Colors[x][y] = Color.Red;
		}
	}
}
