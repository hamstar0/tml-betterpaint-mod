using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public bool CanPaintAt( Item paint_item, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( this.CurrentBrush != PaintBrushType.Erase && PaintHelpers.GetPaintAmount(paint_item) <= 0 ) {
				return false;
			}

			Tile tile = Main.tile[tile_x, tile_y];
			
			switch( this.Layer ) {
			case PaintLayerType.Foreground:
				return myworld.Layers.Foreground.CanPaintAt( tile );
			case PaintLayerType.Background:
				return myworld.Layers.Background.CanPaintAt( tile );
			case PaintLayerType.Anyground:
				return myworld.Layers.Foreground.CanPaintAt( tile ) || myworld.Layers.Background.CanPaintAt( tile );
			default:
				throw new NotImplementedException();
			}
		}


		public bool HasMatchingPaintAt( Color color, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			switch( this.Layer ) {
			case PaintLayerType.Foreground:
				if( myworld.Layers.Foreground.GetColor( tile_x, tile_y ) == color ) {
					return true;
				}
				break;
			case PaintLayerType.Background:
				if( myworld.Layers.Background.GetColor( tile_x, tile_y ) == color ) {
					return true;
				}
				break;
			case PaintLayerType.Anyground:
				if( myworld.Layers.Foreground.HasColor(tile_x, tile_y) ) {
					if( myworld.Layers.Foreground.GetColor( tile_x, tile_y ) == color ) {
						return true;
					}
				} else if( myworld.Layers.Background.HasColor( tile_x, tile_y ) ) {
					if( myworld.Layers.Background.GetColor( tile_x, tile_y ) == color ) {
						return true;
					}
				}
				break;
			default:
				throw new NotImplementedException();
			}

			return false;
		}


		////////////////

		public float ApplyBrushAt( int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			float uses = 0;
			Color color = PaintHelpers.UnlitBaseColor;

			Item paint_item = this.GetCurrentPaintItem();

			if( paint_item != null ) {	// Eraser doesn't need paint
				color = PaintHelpers.GetPaintColor( paint_item );
			}
			
			uses = myworld.Layers.ApplyColorAt( mymod, this.Layer, this.CurrentBrush, color, this.BrushSize, this.PressurePercent, world_x, world_y );
			
			if( paint_item != null && uses > 0 ) {
				PaintHelpers.ConsumePaint( paint_item, uses );
			}

			return uses;
		}

		
		public bool AttemptCopyColorAt( Player player, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			PaintLayer data;

			int copy_type = this.mod.ItemType<CopyCartridgeItem>();
			int copy_item_idx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, new HashSet<int> { copy_type } );
			if( copy_item_idx == -1 ) {
				return false;
			}

			switch( this.Layer ) {
			case PaintLayerType.Foreground:
				data = myworld.Layers.Foreground;
				break;
			case PaintLayerType.Background:
				data = myworld.Layers.Background;
				break;
			case PaintLayerType.Anyground:
				if( myworld.Layers.Foreground.HasColor( tile_x, tile_y ) ) {
					data = myworld.Layers.Foreground;
				} else {
					data = myworld.Layers.Background;
				}
				break;
			default:
				throw new NotImplementedException();
			}

			if( !data.HasColor(tile_x, tile_y) ) {
				return false;
			}

			Color color_at = data.GetColor( tile_x, tile_y );
			color_at.A = 255;

			CopyCartridgeItem.SetWithColor( player, player.inventory[copy_item_idx], color_at );

			return true;
		}
	}
}
