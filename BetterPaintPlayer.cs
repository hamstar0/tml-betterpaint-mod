using BetterPaint.Items;
using HamstarHelpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	partial class BetterPaintPlayer : ModPlayer {
		public bool IsModeSelecting { get; private set; }
		public PaintMode CurrentMode { get; private set; }


		////////////////
		
		public BetterPaintPlayer() : base() {
			this.IsModeSelecting = false;
			this.CurrentMode = PaintMode.Stream;
		}

		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item curritem = this.player.HeldItem;

			if( curritem != null && !curritem.IsAir && curritem.type == this.mod.ItemType<PaintBlasterItem>() ) {
				if( Main.mouseRight ) {
					this.IsModeSelecting = true;
				} else if( this.IsModeSelecting ) {
					this.IsModeSelecting = false;
				}
				
				if( Main.mouseLeft ) {
					Vector2 tile_pos = UIHelpers.GetWorldMousePosition() / 16f;
					int x = (int)tile_pos.X;
					int y = (int)tile_pos.Y;

					if( !BetterPaintTile.Colors.ContainsKey(x) ) {
						BetterPaintTile.Colors[x] = new Dictionary<int, Color>();
					}
					BetterPaintTile.Colors[x][y] = Color.Red;
				}
			}
		}
	}
}
