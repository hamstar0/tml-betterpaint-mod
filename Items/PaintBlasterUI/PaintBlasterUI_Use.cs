using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		private void CheckUIModeInteractions( ref Rectangle bg_rect, ref Rectangle size_rect, ref Rectangle brush_rect, ref Rectangle spray_rect, ref Rectangle bucket_rect, ref Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;

			if( bg_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.Foreground = !this.Foreground;
			} else
			if( size_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.BrushSize = this.BrushSize == 1 ? BetterPaintMod.Instance.Config.BrushSizeLarge : BetterPaintMod.Instance.Config.BrushSizeSmall;
			} else
			if( this.CurrentMode != PaintModeType.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintModeType.Stream;
			} else
			if( this.CurrentMode != PaintModeType.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintModeType.Spray;
			} else
			if( this.CurrentMode != PaintModeType.Spatter && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintModeType.Spatter;
			} else
			if( this.CurrentMode != PaintModeType.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentMode = PaintModeType.Erase;
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
