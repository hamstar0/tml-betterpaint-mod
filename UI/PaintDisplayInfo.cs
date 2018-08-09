using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.Helpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	class PaintDisplayInfo {
		public static IDictionary<string, PaintDisplayInfo> GetPaintSelection( Player player ) {
			var mymod = BetterPaintMod.Instance;
			IList<int> paint_idxs = new List<int>();
			Item[] inv = player.inventory;

			for( int i = 0; i < inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( PaintHelpers.IsPaint(item) ) {
					paint_idxs.Add( i );
				}
			}

			var paint_info = new Dictionary<string, PaintDisplayInfo>( paint_idxs.Count );

			foreach( int idx in paint_idxs ) {
				Item item = Main.LocalPlayer.inventory[idx];
				if( PaintHelpers.GetPaintAmount( item ) <= 0 ) { continue; }

				string key = PaintHelpers.GetPaintColor( item ).ToString()+"_"+PaintHelpers.GetPaintType(item);

				if( !paint_info.ContainsKey( key ) ) {
					paint_info[key] = new PaintDisplayInfo( idx, item );
				} else {
					paint_info[key].Copies++;
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
				percent = color_cart.Quantity / (float)mymod.Config.PaintCartridgeCapacity;
				stack = this.Copies;
			} else if( this.PaintItem.modItem is GlowCartridgeItem ) {
				var glow_cart = (GlowCartridgeItem)this.PaintItem.modItem;

				color = glow_cart.MyColor;
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
