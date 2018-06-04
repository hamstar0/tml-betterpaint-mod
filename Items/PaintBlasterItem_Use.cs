using BetterPaint.Painting;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.UIHelpers;
using HamstarHelpers.Utilities.Timers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		private bool DryFireAvailable = true;



		public override bool CanUseItem( Player player ) {
			Item paint_item = this.GetCurrentPaintItem();

			if( this.IsUsingUI ) {
				return false;
			}

			if( paint_item == null ) {
				if( this.CurrentBrush == PaintBrushType.Erase || this.IsCopying ) {
					return true;
				} else {
					if( this.DryFireAvailable ) {
						this.DryFireAvailable = false;

						Timers.SetTimer( "BetterPaintDryFire", 1 * 60, () => {
							this.DryFireAvailable = true;
							return false;
						} );

						Main.NewText( "Select a paint first.", Color.Yellow );
					}
				}
				return false;
			}

			Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;

			return this.CanPaintAt( paint_item, (ushort)tile_pos.X, (ushort)tile_pos.Y );
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Vector2 world_mouse_pos = new Vector2( Main.mouseX, Main.mouseY ) + Main.screenPosition;
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
					if( PaintData.GetPaintAmount(paint_item) <= 0 ) {
						if( this.SwitchToNextMatchingNonemptyPaint() ) {
							paint_item = this.GetCurrentPaintItem();
						} else {
							paint_item = null;
						}
					}

					if( paint_item != null ) {
						Color paint_color = PaintData.GetPaintColor( paint_item );

						if( this.HasMatchingPaintAt( paint_color, tile_x, tile_y ) ) {
							dust_color = paint_color;
						}
					}
				}

				uses = this.ApplyBrushAt( world_x, world_y );
			}

			if( uses > 0 && dust_color != null ) {
				pos = PlayerItemHelpers.TipOfHeldItem( player ) - Main.screenPosition;
				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, (Color)dust_color, 1f );
			}

			return false;
		}


		////////////////

		public bool SwitchToNextMatchingNonemptyPaint() {
			Item paint_item = this.GetCurrentPaintItem();
			if( paint_item == null ) { return false; }

			Color curr_color = PaintData.GetPaintColor( paint_item );
			
			Item[] inv = Main.LocalPlayer.inventory;
			int cart_type = this.mod.ItemType<ColorCartridgeItem>();
			bool found = false;

			for( int i=0; i<inv.Length; i++ ) {
				if( i == this.CurrentPaintItemInventoryIndex ) { continue; }

				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( PaintData.IsPaint(item) ) {
					if( PaintData.GetPaintAmount(item) <= 0 ) { continue; }
					if( PaintData.GetPaintColor(item) != curr_color ) { continue; }

					this.CurrentPaintItemInventoryIndex = i;
					found = true;

					break;
				}
			}

			return found;
		}
	}
}
