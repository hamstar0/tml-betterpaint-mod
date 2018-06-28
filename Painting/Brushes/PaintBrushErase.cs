using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintLayer data, Color _, byte __, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = BetterPaintMod.Instance;

			int diameter = brush_size == PaintBrushSize.Small ? 1 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int iter_range = Math.Max( 1, diameter / 2 );
			float radius = (float)diameter / 2f;

			int tile_x = world_x / 16;
			int tile_y = world_y / 16;
			
			for( int i = -iter_range; i <= iter_range; i++ ) {
				for( int j = -iter_range; j <= iter_range; j++ ) {
					float dist = (float)Math.Sqrt( (double)((i * i) + (j * j)) );

					if( dist > radius ) {
						continue;
					}

					this.EraseAt( data, pressure_percent, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return 0;
		}


		public void EraseAt( PaintLayer data, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !data.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return;
			}
			if( !data.HasColorAt( tile_x, tile_y ) ) {
				return;
			}

			int tolerance = (int)( pressure_percent * 255f );
				
			Color existing_color = data.GetRawColorAt( tile_x, tile_y );
			Color lerped_color = Color.Lerp( existing_color, Color.Transparent, pressure_percent );

			byte existing_glow = data.GetGlowAt( tile_x, tile_y );
			byte lerped_glow = (byte)MathHelper.Lerp( (float)existing_glow, 0f, pressure_percent );

			float diff = PaintBrush.ComputeChangePercent( existing_color, lerped_color, existing_glow, lerped_glow );

			if( diff <= 0.01f ) {
				data.RemoveRawColorAt( tile_x, tile_y );
				data.RemoveGlowAt( tile_x, tile_y );
			} else {
				data.SetRawColorAt( lerped_color, tile_x, tile_y );
				data.SetGlowAt( lerped_glow, tile_x, tile_y );
			}
		}
	}
}
