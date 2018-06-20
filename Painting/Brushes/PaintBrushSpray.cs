using HamstarHelpers.TileHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushSpray : PaintBrush {
		public override float Apply( PaintLayer data, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;
			int diameter = brush_size == PaintBrushSize.Small ? 3 : 8;
			diameter = (int)((float)diameter * mymod.Config.BrushSizeMultiplier);

			int iter_range = diameter / 2;
			float max_range = (float)diameter / 2f;
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

					uses += this.PaintAt( data, color, glow, pressure_percent, max_range, dist, x, y );
				}
			}

			return uses;
		}

		
		public float PaintAt( PaintLayer data, Color color, byte glow, float pressure_percent, float brush_radius, float dist, ushort tile_x, ushort tile_y ) {
			if( !data.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return 0f;
			}
			
			float dist_pressure_percent = pressure_percent * (1f - (dist / brush_radius));
			Color existing_color = data.GetRawColorAt( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, dist_pressure_percent );

			byte existing_glow = data.GetGlowAt( tile_x, tile_y );
			byte lerped_glow = (byte)MathHelper.Lerp( (float)existing_glow, (float)glow, dist_pressure_percent );

			data.SetRawColorAt( lerped_color, tile_x, tile_y );
			data.SetGlowAt( lerped_glow, tile_x, tile_y );

			float diff = PaintBrush.ComputeChangePercent( existing_color, lerped_color, existing_glow, lerped_glow );
			if( diff <= 0.01f ) {
				dist_pressure_percent = 0f;
			}

			return dist_pressure_percent;
		}
	}
}
