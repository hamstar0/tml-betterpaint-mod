using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		private bool CheckUISettingsInteractions( Rectangle layerRect, Rectangle sizeRect, Rectangle copyRect, Rectangle pressRect ) {
			Player player = Main.LocalPlayer;
			bool hasInteracted = false;

			if( layerRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleLayer();
				hasInteracted = true;
			} else
			if( sizeRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CycleBrushSize();
				hasInteracted = true;
			} else
			if( copyRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.IsCopying = !this.IsCopying;
				hasInteracted = true;
			} else
			if( pressRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CyclePressure();
				hasInteracted = true;
			}

			return hasInteracted;
		}


		private bool CheckUIBrushInteractions( Rectangle brushRect, Rectangle sprayRect, Rectangle bucketRect, Rectangle scrapeRect ) {
			Player player = Main.LocalPlayer;
			bool hasInteracted = false;

			if( this.CurrentBrush != PaintBrushType.Stream && brushRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Stream;
				hasInteracted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Spray && sprayRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spray;
				hasInteracted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Spatter && bucketRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Spatter;
				hasInteracted = true;
			} else
			if( this.CurrentBrush != PaintBrushType.Erase && scrapeRect.Contains( Main.mouseX, Main.mouseY ) ) {
				this.CurrentBrush = PaintBrushType.Erase;
				hasInteracted = true;
			}

			return hasInteracted;
		}


		private bool CheckUIColorInteractions( IDictionary<int, float> paletteAngles ) {
			int invIdx = -1;
			bool hasInteracted = false;
			
			foreach( var kv in paletteAngles ) {
				if( this.IsHoveringIcon( kv.Value, 360 / paletteAngles.Count ) ) {
					invIdx = kv.Key;
					break;
				}
			}
			
			if( invIdx != -1 ) {
				hasInteracted = true;
				this.CurrentPaintItemInventoryIndex = invIdx;
			}

			return hasInteracted;
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
