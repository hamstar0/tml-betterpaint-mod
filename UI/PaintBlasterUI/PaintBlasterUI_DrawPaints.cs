using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
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
			IDictionary<string, PaintDisplayInfo> info_set = PaintDisplayInfo.GetPaintSelection( Main.LocalPlayer );
			var angles = new Dictionary<int, float>( info_set.Count );
			
			double angle_step = 360d / (double)info_set.Count;
			double angle = 0d;
			double radpi = Math.PI / 180d;
			
			foreach( var info in info_set.Values ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * radpi ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * radpi ) );
				int stack;
				float percent;
				Color color;

				info.GetDrawInfo( mymod, x, y, out color, out percent, out stack );
				
				this.DrawColorIcon( mymod, sb, info.PaintItem.type, color, percent, stack, x, y, angle, angle_step,
					(info.FirstInventoryIndex == this.CurrentPaintItemInventoryIndex) );

				angles[ info.FirstInventoryIndex ] = (float)angle;

				angle += angle_step;
			}

			return angles;
		}


		////////////////

		public Texture2D GetPaintTexture( BetterPaintMod mymod, int item_type ) {
			if( item_type == mymod.ItemType<ColorCartridgeItem>() ) {
				return ColorCartridgeItem.ColorCartridgeTex;
			} else if( item_type == mymod.ItemType<GlowCartridgeItem>() ) {
				return GlowCartridgeItem.GlowCartridgeTex;
			} else if( ItemIdentityHelpers.Paints.Item2.Contains( item_type ) ) {
				return Main.itemTexture[item_type];
			} else {
				throw new NotImplementedException();
			}
		}

		public Texture2D GetPaintOverlayTexture( BetterPaintMod mymod, int item_type, out bool has_glow ) {
			if( item_type == mymod.ItemType<ColorCartridgeItem>() ) {
				has_glow = false;
				return ColorCartridgeItem.ColorOverlayTex;
			} else if( item_type == mymod.ItemType<GlowCartridgeItem>() ) {
				has_glow = true;
				return GlowCartridgeItem.ColorOverlayTex;
			} else if( ItemIdentityHelpers.Paints.Item2.Contains( item_type ) ) {
				has_glow = false;
				return null;
			} else {
				throw new NotImplementedException();
			}
		}


		////////////////

		public Rectangle DrawColorIcon( BetterPaintMod mymod, SpriteBatch sb, int item_type, Color color, float amount_percent, int stack, int x, int y, double palette_angle, double angle_step, bool is_selected ) {
			bool has_glow;
			Texture2D cart_tex = this.GetPaintTexture( mymod, item_type );
			Texture2D over_tex = this.GetPaintOverlayTexture( mymod, item_type, out has_glow );

			bool is_hover = this.IsHoveringIcon( palette_angle, angle_step );

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			float color_mul = is_selected ? PaintBlasterUI.SelectedScale :
				( is_hover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );

			sb.Draw( cart_tex, rect, Color.White * color_mul );
			if( over_tex != null ) {
				sb.Draw( over_tex, rect, (has_glow ? color : color * color_mul) );
			}

			if( is_hover ) {
				Color text_color = ColorCartridgeItem.GetCapacityColor( amount_percent );
				Color label_color = has_glow ? Color.GreenYellow * PaintBlasterUI.HoveredScale
					: Color.White * PaintBlasterUI.HoveredScale;
				Color bg_color = Color.Black * PaintBlasterUI.HoveredScale;

				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Capacity:", Main.mouseX, Main.mouseY-16, label_color, bg_color, default( Vector2 ), 1f );
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, (int)( amount_percent * 100 ) + "%", Main.mouseX+72, Main.mouseY - 16, label_color, bg_color, default( Vector2 ), 1f );

				string color_str = PaintHelpers.ColorString( color );

				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Color:", Main.mouseX, Main.mouseY + 8, label_color, bg_color, default( Vector2 ), 1f );
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, color_str, Main.mouseX + 56, Main.mouseY + 8, color, bg_color, default( Vector2 ), 1f );
			}

			if( is_selected ) {
				Rectangle sel_rect = rect;
				sel_rect.X -= 3;
				sel_rect.Y -= 3;
				sel_rect.Width += 6;
				sel_rect.Height += 6;

				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, sel_rect, 2 );
			}

			Utils.DrawBorderStringFourWay( sb, Main.fontItemStack, stack + "", (rect.X+cart_tex.Width)-4, (rect.Y+cart_tex.Height)-12, Color.White, Color.Black, default( Vector2 ), 1f );

			return rect;
		}
	}
}
