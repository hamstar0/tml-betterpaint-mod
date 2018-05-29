using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeStream : PaintMode {
		public override float Apply( PaintData data, Color color, int brush_size, float pressure, int rand_seed, int world_x, int world_y ) {
			int iter_range = brush_size / 2;
			float max_range = (float)brush_size / 2f;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;

			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					float dist = (float)Math.Sqrt( (double)(( i * i ) + ( j * j )) );

					if( dist > max_range ) {
						continue;
					}

					uses += this.PaintAt( data, color, pressure, ( ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return uses;
		}


		public float PaintAt( PaintData data, Color color, float pressure, ushort tile_x, ushort tile_y ) {
			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, pressure );

			data.SetColorAt( color, tile_x, tile_y );
			return 1f;
		}
	}
}
