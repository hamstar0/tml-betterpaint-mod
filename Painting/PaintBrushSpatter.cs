using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	class PaintBrushSpatter : PaintBrush {
		public override float Apply( PaintData data, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;
			var rand = new Random( rand_seed );

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;

			int diameter = brush_size == PaintBrushSize.Small ? 2 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int density = (int)((float)(diameter * diameter) * mymod.Config.BrushSpatterDensity);

			int min_radius = Math.Max( 1, diameter / 3 );
			int extended_radius_range = diameter - min_radius;

			for( int i=0; i<density; i++ ) {
				int extended_radius = rand.Next( extended_radius_range );
				int x_off = rand.Next( -(min_radius + extended_radius), (min_radius + extended_radius) );

				extended_radius = rand.Next( extended_radius_range );
				int y_off = rand.Next( -(min_radius + extended_radius), (min_radius + extended_radius) );

				float pressure_rand = 1f - (float)rand.NextDouble();

				this.PaintAt( data, color, pressure_rand * pressure_percent, (ushort)(tile_x + x_off), (ushort)(tile_y + y_off) );
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
