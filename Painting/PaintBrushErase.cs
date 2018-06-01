using HamstarHelpers.TileHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintData data, Color _, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
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


		public void EraseAt( PaintData data, float pressure_percent, ushort tile_x, ushort tile_y ) {
			if( TileHelpers.IsAir( Main.tile[tile_x, tile_y] ) ) {
				return;
			}

			int tolerance = (int)(pressure_percent * 255f);

			if( data.HasColor(tile_x, tile_y) ) {
				if( pressure_percent == 1f ) {
					data.RemoveColorAt( tile_x, tile_y );
				} else{
					Color existing_color = data.GetColor( tile_x, tile_y );
					Color lerped_color = Color.Lerp( existing_color, Color.Transparent, pressure_percent );

					if( (lerped_color.R + lerped_color.G + lerped_color.B + lerped_color.A) <= 32 ) {
						data.RemoveColorAt( tile_x, tile_y );
					} else {
						data.SetColorAt( lerped_color, tile_x, tile_y );
					}
				}
			}
		}
	}
}
