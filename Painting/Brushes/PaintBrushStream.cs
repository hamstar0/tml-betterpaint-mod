using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushStream : PaintBrush {
		public override float Apply( PaintLayer data, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;
			int iter_range = (int)((brush_size == PaintBrushSize.Small ? 1 : 3) * mymod.Config.BrushSizeMultiplier);
			float radius = (brush_size == PaintBrushSize.Small ? 0.5f : 3f) * mymod.Config.BrushSizeMultiplier;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;

			float uses = 0;
			
			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					float dist = (float)Math.Sqrt( (double)(( i * i ) + ( j * j )) );

					if( dist > radius ) {
						continue;
					}
					
					uses += this.PaintAt( data, color, pressure_percent, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return uses;
		}


		public float PaintAt( PaintLayer data, Color color, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !data.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return 0f;
			}

			Color existing_color = data.GetColor( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, color, pressure_percent );

			data.SetColorAt( lerped_color, tile_x, tile_y );

			if( PaintBrush.ComputeColorChangePercent( existing_color, lerped_color ) == 0 ) {
				return 0;
			}

			return pressure_percent;
			//return PaintBrush.ComputeColorChangePercent( existing_color, lerped_color );
		}
	}
}
