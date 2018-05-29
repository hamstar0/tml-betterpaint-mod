using HamstarHelpers.TileHelpers;
using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System;
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
			Item paint_item = this.GetCurrentPaintItem();
			if( paint_item == null ) { return false; }

			var cartridge = (ColorCartridgeItem)paint_item.modItem;
			
			Vector2 tile_pos = UIHelpers.GetWorldMousePosition();
			int world_x = (int)tile_pos.X;
			int world_y = (int)tile_pos.Y;

			if( !this.HasMatchingPaintAt( cartridge, (ushort)(world_x/16), (ushort)(world_y/16) ) ) {
				this.PaintAt( cartridge, world_x, world_y );

				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, cartridge.MyColor, 1f );
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

		public bool CanPaintAt( ColorCartridgeItem cartridge, ushort x, ushort y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( cartridge.TimesUsed >= mymod.Config.PaintCartridgeCapacity ) {
				return false;
			}

			Tile tile = Main.tile[x, y];

			if( this.Foreground ) {
				if( !TileHelpers.IsSolid( tile, true, true) ) {
					return false;
				}
			} else {
				if( TileHelpers.IsSolid( tile, true, true ) ) {
					return false;
				}
				if( tile == null || tile.wall == 0 ) {
					return false;
				}
			}

			return true;
		}


		public bool HasMatchingPaintAt( ColorCartridgeItem cartridge, ushort x, ushort y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( this.Foreground ) {
				if( myworld.FgColors.GetColor( x, y ) == cartridge.MyColor ) {
					return true;
				}
			} else {
				if( myworld.BgColors.GetColor( x, y ) == cartridge.MyColor ) {
					return true;
				}
			}

			return false;
		}


		////////////////

		public void PaintAt( ColorCartridgeItem color_cartridge, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			float uses = 0;

			if( this.Foreground ) {
				uses = myworld.AddForegroundColor( color_cartridge.MyColor, this.BrushSize, this.CurrentMode, world_x, world_y );
			} else {
				uses = myworld.AddBackgroundColor( color_cartridge.MyColor, this.BrushSize, this.CurrentMode, world_x, world_y );
			}

			float total_uses = color_cartridge.TimesUsed + uses;

			color_cartridge.SetTimesUsed( Math.Min(total_uses, (float)mymod.Config.PaintCartridgeCapacity) );
		}
	}
}
