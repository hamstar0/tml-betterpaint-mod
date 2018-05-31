using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		private void CheckUISettingsInteractions( Rectangle layer_rect, Rectangle size_rect, Rectangle copy_rect, Rectangle press_rect ) {
			Player player = Main.LocalPlayer;

			/*layer_rect.X -= 8;
			size_rect.X -= 8;
			copy_rect.X -= 8;
			press_rect.X -= 8;
			layer_rect.Y -= 8;
			size_rect.Y -= 8;
			copy_rect.Y -= 8;
			press_rect.Y -= 8;
			layer_rect.Width += 16;
			size_rect.Width += 16;
			copy_rect.Width += 16;
			press_rect.Width += 16;
			layer_rect.Height += 16;
			size_rect.Height += 16;
			copy_rect.Height += 16;
			press_rect.Height += 16;*/

			if( layer_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleLayer();
			} else
			if( size_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleBrushSize();
			} else
			if( copy_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.IsCopying = !this.IsCopying;
			} else
			if( press_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CyclePressure();
			}
		}


		private void CheckUIBrushInteractions( Rectangle brush_rect, Rectangle spray_rect, Rectangle bucket_rect, Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;

			/*brush_rect.X -= 8;
			spray_rect.X -= 8;
			bucket_rect.X -= 8;
			scrape_rect.X -= 8;
			brush_rect.Y -= 8;
			spray_rect.Y -= 8;
			bucket_rect.Y -= 8;
			scrape_rect.Y -= 8;
			brush_rect.Width += 16;
			spray_rect.Width += 16;
			bucket_rect.Width += 16;
			scrape_rect.Width += 16;
			brush_rect.Height += 16;
			spray_rect.Height += 16;
			bucket_rect.Height += 16;
			scrape_rect.Height += 16;*/

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


		private void CheckUIColorInteractions( IDictionary<int, float> palette_angles ) {
			int inv_idx = -1;
			
			foreach( var kv in palette_angles ) {
				if( this.IsHoveringIcon( kv.Value, 360 / palette_angles.Count ) ) {
					inv_idx = kv.Key;
					break;
				}
			}

			if( inv_idx != -1 ) {
				this.CurrentCartridgeInventoryIndex = inv_idx;
			}
		}


		////////////////

		public void CycleLayer() {
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
			default:
				throw new NotImplementedException();
			}
		}

		public void CycleBrushSize() {
			switch( this.BrushSize ) {
			case PaintBrushSize.Small:
				this.BrushSize = PaintBrushSize.Large;
				break;
			case PaintBrushSize.Large:
				this.BrushSize = PaintBrushSize.Small;
				break;
			default:
				throw new NotImplementedException();
			}
		}

		public void CyclePressure() {
			if( this.PressurePercent >= 0.75f ) {
				this.PressurePercent = 0.25f;
			} else if( this.PressurePercent <= 0.25f ) {
				this.PressurePercent = 0.625f;
			} else {
				this.PressurePercent = 1f;
			}
		}
	}
}
