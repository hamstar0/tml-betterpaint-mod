using HamstarHelpers.Helpers.XNA;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public abstract bool CanPaintAt( Tile tile );


		////////////////
		
		public bool HasColorAt( ushort tileX, ushort tileY ) {
			return this.Colors.ContainsKey( tileX ) && this.Colors[tileX].ContainsKey( tileY );
		}


		public Color? GetRawColorAt( ushort tileX, ushort tileY ) {
			if( this.Colors.ContainsKey( tileX ) ) {
				if( this.Colors[tileX].ContainsKey( tileY ) ) {
					return (Color?)this.Colors[tileX][tileY];
				}
			}
			return null;
		}


		public byte GetGlowAt( ushort tileX, ushort tileY ) {
			if( this.Glows.ContainsKey( tileX ) ) {
				if( this.Glows[tileX].ContainsKey( tileY ) ) {
					return this.Glows[tileX][tileY];
				}
			}
			return 0;
		}


		////////////////
		
		public void SetRawColorAt( Color color, ushort tileX, ushort tileY ) {
			if( !this.Colors.ContainsKey(tileX) ) {
				this.Colors[tileX] = new Dictionary<ushort, Color>();
			}
			this.Colors[tileX][tileY] = color;

			this.SetMapColorAt( color, tileX, tileY );
		}
		
		public void RemoveRawColorAt( ushort tileX, ushort tileY ) {
			if( !this.Colors.ContainsKey( tileX ) ) { return; }

			if( this.Colors[tileX].Remove( tileY ) ) {
				this.RemoveMapColorAt( tileX, tileY );
			}
		}


		////////////////

		public void SetGlowAt( byte glow, ushort tileX, ushort tileY ) {
			if( !this.Glows.ContainsKey( tileX ) ) {
				this.Glows[tileX] = new Dictionary<ushort, byte>();
			}
			this.Glows[tileX][tileY] = glow;
		}

		public void RemoveGlowAt( ushort tileX, ushort tileY ) {
			if( this.Glows.ContainsKey( tileX ) ) {
				this.Glows[tileX].Remove( tileY );
			}
		}


		////////////////

		public Color ComputeTileColor( ushort tileX, ushort tileY ) {
			Color? rawColor = this.GetRawColorAt( tileX, tileY );
			if( rawColor == null ) { return Color.Transparent; }

			Color computed;
			Color color = (Color)rawColor;
			Color flattened = XNAColorHelpers.FlattenColor( Color.White, color );
			Color envColor = Lighting.GetColor( tileX, tileY, flattened );

			float litPercent = (float)this.GetGlowAt( tileX, tileY ) / 255f;

			if( litPercent > 0 ) {
				float rScale = (float)color.R / 255f;
				float gScale = (float)color.G / 255f;
				float bScale = (float)color.B / 255f;

				int r = envColor.R + (int)( (float)( 255 - envColor.R ) * rScale * litPercent );
				int g = envColor.G + (int)( (float)( 255 - envColor.G ) * gScale * litPercent );
				int b = envColor.B + (int)( (float)( 255 - envColor.B ) * bScale * litPercent );

				computed = new Color( r, g, b );
			} else {
				computed = envColor;
			}

			return computed;
		}
	}
}
