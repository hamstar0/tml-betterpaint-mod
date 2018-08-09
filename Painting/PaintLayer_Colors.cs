using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
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


		public Color? GetRawColorAt( ushort tile_x, ushort tile_y ) {
			if( this.Colors.ContainsKey( tile_x ) ) {
				if( this.Colors[tile_x].ContainsKey( tile_y ) ) {
					return (Color?)this.Colors[tile_x][tile_y];
				}
			}
			return null;
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

			this.SetMapColorAt( color, tile_x, tile_y );
		}
		
		public void RemoveRawColorAt( ushort tile_x, ushort tile_y ) {
			if( !this.Colors.ContainsKey( tile_x ) ) { return; }

			if( this.Colors[tile_x].Remove( tile_y ) ) {
				this.RemoveMapColorAt( tile_x, tile_y );
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
			Color? raw_color = this.GetRawColorAt( tile_x, tile_y );
			if( raw_color == null ) { return Color.Transparent; }

			Color computed;
			Color color = (Color)raw_color;
			Color flattened = XnaColorHelpers.FlattenColor( Color.White, color );
			Color env_color = Lighting.GetColor( tile_x, tile_y, flattened );

			float lit_percent = (float)this.GetGlowAt( tile_x, tile_y ) / 255f;

			if( lit_percent > 0 ) {
				float r_scale = (float)color.R / 255f;
				float g_scale = (float)color.G / 255f;
				float b_scale = (float)color.B / 255f;

				int r = env_color.R + (int)( (float)( 255 - env_color.R ) * r_scale * lit_percent );
				int g = env_color.G + (int)( (float)( 255 - env_color.G ) * g_scale * lit_percent );
				int b = env_color.B + (int)( (float)( 255 - env_color.B ) * b_scale * lit_percent );

				computed = new Color( r, g, b );
			} else {
				computed = env_color;
			}

			return computed;
		}
	}
}
