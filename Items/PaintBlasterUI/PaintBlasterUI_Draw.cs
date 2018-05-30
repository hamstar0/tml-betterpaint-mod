using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public const float SelectedScale = 0.85f;
		public const float HoveredScale = 0.6f;
		public const float IdleScale = 0.35f;


		////////////////

		public void DrawUI( BetterPaintMod mymod, SpriteBatch sb ) {
			IDictionary<int, Rectangle> palette_rects = this.DrawColorPalette( mymod );
			
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;
			int brushes_dist = 72;
			int options_dist = 28;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;

			///

			Texture2D tex_brush = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_bucket = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_scrape = Main.itemTexture[ItemID.PaintScraper];

			var brush_offset = new Vector2( tex_brush.Width, tex_brush.Height ) * 0.5f;
			var spray_offset = new Vector2( tex_spray.Width, tex_spray.Height ) * 0.5f;
			var bucket_offset = new Vector2( tex_bucket.Width, tex_bucket.Height ) * 0.5f;
			var scrape_offset = new Vector2( tex_scrape.Width, tex_scrape.Height ) * 0.5f;

			var brush_rect = new Rectangle( (int)((x - brushes_dist) - brush_offset.X), (int)(y - brush_offset.Y), tex_brush.Width, tex_brush.Height );
			var spray_rect = new Rectangle( (int)(x - spray_offset.X), (int)((y - brushes_dist) - spray_offset.Y), tex_spray.Width, tex_spray.Height );
			var bucket_rect = new Rectangle( (int)((x + brushes_dist) - bucket_offset.X), (int)(y - bucket_offset.Y), tex_bucket.Width, tex_bucket.Height );
			var scrape_rect = new Rectangle( (int)(x - scrape_offset.X), (int)((y + brushes_dist) - scrape_offset.Y), tex_scrape.Width, tex_scrape.Height );

			bool brush_hover = brush_rect.Contains( Main.mouseX, Main.mouseY );
			bool spray_hover = brush_hover ? false : spray_rect.Contains( Main.mouseX, Main.mouseY );
			bool bucket_hover = spray_hover ? false : bucket_rect.Contains( Main.mouseX, Main.mouseY );
			bool scrape_hover = bucket_hover ? false : scrape_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( tex_brush, brush_rect, Color.White * (this.CurrentMode == PaintModeType.Stream ? hilit : (brush_hover ? lit : unlit)) );
			sb.Draw( tex_spray, spray_rect, Color.White * (this.CurrentMode == PaintModeType.Spray ? hilit : (spray_hover ? lit : unlit)) );
			sb.Draw( tex_bucket, bucket_rect, Color.White * (this.CurrentMode == PaintModeType.Spatter ? hilit : (bucket_hover ? lit : unlit)) );
			sb.Draw( tex_scrape, scrape_rect, Color.White * (this.CurrentMode == PaintModeType.Erase ? hilit : (scrape_hover ? lit : unlit)) );

			if( brush_hover ) {
				var tool_color = this.CurrentMode == PaintModeType.Stream ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Stream Mode", new Vector2(brush_rect.X, brush_rect.Y+brush_rect.Height), tool_color );
			} else if( spray_hover ) {
				var tool_color = this.CurrentMode == PaintModeType.Spray ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Spray Mode", new Vector2(spray_rect.X, spray_rect.Y+spray_rect.Height), tool_color );
			} else if( bucket_hover ) {
				var tool_color = this.CurrentMode == PaintModeType.Spatter ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Spatter Mode", new Vector2(bucket_rect.X, bucket_rect.Y+bucket_rect.Height), tool_color );
			} else if( scrape_hover ) {
				var tool_color = this.CurrentMode == PaintModeType.Erase ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Eraser Mode", new Vector2(scrape_rect.X, scrape_rect.Y+scrape_rect.Height), tool_color );
			}

			///

			Texture2D bg_tex = this.Foreground ? PaintBlasterUI.BgOffButtonTex : PaintBlasterUI.BgOnButtonTex;

			var bg_offset = new Vector2( bg_tex.Width, bg_tex.Height ) * 0.5f;

			int bg_x = (x - options_dist) - (int)bg_offset.X;
			int bg_y = (y - options_dist) - (int)bg_offset.Y;
			var bg_rect = new Rectangle( bg_x, bg_y, bg_tex.Width, bg_tex.Height );

			bool bg_hover = bg_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( bg_tex, bg_rect, Color.White * (bg_hover ? hilit : lit) );

			if( bg_hover ) {
				string bg_str = this.Foreground ? "Foreground Only" : "Background Only";
				sb.DrawString( Main.fontMouseText, bg_str, new Vector2(bg_rect.X, bg_rect.Y - 16), Color.White );
			}

			///

			Texture2D size_tex = this.BrushSize == 1 ? PaintBlasterUI.BrushSmallTex : PaintBlasterUI.BrushLargeTex;

			var size_offset = new Vector2( size_tex.Width, size_tex.Height ) * 0.5f;

			int size_x = (x + options_dist) - (int)size_offset.X;
			int size_y = (y + options_dist) - (int)size_offset.Y;
			var size_rect = new Rectangle( size_x, size_y, size_tex.Width, size_tex.Height );

			bool size_hover = size_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( size_tex, size_rect, Color.White * (size_hover ? hilit : lit) );

			if( size_hover ) {
				string size_str = this.BrushSize > 1 ? "Large brush" : "Small brush";
				sb.DrawString( Main.fontMouseText, size_str, new Vector2(size_rect.X, size_rect.Y + size_rect.Height), Color.White );
			}

			///

			Texture2D eye_tex = PaintBlasterUI.EyedropperTex;

			var eye_offset = new Vector2( eye_tex.Width, eye_tex.Height ) * 0.5f;

			int eye_x = ( x + options_dist ) - (int)eye_offset.X;
			int eye_y = ( y + options_dist ) - (int)eye_offset.Y;
			var eye_rect = new Rectangle( eye_x, eye_y, eye_tex.Width, eye_tex.Height );

			bool eye_hover = eye_rect.Contains( Main.mouseX, Main.mouseY );
			Color eye_color = Color.White * ( this.IsEyedropping ? 1f : (eye_hover ? lit : unlit) );

			sb.Draw( eye_tex, eye_rect, eye_color );

			if( eye_hover ) {
				string eye_str = "Eyedropper (needs Camo Cartridges)";
				sb.DrawString( Main.fontMouseText, eye_str, new Vector2( eye_rect.X, eye_rect.Y + eye_rect.Height ), Color.White );
			}

			///

			if( Main.mouseLeft ) {
				if( !this.IsInteractingWithUI ) {
					this.IsInteractingWithUI = true;

					this.CheckUIModeInteractions( ref bg_rect, ref size_rect, ref brush_rect, ref spray_rect, ref bucket_rect, ref scrape_rect );
					this.CheckUIColorInteractions( palette_rects );
				}
			} else {
				this.IsInteractingWithUI = false;
			}
		}


		public IDictionary<int, Rectangle> DrawColorPalette( BetterPaintMod mymod ) {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			var rects = new Dictionary<int, Rectangle>();

			double angle_step = 360d / (double)item_idxs.Count;
			double angle = 0d;
			
			foreach( int idx in item_idxs ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * (Math.PI / 180d) ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * (Math.PI / 180d) ) );

				Item item = Main.LocalPlayer.inventory[ idx ];
				var myitem = (ColorCartridgeItem)item.modItem;

				rects[ idx ] = this.DrawPaintIcon( mymod, myitem.MyColor, myitem.TimesUsed, x, y, (idx == this.CurrentCartridgeInventoryIndex) );

				angle += angle_step;
			}

			return rects;
		}
		
		public Rectangle DrawPaintIcon( BetterPaintMod mymod, Color color, float uses, int x, int y, bool is_selected ) {
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			bool is_hover = rect.Contains( Main.mouseX, Main.mouseY );
			float color_mul = is_selected ? PaintBlasterUI.SelectedScale :
				( is_hover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );
			
			Main.spriteBatch.Draw( cart_tex, rect, Color.White * color_mul );
			Main.spriteBatch.Draw( over_tex, rect, color * color_mul );

			if( is_hover ) {
				float percent = 1f - (uses / (float)mymod.Config.PaintCartridgeCapacity);
				Color text_color = ColorCartridgeItem.GetCapacityColor( percent );
				Color label_color = Color.White * PaintBlasterUI.HoveredScale;
				
				Main.spriteBatch.DrawString( Main.fontMouseText, "Capacity:", new Vector2(Main.mouseX, Main.mouseY-16), label_color );
				Main.spriteBatch.DrawString( Main.fontMouseText, (int)(percent * 100)+"%", new Vector2(Main.mouseX+72, Main.mouseY-16), text_color );

				string color_str = "R:"+color.R+" G:"+color.G+" B:"+color.B+" A:"+color.A;

				Main.spriteBatch.DrawString( Main.fontMouseText, "Color:", new Vector2( Main.mouseX, Main.mouseY + 8 ), label_color );
				Main.spriteBatch.DrawString( Main.fontMouseText, color_str, new Vector2( Main.mouseX+56, Main.mouseY + 8 ), color );
			}

			return rect;
		}
	}
}
