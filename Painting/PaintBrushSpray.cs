using HamstarHelpers.TileHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting {
	class PaintBrushSpray : PaintBrush {
		public override float Apply( PaintData data, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
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

					uses += this.PaintAt( data, color, pressure_percent, max_range, dist, x, y );
				}
			}

			return uses;
		}

		
		public float PaintAt( PaintData data, Color color, float pressure_percent, float brush_radius, float dist, ushort tile_x, ushort tile_y ) {
			if( TileHelpers.IsAir( Main.tile[tile_x, tile_y] ) ) {
				return 0f;
			}

			float dist_pressure_percent = pressure_percent * (1f - (dist / brush_radius));
			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, dist_pressure_percent );

			data.SetColorAt( lerped_color, (ushort)tile_x, (ushort)tile_y );

			return dist_pressure_percent;
			//return PaintBrush.ComputeColorChangePercent( existing_color, lerped_color );
		}
	}
}
