using HamstarHelpers.DebugHelpers;
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
		public bool IsHoveringIcon( double palette_angle, double angle_step ) {
			var screen_mid = new Vector2( Main.screenWidth / 2, Main.screenHeight / 2 );
			var mouse_pos = new Vector2( Main.mouseX, Main.mouseY );

			if( Vector2.Distance(screen_mid, mouse_pos) < (PaintBlasterUI.BrushesRingRadius + 16) ) {
				return false;
			}

			double myangle = Math.Atan2( (double)( mouse_pos.Y - screen_mid.Y ), (double)( mouse_pos.X - screen_mid.X ) ) * ( 180d / Math.PI );
			myangle = myangle < 0 ? 360 + myangle : myangle;

			return Math.Abs( palette_angle - myangle ) <= ( angle_step * 0.5d ) ||
				Math.Abs( (360 + palette_angle) - myangle ) <= ( angle_step * 0.5d );
			//return rect.Contains( Main.mouseX, Main.mouseY );
		}



		public IDictionary<int, float> DrawColorPalette( BetterPaintMod mymod, SpriteBatch sb ) {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			var stack_idx_cart = new Dictionary<string, object[]>( item_idxs.Count );
			var angles = new Dictionary<int, float>( item_idxs.Count );

			foreach( int idx in item_idxs ) {
				Item item = Main.LocalPlayer.inventory[idx];
				var cart = (ColorCartridgeItem)item.modItem;
				if( cart.PaintQuantity == 0 ) { continue; }

				string color_key = cart.MyColor.ToString();

				if( !stack_idx_cart.ContainsKey( color_key ) ) {
					stack_idx_cart[color_key] = new object[] { 1, idx, cart };
				} else {
					stack_idx_cart[color_key][0] = (int)stack_idx_cart[color_key][0] + 1;
				}
			}

			double angle_step = 360d / (double)stack_idx_cart.Count;
			double angle = 0d;
			double radpi = Math.PI / 180d;
			
			foreach( var kv in stack_idx_cart ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * radpi ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * radpi ) );

				var stack = (int)kv.Value[0];
				var idx = (int)kv.Value[1];
				var cart = (ColorCartridgeItem)kv.Value[2];

				this.DrawColorIcon( mymod, sb, cart.MyColor, cart.PaintQuantity, stack, x, y, angle, angle_step,
					( idx == this.CurrentCartridgeInventoryIndex) );

				angles[idx] = (float)angle;

				angle += angle_step;
			}

			return angles;
		}


		////////////////

		public Rectangle DrawColorIcon( BetterPaintMod mymod, SpriteBatch sb, Color color, float paint_amount, int stack, int x, int y, double palette_angle, double angle_step, bool is_selected ) {
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			bool is_hover = this.IsHoveringIcon( palette_angle, angle_step );

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			float color_mul = is_selected ? PaintBlasterUI.SelectedScale :
				( is_hover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );

			sb.Draw( cart_tex, rect, Color.White * color_mul );
			sb.Draw( over_tex, rect, color * color_mul );

			if( is_hover ) {
				float percent = paint_amount / (float)mymod.Config.PaintCartridgeCapacity;
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

				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, sel_rect, 2 );
			}

			sb.DrawString( Main.fontItemStack, stack+"", new Vector2((rect.X+cart_tex.Width)-4, (rect.Y+cart_tex.Height)-12), Color.White );

			return rect;
		}
	}
}
