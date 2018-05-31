using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public const float SelectedScale = 0.85f;
		public const float HoveredScale = 0.6f;
		public const float IdleScale = 0.35f;

		public const int BrushesRingRadius = 72;
		public const int OptionsRingRadius = 28;


		////////////////

		public void DrawUI( BetterPaintMod mymod, SpriteBatch sb ) {
			Rectangle stream_rect, spray_rect, bucket_rect, scrape_rect;
			Rectangle bg_rect, size_rect, copy_rect, press_rect;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			IDictionary<int, Rectangle> palette_rects = this.DrawColorPalette( mymod, sb );
			this.DrawBrushes( sb, out stream_rect, out spray_rect, out bucket_rect, out scrape_rect );
			this.DrawOptionLayer( sb, x, y, out bg_rect );
			this.DrawOptionSize( sb, x, y, out size_rect );
			this.DrawOptionCopy( sb, x, y, out copy_rect );
			this.DrawOptionPressure( sb, x, y, out press_rect );

			if( Main.mouseLeft ) {
				if( !this.IsInteractingWithUI ) {
					this.IsInteractingWithUI = true;

					this.CheckUISettingsInteractions( ref bg_rect, ref size_rect, ref copy_rect, ref press_rect );
					this.CheckUIBrushInteractions( ref stream_rect, ref spray_rect, ref bucket_rect, ref scrape_rect );
					this.CheckUIColorInteractions( palette_rects );
				}
			} else {
				this.IsInteractingWithUI = false;
			}
			
			this.UpdateUI( mymod, Main.LocalPlayer );

			this.PostDrawUI( sb );
		}


		private void PostDrawUI( SpriteBatch sb ) {
			if( this.IsCopying ) {
				Texture2D copy_tex = Main.itemTexture[ItemID.EmptyDropper];
				var mouse_pos = new Vector2( Main.mouseX, Main.mouseY );

				sb.Draw( copy_tex, mouse_pos, Color.White );
			}
		}
	}
}
