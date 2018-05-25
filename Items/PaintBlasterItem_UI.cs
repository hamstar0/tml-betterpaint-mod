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
		private IDictionary<int, Color> GetPaints( Player player ) {
			IDictionary<int, Color> colors = new Dictionary<int, Color>();
			Item[] inv = player.inventory;
			int cartridge_type = this.mod.ItemType<ColorCartridgeItem>();

			for( int i=0; i< inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir || item.type != cartridge_type ) { continue; }

				var myitem = (ColorCartridgeItem)item.modItem;
				
				colors[ myitem.TimesUsed ] = myitem.MyColor;
			}

			return colors;
		}

		////////////////

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

			sb.Draw( tex_brush, brush_rect, Color.White * (this.CurrentMode == PaintMode.Stream ? 0.1f : 0.5f) );
			sb.Draw( tex_spray, spray_rect, Color.White * (this.CurrentMode == PaintMode.Spray ? 0.1f : 0.5f) );
			sb.Draw( tex_bucket, bucket_rect, Color.White * (this.CurrentMode == PaintMode.Flood ? 0.1f : 0.5f) );
			sb.Draw( tex_scrape, scrape_rect, Color.White * (this.CurrentMode == PaintMode.Erase ? 0.1f : 0.5f) );

			this.CheckUIModeInteractions( ref brush_rect, ref spray_rect, ref bucket_rect, ref scrape_rect );
			this.CheckUIColorInteractions( palette_rects );
		}


		public IDictionary<int, Rectangle> DrawColorPalette() {
			IDictionary<int, Color> icons = this.GetPaints( Main.LocalPlayer );
			IDictionary<int, Rectangle> rects = new Dictionary<int, Rectangle>();

			float angle_step = 360f / (float)icons.Count;
			float angle = 0f;
			
			foreach( var kv in icons ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle ) );

				rects[ kv.Key ] = this.DrawPaintIcon( kv.Value, kv.Key, x, y );

				angle += angle_step;
			}

			return rects;
		}
		
		public Rectangle DrawPaintIcon( Color color, int uses, int x, int y ) {
			float fill = (float)uses / 100f;
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			Rectangle rect = new Rectangle( x, y, cart_tex.Width, cart_tex.Height );

			Main.spriteBatch.Draw( cart_tex, rect, Color.White );
			Main.spriteBatch.Draw( over_tex, rect, color );

			return rect;
		}


		////////////////

		private void CheckUIModeInteractions( ref Rectangle brush_rect, ref Rectangle spray_rect, ref Rectangle bucket_rect, ref Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;

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

		private void CheckUIColorInteractions( IDictionary<int, Rectangle> palette_rects ) {
			int inv_idx = -1;

			foreach( var kv in palette_rects ) {
				if( kv.Value.Contains(Main.mouseX, Main.mouseY) ) {
					inv_idx = kv.Key;
					break;
				}
			}

			if( inv_idx != -1 ) {

			}
		}
	}
}
