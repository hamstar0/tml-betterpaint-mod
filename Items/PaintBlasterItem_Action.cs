using BetterPaint.Painting;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.TileHelpers;
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

			if( this.CurrentBrush != PaintBrushType.Erase && PaintData.GetPaintAmount(item) <= 0 ) {
				return false;
			}

			Tile tile = Main.tile[tile_x, tile_y];

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				if( !TileHelpers.IsSolid( tile, true, true ) ) {
					return false;
				}
				break;
			case PaintLayer.Background:
				if( TileHelpers.IsAir( tile ) || TileHelpers.IsSolid( tile, true, true ) ) {
					return false;
				}
				break;
			case PaintLayer.Anyground:
				if( TileHelpers.IsAir( tile ) ) {
					return false;
				}
				break;
			default:
				throw new NotImplementedException();
			}

			return true;
		}


		public bool HasMatchingPaintAt( Color color, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				if( myworld.Layers.Foreground.GetColor( tile_x, tile_y ) == color ) {
					return true;
				}
				break;
			case PaintLayer.Background:
				if( myworld.Layers.Background.GetColor( tile_x, tile_y ) == color ) {
					return true;
				}
				break;
			case PaintLayer.Anyground:
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
			Color color;

			Item paint_item = this.GetCurrentPaintItem();
			ColorCartridgeItem cartridge = null;

			if( paint_item != null ) {
				color = PaintData.GetPaintColor( paint_item );

				if( paint_item.modItem is ColorCartridgeItem ) {
					cartridge = (ColorCartridgeItem)paint_item.modItem;
				}
			} else {
				color = Color.White;
			}

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				uses = myworld.Layers.ApplyForegroundColor( mymod, this.CurrentBrush, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			case PaintLayer.Background:
				uses = myworld.Layers.ApplyBackgroundColor( mymod, this.CurrentBrush, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			case PaintLayer.Anyground:
				uses = myworld.Layers.ApplyAnygroundColor( mymod, this.CurrentBrush, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			default:
				throw new NotImplementedException();
			}

			if( paint_item != null && uses > 0 ) {
				PaintData.ConsumePaint( paint_item, uses );
			}

			return uses;
		}

		
		public bool AttemptCopyColorAt( Player player, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			PaintData data;

			int copy_type = this.mod.ItemType<CopyCartridgeItem>();
			int copy_item_idx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, new HashSet<int> { copy_type } );
			if( copy_item_idx == -1 ) {
				return false;
			}

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				data = myworld.Layers.Foreground;
				break;
			case PaintLayer.Background:
				data = myworld.Layers.Background;
				break;
			case PaintLayer.Anyground:
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
