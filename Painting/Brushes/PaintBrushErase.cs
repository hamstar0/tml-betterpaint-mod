using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintLayer layer, Color _, byte __, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;

			int diameter = brush_size == PaintBrushSize.Small ? 1 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int iter_range = Math.Max( 1, diameter / 2 );
			float radius = (float)diameter / 2f;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;
			
			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					float dist = (float)Math.Sqrt( (double)((i * i) + (j * j)) );

					if( dist > radius ) {
						continue;
					}

					this.EraseAt( layer, pressure_percent, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return 0;
		}


		public void EraseAt( PaintLayer layer, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !layer.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return;
			}
			if( !layer.HasColorAt( tile_x, tile_y ) ) {
				return;
			}

			int tolerance = (int)( pressure_percent * 255f );

			Color? old_color;
			byte old_glow;

			Color blended_color = PaintBrush.GetErasedColor( layer, pressure_percent, tile_x, tile_y, out old_color );
			byte blended_glow = PaintBrush.GetBlendedGlow( layer, 0, pressure_percent, tile_x, tile_y, out old_glow );

			float diff = PaintBrush.ComputeChangePercent( old_color, blended_color, old_glow, blended_glow );

			if( diff <= 0.01f ) {
				layer.RemoveRawColorAt( tile_x, tile_y );
				layer.RemoveGlowAt( tile_x, tile_y );
			} else {
				layer.SetRawColorAt( blended_color, tile_x, tile_y );
				layer.SetGlowAt( blended_glow, tile_x, tile_y );
			}
		}
	}
}
