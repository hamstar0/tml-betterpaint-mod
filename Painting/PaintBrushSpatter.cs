using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintBrushSpatter : PaintBrush {
		public override float Apply( PaintData data, Color color, bool brush_size_small, float pressure, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;
			var rand = new Random( rand_seed );

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;

			int brush_size = brush_size_small ? 2 : 6;
			int density = (int)((float)(brush_size * brush_size) * mymod.Config.BrushSpatterDensity);

			int min_radius = Math.Max( 1, brush_size / 3 );
			int extended_radius_range = brush_size - min_radius;

			for( int i=0; i<density; i++ ) {
				int extended_radius = rand.Next( extended_radius_range );
				int x_off = rand.Next( -(min_radius + extended_radius), (min_radius + extended_radius) );

				extended_radius = rand.Next( extended_radius_range );
				int y_off = rand.Next( -(min_radius + extended_radius), (min_radius + extended_radius) );

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
