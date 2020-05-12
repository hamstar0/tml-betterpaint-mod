using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushStream : PaintBrush {
		public override float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brushSize, float pressurePercent,
				int randSeed, int worldX, int worldY ) {
			var mymod = BetterPaintMod.Instance;
			int iterRange = (int)((brushSize == PaintBrushSize.Small ? 1 : 3) * mymod.Config.BrushSizeMultiplier);
			float radius = (brushSize == PaintBrushSize.Small ? 0.5f : 3f) * mymod.Config.BrushSizeMultiplier;

			int tileX = worldX / 16;
			int tileY = worldY / 16;

			float uses = 0;
			
			for( int i = -iterRange; i <= iterRange; i++ ) {
				for( int j = -iterRange; j <= iterRange; j++ ) {
					float dist = (float)Math.Sqrt( (double)(( i * i ) + ( j * j )) );

					if( dist > radius ) {
						continue;
					}
					
					uses += this.PaintAt( layer, color, glow, pressurePercent, (ushort)(tileX + i), (ushort)(tileY + j) );
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
