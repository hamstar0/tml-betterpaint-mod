using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeSpray : PaintMode {
		public override float Paint( PaintData data, Color color, int brush_size, int world_x, int world_y ) {
			brush_size += 2;

			int iter_range = brush_size / 2;
			float max_range = (float)brush_size / 2f;
			float uses = 0;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;
			double tile_x_offset = (double)( world_x % 16 ) / 16f;
			double tile_y_offset = (double)( world_y % 16 ) / 16f;

			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					double i_off = i + tile_x_offset;
					double j_off = j + tile_y_offset;

					float dist = (float)Math.Sqrt( (i_off * i_off) + (j_off * j_off) );

					if( dist > max_range ) {
						continue;
					}

					ushort x = (ushort)( tile_x + i );
					ushort y = (ushort)( tile_y + j );

					uses += this.PaintAt( data, color, max_range, dist, x, y );
				}
			}

			return uses;
		}

		
		public float PaintAt( PaintData data, Color color, float brush_radius, float dist, ushort tile_x, ushort tile_y ) {
			float percent = dist / brush_radius;
			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( color, existing_color, percent );

			data.SetColorAt( lerped_color, (ushort)tile_x, (ushort)tile_y );

			return percent;
		}
	}
}
