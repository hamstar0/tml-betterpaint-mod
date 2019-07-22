using HamstarHelpers.Helpers.XNA;
using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting.Brushes {
	public enum PaintLayerType : int {
		Foreground,
		Background,
		Anyground
	}

	
	public enum PaintBrushType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}


	public enum PaintBrushSize : int {
		Small,
		Large
	}



	public abstract class PaintBrush {
		public static float ComputeChangePercent( Color? oldColor, Color newColor, byte oldGlow, byte newGlow ) {
			return PaintBrush.ComputeColorChangePercent( oldColor, newColor ) +
				(PaintBrush.ComputeGlowChangePercent( oldGlow, newGlow ) / 4);
		}

		public static float ComputeColorChangePercent( Color? oldColor, Color newColor ) {
			Color baseColor = oldColor != null ? (Color)oldColor : Color.Transparent;

			Color diffColor = XNAColorHelpers.DifferenceRGB( baseColor, newColor );
			float diffValue = XNAColorHelpers.AvgRGB( diffColor );

			return diffValue / 255f;
		}

		public static float ComputeGlowChangePercent( byte oldGlow, byte newGlow ) {
			int diff = Math.Abs( newGlow - oldGlow );

			return (float)diff / 255f;
		}
		
		////////////////

		public static Color GetBlendedColor( PaintLayer layer, Color rgbColor, float pressurePercent, ushort tileX, ushort tileY,
				out Color? oldColor ) {
			Color blendedColor;
			float oldPressurePercent;
			oldColor = layer.GetRawColorAt( tileX, tileY );

			if( oldColor != null ) {
				Color oldRgbColor = (Color)oldColor;

				oldPressurePercent = oldRgbColor.A / 255f;
				float oldPressurePercentOffset = (1f - pressurePercent) * oldPressurePercent;
				
				blendedColor = Color.Lerp( rgbColor, oldRgbColor, oldPressurePercentOffset );
			} else {
				oldPressurePercent = 0f;
				blendedColor = rgbColor;
			}

			blendedColor.A = (byte)(MathHelper.Lerp( oldPressurePercent, 1f, pressurePercent ) * 255f);

			return blendedColor;
		}


		public static Color GetErasedColor( PaintLayer layer, float pressurePercent, ushort tileX, ushort tileY, out Color? oldColor ) {
			oldColor = layer.GetRawColorAt( tileX, tileY );
			if( oldColor == null ) { return Color.Transparent; }

			Color blendedColor = (Color)oldColor;
			blendedColor.A = (byte)( MathHelper.Lerp( blendedColor.A, 0f, pressurePercent ) * 255f );

			return blendedColor;
		}


		public static byte GetBlendedGlow( PaintLayer layer, byte glow, float pressurePercent, ushort tileX, ushort tileY, out byte oldGlow ) {
			byte blendedGlow;
			
			oldGlow = layer.GetGlowAt( tileX, tileY );

			if( glow != 0 ) {
				byte pressuredGlow = (byte)( (float)glow * pressurePercent );

				blendedGlow = (byte)MathHelper.Lerp( (float)oldGlow, 255f, (float)pressuredGlow / 255f );
			} else {
				blendedGlow = (byte)MathHelper.Lerp( (float)oldGlow, 0f, pressurePercent );
			}

			return blendedGlow;
		}


		////////////////

		public abstract float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brushSize, float pressurePercent,
			int randSeed, int worldX, int worldY );
	}
}
