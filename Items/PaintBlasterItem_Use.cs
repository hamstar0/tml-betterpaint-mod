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
				pos = PlayerItemHelpers.TipOfHeldItem( player ) - Main.screenPosition;
				Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, (Color)dust_color, 1f );
			}

			return false;
		}
	}
}
