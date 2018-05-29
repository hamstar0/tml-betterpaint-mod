using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintModeSpatter : PaintMode {
		public override float Apply( PaintData data, Color color, int brush_size, float pressure, int rand_seed, int world_x, int world_y ) {
			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;
			int density = (int)((float)(brush_size * brush_size) * BetterPaintMod.Instance.Config.BrushSpatterDensity);
			int min_size = brush_size / 4;
			int rand_range = brush_size / 2;
			var rand = new Random( rand_seed );

			for( int i=0; i< density; i++ ) {
				int rand_size = rand.Next( rand_range );
				int x_off = rand.Next( -(min_size + rand_size), (min_size + rand_size) );

				rand_size = rand.Next( rand_range );
				int y_off = rand.Next( -(min_size + rand_size), (min_size + rand_size) );

				float pressure_rand = 1f - (float)rand.NextDouble();

				this.PaintAt( data, color, pressure_rand * pressure, (ushort)(tile_x + x_off), (ushort)(tile_y + y_off) );
			}

			return uses;
		}


		public float PaintAt( PaintData data, Color color, float pressure, ushort tile_x, ushort tile_y ) {
			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, pressure );

			data.SetColorAt( lerped_color, tile_x, tile_y );

			return 1f;
		}
	}
}
