using HamstarHelpers.HudHelpers;
using HamstarHelpers.Utilities.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public IDictionary<int, Rectangle> DrawColorPalette( BetterPaintMod mymod, SpriteBatch sb ) {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			var rects = new Dictionary<int, Rectangle>();

			double angle_step = 360d / (double)item_idxs.Count;
			double angle = 0d;
			
			foreach( int idx in item_idxs ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * (Math.PI / 180d) ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * (Math.PI / 180d) ) );

				Item item = Main.LocalPlayer.inventory[ idx ];
				var myitem = (ColorCartridgeItem)item.modItem;

				rects[ idx ] = this.DrawColorIcon( mymod, sb, myitem.MyColor, myitem.TimesUsed, x, y, (idx == this.CurrentCartridgeInventoryIndex) );

				angle += angle_step;
			}

			return rects;
		}


		////////////////

		public Rectangle DrawColorIcon( BetterPaintMod mymod, SpriteBatch sb, Color color, float uses, int x, int y, bool is_selected ) {
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			bool is_hover = rect.Contains( Main.mouseX, Main.mouseY );
			float color_mul = is_selected ? PaintBlasterUI.SelectedScale :
				( is_hover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );

			sb.Draw( cart_tex, rect, Color.White * color_mul );
			sb.Draw( over_tex, rect, color * color_mul );

			if( is_hover ) {
				float percent = 1f - (uses / (float)mymod.Config.PaintCartridgeCapacity);
				Color text_color = ColorCartridgeItem.GetCapacityColor( percent );
				Color label_color = Color.White * PaintBlasterUI.HoveredScale;

				sb.DrawString( Main.fontMouseText, "Capacity:", new Vector2(Main.mouseX, Main.mouseY-16), label_color );
				sb.DrawString( Main.fontMouseText, (int)(percent * 100)+"%", new Vector2(Main.mouseX+72, Main.mouseY-16), text_color );

				string color_str = "R:"+color.R+" G:"+color.G+" B:"+color.B+" A:"+color.A;

				sb.DrawString( Main.fontMouseText, "Color:", new Vector2( Main.mouseX, Main.mouseY + 8 ), label_color );
				sb.DrawString( Main.fontMouseText, color_str, new Vector2( Main.mouseX+56, Main.mouseY + 8 ), color );
			}

			if( is_selected ) {
				Rectangle sel_rect = rect;
				sel_rect.X -= 3;
				sel_rect.Y -= 3;
				sel_rect.Width += 6;
				sel_rect.Height += 6;

				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, sel_rect, 2 );
			}

			return rect;
		}
	}
}
