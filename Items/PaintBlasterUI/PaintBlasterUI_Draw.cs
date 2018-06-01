using HamstarHelpers.HudHelpers;
using HamstarHelpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public const float SelectedScale = 0.85f;
		public const float HoveredScale = 0.65f;
		public const float IdleScale = 0.3f;

		public const int BrushesRingRadius = 72;
		public const int OptionsRingRadius = 28;


		////////////////

		public void DrawUI( BetterPaintMod mymod, SpriteBatch sb ) {
			Rectangle stream_rect, spray_rect, bucket_rect, scrape_rect;
			Rectangle bg_rect, size_rect, copy_rect, press_rect;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			sb.Draw( Main.magicPixel, new Rectangle( x, y, 160, 160 ), null, Color.LightPink * 0.1f, (float)( 45d * ( Math.PI / 180d ) ), new Vector2( 0.5f, 512f ), SpriteEffects.None, 1f );
			HudHelpers.DrawBorderedRect( sb, Color.DarkOliveGreen * 0.4f, Color.OliveDrab * 0.4f, new Rectangle( x - 48, y - 48, 96, 96 ), 4 );

			IDictionary<int, float> palette_angles = this.DrawColorPalette( mymod, sb );
			this.DrawBrushes( sb, out stream_rect, out spray_rect, out bucket_rect, out scrape_rect );
			this.DrawOptionLayer( sb, x, y, out bg_rect );
			this.DrawOptionSize( sb, x, y, out size_rect );
			this.DrawOptionCopy( sb, x, y, out copy_rect );
			this.DrawOptionPressure( sb, x, y, out press_rect );

			if( Main.mouseLeft ) {
				if( !this.IsInteractingWithUI ) {
					this.IsInteractingWithUI = true;

					this.CheckUISettingsInteractions( bg_rect, size_rect, copy_rect, press_rect );
					this.CheckUIBrushInteractions( stream_rect, spray_rect, bucket_rect, scrape_rect );
					this.CheckUIColorInteractions( palette_angles );
				}
			} else {
				this.IsInteractingWithUI = false;
			}
		}


		public void DrawScreen( BetterPaintMod mymod, SpriteBatch sb ) {
			if( this.IsCopying ) {
				Texture2D copy_tex = Main.itemTexture[ItemID.EmptyDropper];
				var mouse_pos = new Vector2( Main.mouseX, Main.mouseY );

				sb.Draw( copy_tex, mouse_pos, Color.White );
			}
		}
	}
}
