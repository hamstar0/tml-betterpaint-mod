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
				this.CanPaintAt( (ColorCartridgeItem)paint_item.modItem, (int)tile_pos.X, (int)tile_pos.Y );
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Item paint_item = this.GetCurrentPaintItem();
			if( paint_item == null ) { return false; }

			var myitem = (ColorCartridgeItem)paint_item.modItem;
			
			Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;
			int x = (int)tile_pos.X;
			int y = (int)tile_pos.Y;

			this.PaintAt( myitem, x, y );

			Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, myitem.MyColor, 1f );

			return false;
		}


		public void CheckUse() {
			if( Main.mouseRight ) {
				this.IsModeSelecting = true;
			} else if( this.IsModeSelecting ) {
				this.IsModeSelecting = false;
			}
			/*if( Main.mouseLeft ) {
				if( this.CanUseItem( Main.LocalPlayer ) ) {
					Item paint_item = this.GetCurrentPaintItem();

					if( paint_item != null ) {
						var myitem = (ColorCartridgeItem)paint_item.modItem;

						Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;
						int x = (int)tile_pos.X;
						int y = (int)tile_pos.Y;

						this.TryPaintAt( myitem, x, y );
					}
				}
			}*/
		}


		public bool CanPaintAt( ColorCartridgeItem cartridge, int x, int y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			if( cartridge.TimesUsed >= mymod.Config.PaintCartridgeCapacity ) {
				return false;
			}

			if( myworld.HasFgColor(cartridge.MyColor, x, y) ) {
				return false;
			}

			return true;
		}


		public void PaintAt( ColorCartridgeItem color_cartridge, int x, int y ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();
			int uses = 0;

			if( this.Foreground ) {
				uses = myworld.AddForegroundColor( color_cartridge.MyColor, this.BrushSize, this.CurrentMode, (ushort)x, (ushort)y );
			} else {
				uses = myworld.AddBackgroundColor( color_cartridge.MyColor, this.BrushSize, this.CurrentMode, (ushort)x, (ushort)y );
			}

			int total_uses = color_cartridge.TimesUsed + uses;

			color_cartridge.SetTimesUsed( Math.Min(total_uses, mymod.Config.PaintCartridgeCapacity) );
		}
	}
}
