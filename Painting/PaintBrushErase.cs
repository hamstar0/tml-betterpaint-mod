using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintData data, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;

			int diameter = brush_size == PaintBrushSize.Small ? 1 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int iter_range = Math.Max( 1, diameter / 2 );
			float radius = (float)diameter / 2f;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			color.A = color.A > 192 ? (byte)192 : (byte)0;
			
			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					float dist = (float)Math.Sqrt( (double)((i * i) + (j * j)) );

					if( dist > radius ) {
						continue;
					}

					this.EraseAt( data, color, pressure_percent, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return 0;
		}


		public void EraseAt( PaintData data, Color color, float pressure, ushort tile_x, ushort tile_y ) {
			int tolerance = (int)(pressure * 255f);

			if( data.HasColor(tile_x, tile_y) ) {
				Color existing_color = data.GetColor( tile_x, tile_y );

				if( Math.Abs((int)existing_color.R - (int)color.R) > tolerance ) {
					return;
				}
				if( Math.Abs((int)existing_color.G - (int)color.G) > tolerance ) {
					return;
				}
				if( Math.Abs((int)existing_color.B - (int)color.B) > tolerance ) {
					return;
				}

				data.RemoveColorAt( tile_x, tile_y );
			}
		}
	}
}
