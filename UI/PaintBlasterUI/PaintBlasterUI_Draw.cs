using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		public const float SelectedScale = 0.925f;
		public const float HoveredScale = 0.8f;
		public const float IdleScale = 0.5f;

		public const int BrushesRingRadius = 72;
		public const int OptionsRingRadius = 28;


		////////////////

		public void DrawUI( BetterPaintMod mymod, SpriteBatch sb ) {
			Rectangle streamRect, sprayRect, bucketRect, scrapeRect;
			Rectangle bgRect, sizeRect, copyRect, pressRect;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			sb.Draw( Main.magicPixel, new Rectangle( x, y, 160, 160 ), null, Color.LightPink * 0.3f, (float)( 45d * DotNetHelpers.RadDeg ), new Vector2( 0.5f, 512f ), SpriteEffects.None, 1f );
			HudHelpers.DrawBorderedRect( sb, Color.DarkOliveGreen * 0.5f, Color.OliveDrab * 0.5f, new Rectangle( x - 48, y - 48, 96, 96 ), 4 );

			this.DrawBrushes( sb, out streamRect, out sprayRect, out bucketRect, out scrapeRect );
			this.DrawOptionLayer( sb, x, y, out bgRect );
			this.DrawOptionSize( sb, x, y, out sizeRect );
			this.DrawOptionCopy( sb, x, y, out copyRect );
			this.DrawOptionPressure( sb, x, y, out pressRect );
			IDictionary<int, float> paletteAngles = this.DrawColorPalette( mymod, sb );

			if( Main.mouseLeft ) {
				if( !this.IsInteractingWithUI ) {
					this.IsInteractingWithUI = true;

					bool hasInteracted = false;

					hasInteracted = this.CheckUISettingsInteractions( bgRect, sizeRect, copyRect, pressRect );
					hasInteracted |= this.CheckUIBrushInteractions( streamRect, sprayRect, bucketRect, scrapeRect );
					hasInteracted |= this.CheckUIColorInteractions( paletteAngles );

					if( hasInteracted ) {
						Main.PlaySound( SoundID.MenuTick );
					}
				}
			} else {
				this.IsInteractingWithUI = false;
			}
		}


		public void DrawScreen( BetterPaintMod mymod, SpriteBatch sb ) {
			if( this.IsCopying ) {
				Texture2D copyTex = Main.itemTexture[ItemID.EmptyDropper];
				var mousePos = new Vector2( Main.mouseX, Main.mouseY );

				sb.Draw( copyTex, mousePos, Color.White );
			}
		}
	}
}
