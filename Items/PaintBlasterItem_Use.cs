using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		private IDictionary<ushort, ushort> BufferedPaintedTiles = new Dictionary<ushort, ushort>();
		private float BufferedPaintUses = 0f;
		private bool DryFireAvailable = true;


		////////////////

		public override bool CanUseItem( Player player ) {
			if( this.IsUsingUI ) {
				return false;
			}
			if( this.IsCopying ) {
				return true;
			}

			bool canPaintAt = true;
			Item paintItem = null;

			if( this.CurrentBrush != PaintBrushType.Erase ) {
				paintItem = this.GetCurrentPaintItem();

				if( paintItem == null ) {
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
			}

			Vector2 worldPos = UIHelpers.GetWorldMousePosition();
			Vector2 tilePos = worldPos / 16f;
			ushort tileX = (ushort)tilePos.X;
			ushort tileY = (ushort)tilePos.Y;

			if( paintItem == null ) {
				canPaintAt = true;
			} else {
				canPaintAt = this.CanPaintAt( paintItem, tileX, tileY );
			}
			
			if( canPaintAt ) {
				if( this.CanUseBlasterAt( tileX, tileY ) ) {
					this.BufferedPaintUses += this.BlastPaintAt( worldPos );
				}
			}

			return canPaintAt;
		}


		public override bool Shoot( Player player, ref Vector2 pos, ref float velX, ref float velY, ref int type, ref int dmg,
				ref float kb ) {
			Vector2 worldMousePos = new Vector2( Main.mouseX, Main.mouseY ) + Main.screenPosition;
			int worldX = (int)worldMousePos.X;
			int worldY = (int)worldMousePos.Y;
			ushort tileX = (ushort)( worldX / 16 );
			ushort tileY = (ushort)( worldY / 16 );
			Color? dustColor = null;
			
			if( this.IsCopying ) {
				if( !this.AttemptCopyColorAt( player, tileX, tileY ) ) {
					Main.NewText( "No color found to copy from.", Color.Yellow );
				}

				this.IsCopying = false;
			} else {
				Item paintItem = this.GetCurrentPaintItem();

				if( paintItem != null ) {
					if( PaintHelpers.GetPaintAmount(paintItem) <= 0 ) {
						if( this.SwitchToNextMatchingNonemptyPaint() ) {
							paintItem = this.GetCurrentPaintItem();
						} else {
							paintItem = null;
						}
					}

					if( paintItem != null ) {
						Color paintColor = PaintHelpers.GetPaintColor( paintItem );

						if( this.HasMatchingPaintAt( paintColor, tileX, tileY ) ) {
							dustColor = paintColor;
						}
					}
				}
			}

			if( this.BufferedPaintUses > 0 && dustColor != null ) {
				this.BufferedPaintUses = 0f;

				pos = PlayerItemHelpers.TipOfHeldItem( player ) - Main.screenPosition;
				Dust.NewDust( pos, 8, 8, 2, velX, velY, 0, (Color)dustColor, 1f );
			}
			
			return false;
		}


		////////////////

		public bool CanUseBlasterAt( ushort tileX, ushort tileY ) {
			if( this.BufferedPaintedTiles.ContainsKey( tileX ) ) {
				if( this.BufferedPaintedTiles[tileX] == tileY ) {
					return false;
				}
			}
			this.BufferedPaintedTiles[tileX] = tileY;

			return true;
		}

		public float BlastPaintAt( Vector2 worldPos ) {
			if( Timers.GetTimerTickDuration( "PaintBlasterFlowReset" ) <= 0 ) {
				Timers.SetTimer( "PaintBlasterFlowReset", 30, () => {
					this.BufferedPaintedTiles.Clear();
					return false;
				} );
			}
			
			return this.ApplyBrushAt( (int)worldPos.X, (int)worldPos.Y );
		}
		

		////////////////

		public bool SwitchToNextMatchingNonemptyPaint() {
			Item paintItem = this.GetCurrentPaintItem();
			if( paintItem == null ) { return false; }

			Color currColor = PaintHelpers.GetPaintColor( paintItem );
			
			Item[] inv = Main.LocalPlayer.inventory;
			int cartType = this.mod.ItemType<ColorCartridgeItem>();
			bool found = false;

			for( int i=0; i<inv.Length; i++ ) {
				if( i == this.CurrentPaintItemInventoryIndex ) { continue; }

				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( PaintHelpers.IsPaint(item) ) {
					if( PaintHelpers.GetPaintAmount(item) <= 0 ) { continue; }
					if( PaintHelpers.GetPaintColor(item) != currColor ) { continue; }

					this.CurrentPaintItemInventoryIndex = i;
					found = true;

					break;
				}
			}

			return found;
		}
	}
}
