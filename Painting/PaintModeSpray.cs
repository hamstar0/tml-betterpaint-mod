using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeSpray : PaintMode {
		public override int Paint( PaintData data, Color color, int brush_size, int world_x, int world_y ) {
			brush_size += 2;

			int iter_range = brush_size / 2;
			double max_range = (double)brush_size / 2d;
			double uses = 0;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;
			int tile_x_offset = world_x % 16;
			int tile_y_offset = world_y % 16;

			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					int i_off = i + tile_x_offset;
					int j_off = j + tile_y_offset;

					double dist = Math.Sqrt( (double)( ( i_off * i_off ) + ( j_off * j_off ) ) );

					if( dist > max_range ) {
						continue;
					}

					uses += this.PaintAt( data, color, brush_size, dist, (ushort)( tile_x + i ), (ushort)( tile_y + j ) );
				}
			}

			return (int)uses;
		}


		public override double PaintAt( PaintData data, Color color, int brush_size, double dist, ushort tile_x, ushort tile_y ) {
			float amount = (float)dist / (float)brush_size;
			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( color, existing_color, amount );

			data.SetColorAt( lerped_color, (ushort)tile_x, (ushort)tile_y );

			return (double)amount;
		}
	}
}
