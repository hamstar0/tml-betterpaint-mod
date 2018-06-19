using HamstarHelpers.TileHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintLayer data, Color _, bool __, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
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

					this.EraseAt( data, pressure_percent, is_lit, (ushort)(tile_x + i), (ushort)(tile_y + j) );
				}
			}

			return 0;
		}


		public void EraseAt( PaintLayer data, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( !data.CanPaintAt( Main.tile[tile_x, tile_y] ) ) {
				return;
			}

			if( data.HasColor(tile_x, tile_y) ) {
				int tolerance = (int)( pressure_percent * 255f );

				if( pressure_percent == 1f ) {
					data.RemoveColorAt( tile_x, tile_y );
				} else{
					Color existing_color = data.GetColor( tile_x, tile_y );
					Color lerped_color = Color.Lerp( existing_color, Color.White, pressure_percent );

					if( XnaColorHelpers.AvgRGBA(lerped_color) >= 240 ) {
						data.RemoveColorAt( tile_x, tile_y );
					} else {
						data.SetColorAt( lerped_color, tile_x, tile_y );
					}
				}
			}
		}
	}
}
