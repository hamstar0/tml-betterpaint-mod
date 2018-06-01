using BetterPaint.Painting;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
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

			if( this.IsUsingUI ) {
				return false;
			}

			if( paint_item == null ) {
				if( this.CurrentBrush == PaintBrushType.Erase || this.IsCopying ) {
					return true;
				}
				return false;
			}

			Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;

			return this.CanPaintAt( (ColorCartridgeItem)paint_item.modItem, (ushort)tile_pos.X, (ushort)tile_pos.Y );
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Vector2 world_mouse_pos = UIHelpers.GetWorldMousePosition();
			int world_x = (int)world_mouse_pos.X;
			int world_y = (int)world_mouse_pos.Y;
			ushort tile_x = (ushort)( world_x / 16 );
			ushort tile_y = (ushort)( world_y / 16 );
			Color? dust_color = null;
			float uses = 0f;

			if( this.IsCopying ) {
				if( !this.AttemptCopyColorAt( player, tile_x, tile_y ) ) {
					Main.NewText( "No color found to copy from.", Color.Yellow );
				}

				this.IsCopying = false;
			} else {
				Item paint_item = this.GetCurrentPaintItem();

				if( paint_item != null ) {
					var cartridge = (ColorCartridgeItem)paint_item.modItem;

					if( this.HasMatchingPaintAt( cartridge.MyColor, tile_x, tile_y ) ) {
						dust_color = cartridge.MyColor;
					}
				}

				uses = this.ApplyBrushAt( world_x, world_y );
			}

			if( uses > 0 && dust_color != null ) {
				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, (Color)dust_color, 1f );
			}

			return false;
		}


		////////////////

		public bool CanPaintAt( ColorCartridgeItem cartridge, ushort tile_x, ushort tile_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( this.CurrentBrush != PaintBrushType.Erase && cartridge.PaintQuantity <= 0f ) {
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
				cartridge = (ColorCartridgeItem)paint_item.modItem;
				color = cartridge.MyColor;
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

			if( cartridge != null && uses > 0 ) {
				cartridge.ConsumePaint( uses );
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
