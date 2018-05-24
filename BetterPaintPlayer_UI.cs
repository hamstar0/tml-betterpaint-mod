using BetterPaint.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint {
	enum PaintMode : int {
		Stream,
		Spray,
		Flood,
		Erase
	}



	partial class BetterPaintPlayer : ModPlayer {
		private IDictionary<int, Color> GetPaints() {
			IDictionary<int, Color> colors = new Dictionary<int, Color>();
			Item[] inv = this.player.inventory;

			for( int i=0; i< inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir || item.type != this.mod.ItemType<ColorCartridgeItem>() ) { continue; }

				var data = item.GetGlobalItem<ColorCartridgeItemData>();
				colors[ data.Uses ] = data.MyColor;
			}

			return colors;
		}

		////////////////

		public void DrawPainterUI( SpriteBatch sb ) {
			this.DrawColorPalette();

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
				Main.NewText( "Paint mode: Stream", Color.Aquamarine );
			} else
			if( this.CurrentMode != PaintMode.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Spray;
				Main.NewText( "Paint mode: Spray", Color.Aquamarine );
			} else
			if( this.CurrentMode != PaintMode.Flood && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Flood;
				Main.NewText( "Paint mode: Flood", Color.Aquamarine );
			} else
			if( this.CurrentMode != PaintMode.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintMode.Erase;
				Main.NewText( "Paint mode: Erase", Color.Aquamarine );
			}
		}


		public void DrawColorPalette() {
			IDictionary<int, Color> icons = this.GetPaints();

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

			Main.spriteBatch.Draw( Main.magicPixel, new Rectangle( x, y, 16, 16 ), color );
		}
	}
}
