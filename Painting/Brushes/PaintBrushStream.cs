using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushStream : PaintBrush {
		public override float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
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
					
					uses += this.PaintAt( layer, color, glow, pressure_percent, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return uses;
		}


		public float PaintAt( PaintLayer layer, Color color, byte glow, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !layer.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return 0f;
			}
			
			Color? old_color;
			byte old_glow;

			Color blended_color = PaintBrush.GetBlendedColor( layer, color, pressure_percent, tile_x, tile_y, out old_color );
			byte blended_glow = PaintBrush.GetBlendedGlow( layer, glow, pressure_percent, tile_x, tile_y, out old_glow );

			layer.SetRawColorAt( blended_color, tile_x, tile_y );
			layer.SetGlowAt( blended_glow, tile_x, tile_y );

			float diff = PaintBrush.ComputeChangePercent( old_color, blended_color, old_glow, blended_glow );
			if( diff <= 0.01f ) {
				pressure_percent = 0f;
			}

			return pressure_percent;
			//return PaintBrush.ComputeColorChangePercent( existing_color, lerped_color );
		}
	}
}
