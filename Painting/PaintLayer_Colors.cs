using HamstarHelpers.DebugHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public abstract bool CanPaintAt( Tile tile );

		////////////////


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


		public byte GetGlowAt( ushort tile_x, ushort tile_y ) {
			if( this.Glows.ContainsKey( tile_x ) ) {
				if( this.Glows[tile_x].ContainsKey( tile_y ) ) {
					return this.Glows[tile_x][tile_y];
				}
			}
			return 0;
		}


		////////////////
		
		public void SetRawColorAt( Color color, ushort tile_x, ushort tile_y ) {
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


		////////////////

		public void SetGlowAt( byte glow, ushort tile_x, ushort tile_y ) {
			if( !this.Glows.ContainsKey( tile_x ) ) {
				this.Glows[tile_x] = new Dictionary<ushort, byte>();
			}
			this.Glows[tile_x][tile_y] = glow;
		}

		public void RemoveGlowAt( ushort tile_x, ushort tile_y ) {
			if( this.Glows.ContainsKey( tile_x ) ) {
				this.Glows[tile_x].Remove( tile_y );
			}
		}


		////////////////

		public Color ComputeTileColor( ushort tile_x, ushort tile_y ) {
			Color end_color;
			Color paint_data = this.GetRawColorAt( tile_x, tile_y );
			Color flattened = XnaColorHelpers.FlattenColor( paint_data );

			Color env_color = Lighting.GetColor( tile_x, tile_y, flattened );

			float lit_scale = (float)this.GetGlowAt( tile_x, tile_y ) / 255f;

			if( lit_scale > 0 ) {
				float r_scale = (float)paint_data.R / 255f;
				float g_scale = (float)paint_data.G / 255f;
				float b_scale = (float)paint_data.B / 255f;

				int r = env_color.R + (int)( ( 255 - env_color.R ) * r_scale );
				int g = env_color.G + (int)( ( 255 - env_color.G ) * g_scale );
				int b = env_color.B + (int)( ( 255 - env_color.B ) * b_scale );

				end_color = new Color( r, g, b );
			} else {
				end_color = env_color;
			}

			return end_color;
		}
	}
}
