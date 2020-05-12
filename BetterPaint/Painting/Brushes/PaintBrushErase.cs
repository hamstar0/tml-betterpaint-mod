using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting.Brushes {
	class PaintBrushErase : PaintBrush {
		public override float Apply( PaintLayer layer, Color _, byte __, PaintBrushSize brushSize, float pressurePercent,
				int randSeed, int worldX, int worldY ) {
			var mymod = BetterPaintMod.Instance;

			int diameter = brushSize == PaintBrushSize.Small ? 1 : 6;
			diameter = (int)( (float)diameter * mymod.Config.BrushSizeMultiplier );

			int iterRange = Math.Max( 1, diameter / 2 );
			float radius = (float)diameter / 2f;

			int tileX = worldX / 16;
			int tileY = worldY / 16;
			
			for( int i = -iterRange; i <= iterRange; i++ ) {
				for( int j = -iterRange; j <= iterRange; j++ ) {
					float dist = (float)Math.Sqrt( (double)((i * i) + (j * j)) );

					if( dist > radius ) {
						continue;
					}

					this.EraseAt( layer, pressurePercent, (ushort)(tileX + i), (ushort)(tileY + j) );
				}
			}

			return 0;
		}


		public void EraseAt( PaintLayer layer, float pressurePercent, ushort tileX, ushort tileY ) {
			if( !layer.CanPaintAt( Main.tile[tileX, tileY] ) ) {
				return;
			}
			if( !layer.HasColorAt( tileX, tileY ) ) {
				return;
			}

			int tolerance = (int)( pressurePercent * 255f );

			Color? oldColor;
			byte oldGlow;

			Color blendedColor = PaintBrush.GetErasedColor( layer, pressurePercent, tileX, tileY, out oldColor );
			byte blendedGlow = PaintBrush.GetBlendedGlow( layer, 0, pressurePercent, tileX, tileY, out oldGlow );

			float diff = PaintBrush.ComputeChangePercent( oldColor, blendedColor, oldGlow, blendedGlow );

			if( diff <= 0.01f ) {
				layer.RemoveRawColorAt( tileX, tileY );
				layer.RemoveGlowAt( tileX, tileY );
			} else {
				layer.SetRawColorAt( blendedColor, tileX, tileY );
				layer.SetGlowAt( blendedGlow, tileX, tileY );
			}
		}
	}
}
