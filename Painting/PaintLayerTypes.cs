using HamstarHelpers.Helpers.Tiles;
using Terraria;


namespace BetterPaint.Painting {
	class ForegroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return tile.active();	//TileHelpers.IsSolid( tile, true, true ) || Main.tileTable[ tile.type ];
		}
	}


	class BackgroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return !TileHelpers.IsAir( tile ) && tile.wall != 0;
		}
	}
}
