using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const float SelectedScale = 0.85f;
		public const float HoveredScale = 0.6f;
		public const float IdleScale = 0.35f;


		////////////////

		public void DrawPainterUI( SpriteBatch sb ) {
			IDictionary<int, Rectangle> palette_rects = this.DrawColorPalette();
			
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;
			int mode_dist = 72;
			int setting_dist = 28;
			float hilit = PaintBlasterItem.SelectedScale;
			float lit = PaintBlasterItem.HoveredScale;
			float unlit = PaintBlasterItem.IdleScale;

			///

			Texture2D tex_brush = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_bucket = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_scrape = Main.itemTexture[ItemID.PaintScraper];

			var brush_offset = new Vector2( tex_brush.Width, tex_brush.Height ) * 0.5f;
			var spray_offset = new Vector2( tex_spray.Width, tex_spray.Height ) * 0.5f;
			var bucket_offset = new Vector2( tex_bucket.Width, tex_bucket.Height ) * 0.5f;
			var scrape_offset = new Vector2( tex_scrape.Width, tex_scrape.Height ) * 0.5f;

			var brush_rect = new Rectangle( (int)((x - mode_dist) - brush_offset.X), (int)(y - brush_offset.Y), tex_brush.Width, tex_brush.Height );
			var spray_rect = new Rectangle( (int)(x - spray_offset.X), (int)((y - mode_dist) - spray_offset.Y), tex_spray.Width, tex_spray.Height );
			var bucket_rect = new Rectangle( (int)((x + mode_dist) - bucket_offset.X), (int)(y - bucket_offset.Y), tex_bucket.Width, tex_bucket.Height );
			var scrape_rect = new Rectangle( (int)(x - scrape_offset.X), (int)((y + mode_dist) - scrape_offset.Y), tex_scrape.Width, tex_scrape.Height );

			bool brush_hover = brush_rect.Contains( Main.mouseX, Main.mouseY );
			bool spray_hover = brush_hover ? false : spray_rect.Contains( Main.mouseX, Main.mouseY );
			bool bucket_hover = spray_hover ? false : bucket_rect.Contains( Main.mouseX, Main.mouseY );
			bool scrape_hover = bucket_hover ? false : scrape_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( tex_brush, brush_rect, Color.White * (this.CurrentMode == PaintMode.Stream ? hilit : (brush_hover ? lit : unlit)) );
			sb.Draw( tex_spray, spray_rect, Color.White * (this.CurrentMode == PaintMode.Spray ? hilit : (spray_hover ? lit : unlit)) );
			sb.Draw( tex_bucket, bucket_rect, Color.White * (this.CurrentMode == PaintMode.Flood ? hilit : (bucket_hover ? lit : unlit)) );
			sb.Draw( tex_scrape, scrape_rect, Color.White * (this.CurrentMode == PaintMode.Erase ? hilit : (scrape_hover ? lit : unlit)) );

			if( brush_hover ) {
				var tool_color = this.CurrentMode == PaintMode.Stream ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Stream Mode", new Vector2(brush_rect.X, brush_rect.Y+brush_rect.Height), tool_color );
			} else if( spray_hover ) {
				var tool_color = this.CurrentMode == PaintMode.Spray ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Spray Mode", new Vector2(spray_rect.X, spray_rect.Y+spray_rect.Height), tool_color );
			} else if( bucket_hover ) {
				var tool_color = this.CurrentMode == PaintMode.Flood ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Flood Fill Mode", new Vector2(bucket_rect.X, bucket_rect.Y+bucket_rect.Height), tool_color );
			} else if( scrape_hover ) {
				var tool_color = this.CurrentMode == PaintMode.Erase ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Erasor Mode", new Vector2(scrape_rect.X, scrape_rect.Y+scrape_rect.Height), tool_color );
			}

			///

			Texture2D bg_tex = this.Foreground ? PaintBlasterItem.BgOffButtonTex : PaintBlasterItem.BgOnButtonTex;

			var bg_offset = new Vector2( bg_tex.Width, bg_tex.Height ) * 0.5f;

			int bg_x = (x - setting_dist) - (int)bg_offset.X;
			int bg_y = (y - setting_dist) - (int)bg_offset.Y;
			var bg_rect = new Rectangle( bg_x, bg_y, bg_tex.Width, bg_tex.Height );

			bool bg_hover = bg_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( bg_tex, bg_rect, Color.White * (bg_hover ? hilit : lit) );

			if( bg_hover ) {
				string bg_str = this.Foreground ? "Foreground Only" : "Background Only";
				sb.DrawString( Main.fontMouseText, bg_str, new Vector2(bg_rect.X, bg_rect.Y - 16), Color.White );
			}

			///

			Texture2D size_tex = this.BrushSize == 1 ? PaintBlasterItem.BrushSmallTex : PaintBlasterItem.BrushLargeTex;

			var size_offset = new Vector2( size_tex.Width, size_tex.Height ) * 0.5f;

			int size_x = (x + setting_dist) - (int)size_offset.X;
			int size_y = (y + setting_dist) - (int)size_offset.Y;
			var size_rect = new Rectangle( size_x, size_y, size_tex.Width, size_tex.Height );

			bool size_hover = size_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( size_tex, size_rect, Color.White * (size_hover ? hilit : lit) );

			if( size_hover ) {
				string size_str = this.BrushSize > 1 ? "Large brush" : "Small brush";
				sb.DrawString( Main.fontMouseText, size_str, new Vector2(size_rect.X, size_rect.Y + size_rect.Height), Color.White );
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


		public IDictionary<int, Rectangle> DrawColorPalette() {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			var rects = new Dictionary<int, Rectangle>();

			double angle_step = 360d / (double)item_idxs.Count;
			double angle = 0d;
			
			foreach( int idx in item_idxs ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * (Math.PI / 180d) ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * (Math.PI / 180d) ) );

				Item item = Main.LocalPlayer.inventory[ idx ];
				var myitem = (ColorCartridgeItem)item.modItem;

				rects[ idx ] = this.DrawPaintIcon( myitem.MyColor, myitem.TimesUsed, x, y, (idx == this.CurrentCartridgeInventoryIndex) );

				angle += angle_step;
			}

			return rects;
		}
		
		public Rectangle DrawPaintIcon( Color color, int uses, int x, int y, bool is_selected ) {
			var mymod = (BetterPaintMod)this.mod;
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			bool is_hover = rect.Contains( Main.mouseX, Main.mouseY );
			float color_mul = is_selected ? PaintBlasterItem.SelectedScale :
				( is_hover ? PaintBlasterItem.HoveredScale : PaintBlasterItem.IdleScale );
			
			Main.spriteBatch.Draw( cart_tex, rect, Color.White * color_mul );
			Main.spriteBatch.Draw( over_tex, rect, color * color_mul );

			if( is_hover ) {
				float percent = 1f - ((float)uses / (float)mymod.Config.PaintCartridgeCapacity);
				Color text_color = percent < 0.15f ? Color.Red : (
					percent < 0.35f ? Color.Yellow : (
						percent < 1.0f ? Color.White : Color.LimeGreen
					)
				) * PaintBlasterItem.HoveredScale;
				Color label_color = Color.White * PaintBlasterItem.HoveredScale;
				
				Main.spriteBatch.DrawString( Main.fontMouseText, "Capacity:", new Vector2(Main.mouseX, Main.mouseY-16), label_color );
				Main.spriteBatch.DrawString( Main.fontMouseText, (int)(percent * 100)+"%", new Vector2(Main.mouseX+72, Main.mouseY-16), text_color );

				string color_str = "R:"+color.R+" G:"+color.G+" B:"+color.B+" A:"+color.A;

				Main.spriteBatch.DrawString( Main.fontMouseText, "Color:", new Vector2( Main.mouseX, Main.mouseY + 8 ), label_color );
				Main.spriteBatch.DrawString( Main.fontMouseText, color_str, new Vector2( Main.mouseX+56, Main.mouseY + 8 ), color );
			}

			return rect;
		}


		////////////////

		private void CheckUIModeInteractions( ref Rectangle bg_rect, ref Rectangle size_rect, ref Rectangle brush_rect, ref Rectangle spray_rect, ref Rectangle bucket_rect, ref Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;

			if( bg_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.Foreground = !this.Foreground;
			} else
			if( size_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.BrushSize = this.BrushSize == 1 ? 6 : 1;
			} else
			if( this.CurrentMode != PaintMode.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Stream;
			} else
			if( this.CurrentMode != PaintMode.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Spray;
			} else
			if( this.CurrentMode != PaintMode.Flood && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Flood;
			} else
			if( this.CurrentMode != PaintMode.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Erase;
			}
		}

		private void CheckUIColorInteractions( IDictionary<int, Rectangle> palette_rects ) {
			int inv_idx = -1;
			
			foreach( var kv in palette_rects ) {
				if( kv.Value.Contains( Main.mouseX, Main.mouseY ) ) {
					inv_idx = kv.Key;
					break;
				}
			}

			if( inv_idx != -1 ) {
				this.CurrentCartridgeInventoryIndex = inv_idx;
			}
		}
	}
}
