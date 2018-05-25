using BetterPaint.Items;
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



	partial class ColorCartridgeItemData : GlobalItem {
		private IDictionary<int, Color> GetPaints( Player player ) {
			IDictionary<int, Color> colors = new Dictionary<int, Color>();
			Item[] inv = player.inventory;

			for( int i=0; i< inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir || item.type != this.mod.ItemType<ColorCartridgeItem>() ) { continue; }
				
				colors[ this.Uses ] = this.MyColor;
			}

			return colors;
		}

		////////////////

		public void DrawPainterUI( SpriteBatch sb ) {
			this.DrawColorPalette();

			Player plr = Main.LocalPlayer;
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
			
			if( this.CurrentMode != PaintMode.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Stream;
				PlayerMessages.AddPlayerLabel( plr, "Paint mode: Stream", Color.Aquamarine, 60, true );
			} else
			if( this.CurrentMode != PaintMode.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Spray;
				PlayerMessages.AddPlayerLabel( plr, "Paint mode: Spray", Color.Aquamarine, 60, true );
			} else
			if( this.CurrentMode != PaintMode.Flood && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Flood;
				PlayerMessages.AddPlayerLabel( plr, "Paint mode: Flood", Color.Aquamarine, 60, true );
			} else
			if( this.CurrentMode != PaintMode.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Erase;
				PlayerMessages.AddPlayerLabel( plr, "Paint mode: Erase", Color.Aquamarine, 60, true );
			}
		}


		public void DrawColorPalette() {
			IDictionary<int, Color> icons = this.GetPaints( Main.LocalPlayer );

			float angle_step = 360f / (float)icons.Count;
			float angle = 0f;

			foreach( var kv in icons ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle ) );

				this.DrawPaintIcon( kv.Value, kv.Key, x, y );

				angle += angle_step;
			}
		}


		public void DrawPaintIcon( Color color, int uses, int x, int y ) {
			float fill = (float)uses / 100f;
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			Main.spriteBatch.Draw( cart_tex, new Rectangle( x, y, cart_tex.Width, cart_tex.Height ), Color.White );
			Main.spriteBatch.Draw( over_tex, new Rectangle( x, y, over_tex.Width, over_tex.Height ), color );
		}
	}
}
