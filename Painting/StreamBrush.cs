using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class StreamBrush {
		public static void Paint( PaintData data, Color color, int size, int x, int y ) {
			int iter_range = (size / 2) + 1;
			double max_range = (double)size / 2d;

			for( int i = -iter_range; i < iter_range; i++ ) {
				for( int j = -iter_range; j < iter_range; j++ ) {
					double dist = Math.Sqrt( ( i * i ) + ( j * j ) );

					if( dist > max_range ) {
						continue;
					}

					StreamBrush.PaintAt( data, color, dist, i, j );
				}
			}
		}


		public static void PaintAt( PaintData data, Color color, double range, int x, int y ) {
			data.ColorAt( color, (ushort)x, (ushort)y );
		}
	}
}
