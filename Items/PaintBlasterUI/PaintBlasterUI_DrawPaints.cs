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
			var stack_idx_cart = new Dictionary<string, object[]>( item_idxs.Count );
			var rects = new Dictionary<int, Rectangle>( item_idxs.Count );

			foreach( int idx in item_idxs ) {
				Item item = Main.LocalPlayer.inventory[idx];
				var cart = (ColorCartridgeItem)item.modItem;
				if( cart.RemainingCapacity() == 0 ) { continue; }

				string color_key = cart.MyColor.ToString();

				if( !stack_idx_cart.ContainsKey( color_key ) ) {
					stack_idx_cart[color_key] = new object[] { 1, idx, cart };
				} else {
					stack_idx_cart[color_key][0] = (int)stack_idx_cart[color_key][0] + 1;
				}
			}

			double angle_step = 360d / (double)stack_idx_cart.Count;
			double angle = 0d;
			
			foreach( var kv in stack_idx_cart ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * (Math.PI / 180d) ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * (Math.PI / 180d) ) );

				var stack = (int)kv.Value[0];
				var idx = (int)kv.Value[1];
				var cart = (ColorCartridgeItem)kv.Value[2];

				rects[ idx ] = this.DrawColorIcon( mymod, sb, cart.MyColor, cart.TimesUsed, stack, x, y, (idx == this.CurrentCartridgeInventoryIndex) );

				angle += angle_step;
			}

			return rects;
		}


		////////////////

		public Rectangle DrawColorIcon( BetterPaintMod mymod, SpriteBatch sb, Color color, float uses, int stack, int x, int y, bool is_selected ) {
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

			sb.DrawString( Main.fontItemStack, stack+"", new Vector2((rect.X+cart_tex.Width)-4, (rect.Y+cart_tex.Height)-4), Color.White );

			return rect;
		}
	}
}
