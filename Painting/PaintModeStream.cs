using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeStream : PaintMode {
		public override int Paint( PaintData data, Color color, int brush_size, int world_x, int world_y ) {
			int iter_range = brush_size / 2;
			double max_range = (double)brush_size / 2d;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			double uses = 0;

			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					double dist = Math.Sqrt( (double)(( i * i ) + ( j * j )) );

					if( dist > max_range ) {
						continue;
					}

					uses += this.PaintAt( data, color, brush_size, dist, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return (int)uses;
		}


		public override double PaintAt( PaintData data, Color color, int brush_size, double dist, ushort tile_x, ushort tile_y ) {
			data.SetColorAt( color, tile_x, tile_y );
			return 1;
		}
	}
}
