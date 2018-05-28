using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeStream : PaintMode {
		public override int Paint( PaintData data, Color color, int size, int x, int y ) {
			int iter_range = size / 2;
			double max_range = (double)size / 2d;
			double uses = 0;

			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					double dist = Math.Sqrt( (double)(( i * i ) + ( j * j )) );

					if( dist > max_range ) {
						continue;
					}

					uses += this.PaintAt( data, color, dist, x + i, y + j );
				}
			}

			return (int)uses;
		}


		public override int PaintAt( PaintData data, Color color, double dist, int x, int y ) {
			data.AddColorAt( color, (ushort)x, (ushort)y );
			return 1;
		}
	}
}
