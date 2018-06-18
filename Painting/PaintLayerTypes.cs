using HamstarHelpers.TileHelpers;
using Terraria;


namespace BetterPaint.Painting {
	class ForegroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return TileHelpers.IsSolid( tile, true, true );
		}
	}


	class BackgroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return !TileHelpers.IsAir( tile ) && tile.wall != 0;
		}
	}
}
