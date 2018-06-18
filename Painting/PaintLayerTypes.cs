using HamstarHelpers.TileHelpers;
using Terraria;


namespace BetterPaint.Painting {
	class ForegroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return TileHelpers.IsSolid( tile, true, true ) || Main.tileTable[ tile.type ];
		}
	}


	class BackgroundPaintLayer : PaintLayer {
		public override bool CanPaintAt( Tile tile ) {
			return !TileHelpers.IsAir( tile ) && tile.wall != 0;
		}
	}
}
