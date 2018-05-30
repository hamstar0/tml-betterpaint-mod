using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		private void CheckUISettingsInteractions( ref Rectangle bg_rect, ref Rectangle size_rect, ref Rectangle copy_rect ) {
			Player player = Main.LocalPlayer;

			if( bg_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CyclePaintMode();
			} else
			if( size_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.BrushSize = !this.BrushSize;
			} else
			if( copy_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.IsCopying = !this.IsCopying;
			}
		}


		private void CheckUIBrushInteractions( ref Rectangle brush_rect, ref Rectangle spray_rect, ref Rectangle bucket_rect, ref Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;
			
			if( this.CurrentBrush != PaintBrushType.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Stream;
			} else
			if( this.CurrentBrush != PaintBrushType.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spray;
			} else
			if( this.CurrentBrush != PaintBrushType.Spatter && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spatter;
			} else
			if( this.CurrentBrush != PaintBrushType.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Erase;
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



		public void CyclePaintMode() {
			switch( this.Layer ) {
			case PaintLayer.Foreground:
				this.Layer = PaintLayer.Background;
				break;
			case PaintLayer.Background:
				this.Layer = PaintLayer.Anyground;
				break;
			case PaintLayer.Anyground:
				this.Layer = PaintLayer.Foreground;
				break;
			}
		}
	}
}
