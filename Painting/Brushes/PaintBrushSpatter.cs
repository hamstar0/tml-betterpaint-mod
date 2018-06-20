using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushSpatter : PaintBrush {
		public override float Apply( PaintLayer data, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent,
				int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;
			var rand = new Random( rand_seed );

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;

			int diameter = brush_size == PaintBrushSize.Small ? 3 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int spats = (int)( (float)(diameter * diameter) * mymod.Config.BrushSpatterDensity );

			int min_radius = Math.Max( 1, diameter / 3 );
			int extended_radius_range = diameter - min_radius;
			
			for( int i=0; i<spats; i++ ) {
				int extended_radius = rand.Next( extended_radius_range );
				int radius = min_radius + extended_radius;

				int x_off = rand.Next( -radius, radius );

				radius = min_radius + extended_radius;
				int y_off = rand.Next( -radius, radius );

				float rand_percent = pressure_percent * (1f - (float)rand.NextDouble());

				if( rand_percent >= 0.01f ) {
					uses += this.PaintAt( data, color, glow, rand_percent, (ushort)( tile_x + x_off ), (ushort)( tile_y + y_off ) );
				}
			}

			return uses;
		}


		public float PaintAt( PaintLayer data, Color color, byte glow, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !data.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return 0f;
			}

			Color existing_color = data.GetRawColorAt( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, pressure_percent );

			byte existing_glow = data.GetGlowAt( tile_x, tile_y );
			byte lerped_glow = (byte)MathHelper.Lerp( (float)existing_glow, (float)glow, pressure_percent );

			data.SetRawColorAt( lerped_color, tile_x, tile_y );
			data.SetGlowAt( lerped_glow, tile_x, tile_y );

			float diff = PaintBrush.ComputeChangePercent( existing_color, lerped_color, existing_glow, lerped_glow );
			if( diff <= 0.01f ) {
				pressure_percent = 0f;
			}

			return pressure_percent;
			//return PaintBrush.ComputeColorChangePercent( existing_color, lerped_color );
		}
	}
}
