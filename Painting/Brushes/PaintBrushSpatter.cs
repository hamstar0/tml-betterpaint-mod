using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushSpatter : PaintBrush {
		public override float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brushSize, float pressurePercent,
				int randSeed, int worldX, int worldY ) {
			var mymod = BetterPaintMod.Instance;
			var rand = new Random( randSeed );

			int tileX = worldX / 16;
			int tileY = worldY / 16;

			float uses = 0;

			int diameter = brushSize == PaintBrushSize.Small ? 3 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int spats = (int)( (float)(diameter * diameter) * mymod.Config.BrushSpatterDensity );

			int minRadius = Math.Max( 1, diameter / 3 );
			int extendedRadiusRange = diameter - minRadius;
			
			for( int i=0; i<spats; i++ ) {
				int extendedRadius = rand.Next( extendedRadiusRange );
				int radius = minRadius + extendedRadius;

				int xOff = rand.Next( -radius, radius );

				radius = minRadius + extendedRadius;
				int yOff = rand.Next( -radius, radius );

				float randPercent = pressurePercent * (1f - (float)rand.NextDouble());

				if( randPercent >= 0.01f ) {
					uses += this.PaintAt( layer, color, glow, randPercent, (ushort)( tileX + xOff ), (ushort)( tileY + yOff ) );
				}
			}

			return uses;
		}


		public float PaintAt( PaintLayer layer, Color color, byte glow, float pressurePercent, ushort tileX, ushort tileY ) {
			if( !layer.CanPaintAt( Main.tile[tileX, tileY] ) ) {
				return 0f;
			}
			
			Color? oldColor;
			byte oldGlow;

			Color blendedColor = PaintBrush.GetBlendedColor( layer, color, pressurePercent, tileX, tileY, out oldColor );
			byte blendedGlow = PaintBrush.GetBlendedGlow( layer, glow, pressurePercent, tileX, tileY, out oldGlow );

			layer.SetRawColorAt( blendedColor, tileX, tileY );
			layer.SetGlowAt( blendedGlow, tileX, tileY );

			float diff = PaintBrush.ComputeChangePercent( oldColor, blendedColor, oldGlow, blendedGlow );
			if( diff <= 0.01f ) {
				pressurePercent = 0f;
			}

			return pressurePercent;
			//return PaintBrush.ComputeColorChangePercent( existing_color, lerped_color );
		}
	}
}
