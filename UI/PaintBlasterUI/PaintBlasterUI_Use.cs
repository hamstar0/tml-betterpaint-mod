using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		private bool CheckUISettingsInteractions( Rectangle layer_rect, Rectangle size_rect, Rectangle copy_rect, Rectangle press_rect ) {
			Player player = Main.LocalPlayer;
			bool has_interacted = false;

			if( layer_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleLayer();
				has_interacted = true;
			} else
			if( size_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleBrushSize();
				has_interacted = true;
			} else
			if( copy_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.IsCopying = !this.IsCopying;
				has_interacted = true;
			} else
			if( press_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CyclePressure();
				has_interacted = true;
			}

			return has_interacted;
		}


		private bool CheckUIBrushInteractions( Rectangle brush_rect, Rectangle spray_rect, Rectangle bucket_rect, Rectangle scrape_rect ) {
			Player player = Main.LocalPlayer;
			bool has_interacted = false;

			if( this.CurrentBrush != PaintBrushType.Stream && brush_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Stream;
				has_interacted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Spray && spray_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spray;
				has_interacted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Spatter && bucket_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spatter;
				has_interacted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Erase && scrape_rect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Erase;
				has_interacted = true;
			}

			return has_interacted;
		}


		private bool CheckUIColorInteractions( IDictionary<int, float> palette_angles ) {
			int inv_idx = -1;
			bool has_interacted = false;
			
			foreach( var kv in palette_angles ) {
				if( this.IsHoveringIcon( kv.Value, 360 / palette_angles.Count ) ) {
					inv_idx = kv.Key;
					break;
				}
			}
			
			if( inv_idx != -1 ) {
				has_interacted = true;
				this.CurrentPaintItemInventoryIndex = inv_idx;
			}

			return has_interacted;
		}


		////////////////

		public void CycleLayer() {
			switch( this.Layer ) {
			case PaintLayerType.Foreground:
				this.Layer = PaintLayerType.Background;
				break;
			case PaintLayerType.Background:
				this.Layer = PaintLayerType.Anyground;
				break;
			case PaintLayerType.Anyground:
				this.Layer = PaintLayerType.Foreground;
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
