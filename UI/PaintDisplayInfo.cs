using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	class PaintDisplayInfo {
		public static IDictionary<string, PaintDisplayInfo> GetPaintInfo( Player player ) {
			var mymod = BetterPaintMod.Instance;
			IList<int> paint_idxs = new List<int>();
			Item[] inv = player.inventory;
			int color_cartridge_type = mymod.ItemType<ColorCartridgeItem>();
			int glow_cartridge_type = mymod.ItemType<ColorCartridgeItem>();

			for( int i = 0; i < inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( item.type == color_cartridge_type ) {
					paint_idxs.Add( i );
				} else if( item.type == glow_cartridge_type ) {
					paint_idxs.Add( i );
				} else if( ItemIdentityHelpers.Paints.Item2.Contains( item.type ) ) {
					paint_idxs.Add( i );
				}
			}

			var paint_info = new Dictionary<string, PaintDisplayInfo>( paint_idxs.Count );
			var angles = new Dictionary<int, float>( paint_idxs.Count );

			foreach( int idx in paint_idxs ) {
				Item item = Main.LocalPlayer.inventory[idx];
				if( PaintLayer.GetPaintAmount( item ) <= 0 ) { continue; }

				string color_key = PaintLayer.GetPaintColor( item ).ToString();

				if( !paint_info.ContainsKey( color_key ) ) {
					paint_info[color_key] = new PaintDisplayInfo( idx, item );
				} else {
					paint_info[color_key].Copies++;
				}
			}

			return paint_info;
		}



		////////////////

		public int Copies;
		public int FirstInventoryIndex;
		public Item PaintItem;

		public PaintDisplayInfo( int idx, Item paint ) {
			this.Copies = 1;
			this.FirstInventoryIndex = idx;
			this.PaintItem = paint;
		}


		public void GetDrawInfo( BetterPaintMod mymod, int x, int y, out Color color, out float percent, out int stack ) {
			if( this.PaintItem.modItem is ColorCartridgeItem ) {
				var color_cart = (ColorCartridgeItem)this.PaintItem.modItem;

				color = color_cart.MyColor;
				percent = color_cart.PaintQuantity / (float)mymod.Config.PaintCartridgeCapacity;
				stack = this.Copies;
			} else if( this.PaintItem.modItem is GlowCartridgeItem ) {
				var glow_cart = (GlowCartridgeItem)this.PaintItem.modItem;

				color = Color.SpringGreen;
				percent = glow_cart.Quantity / (float)mymod.Config.PaintCartridgeCapacity;
				stack = this.Copies;
			} else if( ItemIdentityHelpers.Paints.Item2.Contains(this.PaintItem.type) ) {
				color = WorldGen.paintColor( this.PaintItem.paint );
				percent = (float)this.PaintItem.stack / 999f;
				stack = this.PaintItem.stack;
			} else {
				throw new NotImplementedException();
			}
		}
	}
}
