using BetterPaint.Helpers.XnaHelpers;
using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public bool HasColorAt( ushort tile_x, ushort tile_y ) {
			return this.Colors.ContainsKey( tile_x ) && this.Colors[tile_x].ContainsKey( tile_y );
		}


		public Color GetRawColorAt( ushort tile_x, ushort tile_y ) {
			if( this.Colors.ContainsKey( tile_x ) ) {
				if( this.Colors[tile_x].ContainsKey( tile_y ) ) {
					return this.Colors[tile_x][tile_y];
				}
			}
			return Color.Transparent;
		}


		////////////////

		public Color ComputeTileColor( ushort tile_x, ushort tile_y ) {
			Color end_color;
			Color black_paint_data = this.GetRawColorAt( tile_x, tile_y );
			Color white_paint_data = XnaColorHelpers.GetWhiteBase( black_paint_data );

			Color full_white_color = PaintHelpers.FullColor( white_paint_data );
			Color env_color = Lighting.GetColor( tile_x, tile_y, full_white_color );

			float lit_scale = (float)black_paint_data.A / 255f;

			if( lit_scale > 0 ) {
				float r_scale = (float)black_paint_data.R / 255f;
				float g_scale = (float)black_paint_data.G / 255f;
				float b_scale = (float)black_paint_data.B / 255f;

				int r = env_color.R + (int)( ( 255 - env_color.R ) * r_scale );
				int g = env_color.G + (int)( ( 255 - env_color.G ) * g_scale );
				int b = env_color.B + (int)( ( 255 - env_color.B ) * b_scale );

				end_color = new Color( r, g, b );
			} else {
				end_color = env_color;
			}

			return end_color;
		}


		////////////////

		public abstract bool CanPaintAt( Tile tile );


		public void SetColorAt( Color color, ushort tile_x, ushort tile_y ) {
			if( !this.Colors.ContainsKey(tile_x) ) {
				this.Colors[tile_x] = new Dictionary<ushort, Color>();
			}
			this.Colors[tile_x][tile_y] = color;
		}


		public void RemoveColorAt( ushort tile_x, ushort tile_y ) {
			if( this.Colors.ContainsKey( tile_x ) ) {
				this.Colors[tile_x].Remove( tile_y );
			}
		}
	}
}
