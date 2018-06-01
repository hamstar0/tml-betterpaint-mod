using BetterPaint.Painting;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.TileHelpers;
using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public override bool CanUseItem( Player player ) {
			Item paint_item = this.GetCurrentPaintItem();
			Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;

			return !this.IsModeSelecting &&
				paint_item != null &&
				this.CanPaintAt( (ColorCartridgeItem)paint_item.modItem, (ushort)tile_pos.X, (ushort)tile_pos.Y );
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Vector2 tile_pos = UIHelpers.GetWorldMousePosition();
			int world_x = (int)tile_pos.X;
			int world_y = (int)tile_pos.Y;
			ushort tile_x = (ushort)( world_x / 16 );
			ushort tile_y = (ushort)( world_y / 16 );
			Color? dust_color = null;
			float uses = 0f;

			if( this.IsCopying ) {
				int copy_type = this.mod.ItemType<CopyCartridgeItem>();
				int item_idx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, new HashSet<int> { copy_type } );

				if( item_idx != -1 ) {
					this.CopyColorAt( player, item_idx, tile_x, tile_y );
				}
			} else {
				Item paint_item = this.GetCurrentPaintItem();

				if( paint_item != null ) {
					var cartridge = (ColorCartridgeItem)paint_item.modItem;

					if( this.HasMatchingPaintAt( cartridge.MyColor, tile_x, tile_y ) ) {
						dust_color = cartridge.MyColor;
					}
				}

				uses = this.ApplyAt( world_x, world_y );
			}

			if( uses > 0 && dust_color != null ) {
				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, (Color)dust_color, 1f );
			}

			return false;
		}


		////////////////

		public void CheckMenu() {
			if( Main.mouseRight ) {
				this.IsModeSelecting = true;
			} else if( this.IsModeSelecting ) {
				this.IsModeSelecting = false;
			}
		}

		////////////////

		public bool CanPaintAt( ColorCartridgeItem cartridge, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( cartridge.RemainingCapacity() <= 0f ) {
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

		public float ApplyAt( int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			float uses = 0;
			Color color;

			Item paint_item = this.GetCurrentPaintItem();
			ColorCartridgeItem cartridge = null;

			if( paint_item != null ) {
				cartridge = (ColorCartridgeItem)paint_item.modItem;
				color = cartridge.MyColor;
			} else {
				color = Color.White;
			}

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				uses = myworld.Layers.ApplyForegroundColor( mymod, this.CurrentMode, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			case PaintLayer.Background:
				uses = myworld.Layers.ApplyBackgroundColor( mymod, this.CurrentMode, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			case PaintLayer.Anyground:
				uses = myworld.Layers.ApplyAnygroundColor( mymod, this.CurrentMode, color, this.BrushSize, this.PressurePercent, world_x, world_y );
				break;
			default:
				throw new NotImplementedException();
			}

			if( cartridge != null && uses > 0 ) {
				float total_uses = cartridge.TimesUsed + uses;

				cartridge.SetTimesUsed( Math.Min( total_uses, (float)mymod.Config.PaintCartridgeCapacity ) );
			}

			return uses;
		}


		public bool CopyColorAt( Player player, int copy_cart_inv_idx, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)(world_x / 16);
			ushort tile_y = (ushort)(world_y / 16);
			PaintData data;

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

			CopyCartridgeItem.SetWithColor( player, copy_cart_inv_idx, color_at );

			return true;
		}
	}
}
