﻿using BetterPaint.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintPlayer : ModPlayer {
		public bool IsModeSelecting { get; private set; }


		////////////////
		
		public BetterPaintPlayer() : base() {
			this.IsModeSelecting = false;
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
			}
		}


		////////////////

		private IList<int> GetPaintColors() {

		}

		////////////////

		public void DrawPainterUI( SpriteBatch sb ) {
			IList<int> icons = this.GetPaintColors();
			int icon_count = icons.Count;

			float angle_step = 360f / (float)icon_count;
			float angle = 0f;

			for( int i=0; i<icon_count; i++ ) {
				Item paint = this.player.inventory[ icons[i] ];
				var data = paint.GetGlobalItem<ColorCartridgeItemData>();
				
				int x = (int)(128d * Math.Cos( angle ));
				int y = (int)(128d * Math.Sin( angle ));

				this.DrawPaintIcon( data.MyColor, x, y );

				angle += angle_step;
			}
		}


		public void DrawPaintIcon( Color color, int x, int y ) {

		}
	}
}
