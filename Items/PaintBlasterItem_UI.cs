using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	enum PaintMode : int {
		Stream,
		Spray,
		Flood,
		Erase
	}



	partial class PaintBlasterItem : ModItem {
		public void DrawPainterUI( SpriteBatch sb ) {
			IDictionary<int, Rectangle> palette_rects = this.DrawColorPalette();
			
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			Texture2D tex_brush = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_bucket = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_scrape = Main.itemTexture[ItemID.PaintScraper];

			var brush_off = new Vector2( tex_brush.Width, tex_brush.Height ) * 0.5f;
			var spray_off = new Vector2( tex_spray.Width, tex_spray.Height ) * 0.5f;
			var bucket_off = new Vector2( tex_bucket.Width, tex_bucket.Height ) * 0.5f;
			var scrape_off = new Vector2( tex_scrape.Width, tex_scrape.Height ) * 0.5f;

			var brush_rect = new Rectangle( (int)(brush_off.X + (x - 64)), (int)(brush_off.Y + y), tex_brush.Width, tex_brush.Height );
			var spray_rect = new Rectangle( (int)(brush_off.X + x), (int)(brush_off.Y + (y - 64)), tex_spray.Width, tex_spray.Height );
			var bucket_rect = new Rectangle( (int)(brush_off.X + (x + 64)), (int)(brush_off.Y + y), tex_bucket.Width, tex_bucket.Height );
			var scrape_rect = new Rectangle( (int)(scrape_off.X + x), (int)(brush_off.Y + (y + 64)), tex_scrape.Width, tex_scrape.Height );

			sb.Draw( tex_brush, brush_rect, Color.White * (this.CurrentMode == PaintMode.Stream ? 0.5f : 0.2f) );
			sb.Draw( tex_spray, spray_rect, Color.White * (this.CurrentMode == PaintMode.Spray ? 0.5f : 0.2f) );
			sb.Draw( tex_bucket, bucket_rect, Color.White * (this.CurrentMode == PaintMode.Flood ? 0.5f : 0.2f) );
			sb.Draw( tex_scrape, scrape_rect, Color.White * (this.CurrentMode == PaintMode.Erase ? 0.5f : 0.2f) );

			this.CheckUIModeInteractions( ref brush_rect, ref spray_rect, ref bucket_rect, ref scrape_rect );
			this.CheckUIColorInteractions( palette_rects );
		}


		public IDictionary<int, Rectangle> DrawColorPalette() {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			IDictionary<int, Rectangle> rects = new Dictionary<int, Rectangle>();

			float angle_step = 360f / (float)item_idxs.Count;
			float angle = 0f;
			
			foreach( int idx in item_idxs ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle ) );

				Item item = Main.LocalPlayer.inventory[ idx ];
				var myitem = (ColorCartridgeItem)item.modItem;

				rects[ idx ] = this.DrawPaintIcon( myitem.MyColor, myitem.TimesUsed, x, y, idx == this.CurrentCartridgeInventoryIndex );

				angle += angle_step;
			}

			return rects;
		}
		
		public Rectangle DrawPaintIcon( Color color, int uses, int x, int y, bool is_highlighted ) {
			float fill = (float)uses / 100f;
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			Rectangle rect = new Rectangle( x, y, cart_tex.Width, cart_tex.Height );

			Main.spriteBatch.Draw( cart_tex, rect, Color.White * (is_highlighted ? 1f : 0.5f) );
			Main.spriteBatch.Draw( over_tex, rect, color * (is_highlighted ? 1f : 0.5f) );

			return rect;
		}


		////////////////

		private void CheckUIModeInteractions( ref Rectangle brush_rect, ref Rectangle spray_rect, ref Rectangle bucket_rect, ref Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;

			if( Main.mouseLeft ) {
				if( this.CurrentMode != PaintMode.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
					this.CurrentMode = PaintMode.Stream;
					PlayerMessages.AddPlayerLabel( player, "Paint mode: Stream", Color.Aquamarine, 60, true );
				} else
				if( this.CurrentMode != PaintMode.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
					this.CurrentMode = PaintMode.Spray;
					PlayerMessages.AddPlayerLabel( player, "Paint mode: Spray", Color.Aquamarine, 60, true );
				} else
				if( this.CurrentMode != PaintMode.Flood && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
					this.CurrentMode = PaintMode.Flood;
					PlayerMessages.AddPlayerLabel( player, "Paint mode: Flood", Color.Aquamarine, 60, true );
				} else
				if( this.CurrentMode != PaintMode.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
					this.CurrentMode = PaintMode.Erase;
					PlayerMessages.AddPlayerLabel( player, "Paint mode: Erase", Color.Aquamarine, 60, true );
				}
			}
		}

		private void CheckUIColorInteractions( IDictionary<int, Rectangle> palette_rects ) {
			int inv_idx = -1;

			if( Main.mouseLeft ) {
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
}
