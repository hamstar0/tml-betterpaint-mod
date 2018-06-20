using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		private bool DryFireAvailable = true;
		private float BufferedPaintUses = 0f;
		private IDictionary<ushort, ushort> BufferedPaintedTiles = new Dictionary<ushort, ushort>();


		////////////////

		public override bool CanUseItem( Player player ) {
			if( this.IsUsingUI ) {
				return false;
			}
			if( this.CurrentBrush == PaintBrushType.Erase || this.IsCopying ) {
				return true;
			}

			Item paint_item = this.GetCurrentPaintItem();

			if( paint_item == null ) {
				if( this.DryFireAvailable ) {
					this.DryFireAvailable = false;

					Timers.SetTimer( "BetterPaintDryFire", 1 * 60, () => {
						this.DryFireAvailable = true;
						return false;
					} );

					Main.NewText( "Select a paint first.", Color.Yellow );
				}
				return false;
			}

			Vector2 world_pos = UIHelpers.GetWorldMousePosition();
			Vector2 tile_pos = world_pos / 16f;
			ushort tile_x = (ushort)tile_pos.X;
			ushort tile_y = (ushort)tile_pos.Y;

			bool can_paint_at = this.CanPaintAt( paint_item, tile_x, tile_y );
			if( can_paint_at ) {
				if( this.CanUseBlasterAt( tile_x, tile_y ) ) {
					this.BufferedPaintUses += this.BlastPaintAt( world_pos );
				}
			}

			return can_paint_at;
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Vector2 world_mouse_pos = new Vector2( Main.mouseX, Main.mouseY ) + Main.screenPosition;
			int world_x = (int)world_mouse_pos.X;
			int world_y = (int)world_mouse_pos.Y;
			ushort tile_x = (ushort)( world_x / 16 );
			ushort tile_y = (ushort)( world_y / 16 );
			Color? dust_color = null;
			
			if( this.IsCopying ) {
				if( !this.AttemptCopyColorAt( player, tile_x, tile_y ) ) {
					Main.NewText( "No color found to copy from.", Color.Yellow );
				}

				this.IsCopying = false;
			} else {
				Item paint_item = this.GetCurrentPaintItem();

				if( paint_item != null ) {
					if( PaintHelpers.GetPaintAmount(paint_item) <= 0 ) {
						if( this.SwitchToNextMatchingNonemptyPaint() ) {
							paint_item = this.GetCurrentPaintItem();
						} else {
							paint_item = null;
						}
					}

					if( paint_item != null ) {
						Color paint_color = PaintHelpers.GetPaintColor( paint_item );

						if( this.HasMatchingPaintAt( paint_color, tile_x, tile_y ) ) {
							dust_color = PaintHelpers.FullColor( paint_color );
						}
					}
				}
			}

			if( this.BufferedPaintUses > 0 && dust_color != null ) {
				this.BufferedPaintUses = 0f;

				pos = PlayerItemHelpers.TipOfHeldItem( player ) - Main.screenPosition;
				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, (Color)dust_color, 1f );
			}
			
			return false;
		}


		////////////////

		public bool CanUseBlasterAt( ushort tile_x, ushort tile_y ) {
			if( this.BufferedPaintedTiles.ContainsKey( tile_x ) ) {
				if( this.BufferedPaintedTiles[tile_x] == tile_y ) {
					return false;
				}
			}
			this.BufferedPaintedTiles[tile_x] = tile_y;

			return true;
		}

		public float BlastPaintAt( Vector2 world_pos ) {
			if( Timers.GetTimerTickDuration( "PaintBlasterFlowReset" ) <= 0 ) {
				Timers.SetTimer( "PaintBlasterFlowReset", 30, () => {
					this.BufferedPaintedTiles.Clear();
					return false;
				} );
			}
			
			return this.ApplyBrushAt( (int)world_pos.X, (int)world_pos.Y );
		}
		

		////////////////

		public bool SwitchToNextMatchingNonemptyPaint() {
			Item paint_item = this.GetCurrentPaintItem();
			if( paint_item == null ) { return false; }

			Color curr_color = PaintHelpers.GetPaintColor( paint_item );
			
			Item[] inv = Main.LocalPlayer.inventory;
			int cart_type = this.mod.ItemType<ColorCartridgeItem>();
			bool found = false;

			for( int i=0; i<inv.Length; i++ ) {
				if( i == this.CurrentPaintItemInventoryIndex ) { continue; }

				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( PaintHelpers.IsPaint(item) ) {
					if( PaintHelpers.GetPaintAmount(item) <= 0 ) { continue; }
					if( PaintHelpers.GetPaintColor(item) != curr_color ) { continue; }

					this.CurrentPaintItemInventoryIndex = i;
					found = true;

					break;
				}
			}

			return found;
		}
	}
}
