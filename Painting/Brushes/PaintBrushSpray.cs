using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushSpray : PaintBrush {
		public override float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brushSize, float pressurePercent,
				int randSeed, int worldX, int worldY ) {
			var mymod = BetterPaintMod.Instance;
			int diameter = brushSize == PaintBrushSize.Small ? 3 : 8;
			diameter = (int)((float)diameter * mymod.Config.BrushSizeMultiplier);

			int iterRange = diameter / 2;
			float maxRange = (float)diameter / 2f;
			float uses = 0;

			int tileX = worldX / 16;
			int tileY = worldY / 16;
			double tileOffsetX = (double)( worldX % 16 ) / 16f;
			double tileOffsetY = (double)( worldY % 16 ) / 16f;
			
			for( int i = -iterRange; i <= iterRange; i++ ) {
				for( int j = -iterRange; j <= iterRange; j++ ) {
					double iOff = i + tileOffsetX;
					double jOff = j + tileOffsetY;

					float dist = (float)Math.Sqrt( (iOff * iOff) + (jOff * jOff) );

					if( dist > maxRange ) {
						continue;
					}

					ushort x = (ushort)( tileX + i );
					ushort y = (ushort)( tileY + j );

					uses += this.PaintAt( layer, color, glow, pressurePercent, maxRange, dist, x, y );
				}
			}

			return uses;
		}

		
		public float PaintAt( PaintLayer layer, Color color, byte glow, float pressurePercent, float brushRadius, float dist,
				ushort tileX, ushort tileY ) {
			if( !layer.CanPaintAt( Main.tile[tileX, tileY] ) ) {
				return 0f;
			}
			
			float distPressurePercent = MathHelper.Clamp( pressurePercent * (1f - (dist / brushRadius)), 0f, 1f );
			
			Color? oldColor;
			byte oldGlow;

			Color blendedColor = PaintBrush.GetBlendedColor( layer, color, distPressurePercent, tileX, tileY, out oldColor );
			byte blendedGlow = PaintBrush.GetBlendedGlow( layer, glow, distPressurePercent, tileX, tileY, out oldGlow );

			layer.SetRawColorAt( blendedColor, tileX, tileY );
			layer.SetGlowAt( blendedGlow, tileX, tileY );

			float diff = PaintBrush.ComputeChangePercent( oldColor, blendedColor, oldGlow, blendedGlow );
			if( diff <= 0.01f ) {
				distPressurePercent = 0f;
			}

			return distPressurePercent;
		}
	}
}
