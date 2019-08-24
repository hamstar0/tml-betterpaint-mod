using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	class PaintDisplayInfo {
		public static IDictionary<string, PaintDisplayInfo> GetPaintSelection( Player player ) {
			var mymod = BetterPaintMod.Instance;
			IList<int> paintIdxs = new List<int>();
			Item[] inv = player.inventory;

			for( int i = 0; i < inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( PaintHelpers.IsPaint(item) ) {
					paintIdxs.Add( i );
				}
			}

			var paintInfo = new Dictionary<string, PaintDisplayInfo>( paintIdxs.Count );

			foreach( int idx in paintIdxs ) {
				Item item = Main.LocalPlayer.inventory[idx];
				if( PaintHelpers.GetPaintAmount( item ) <= 0 ) { continue; }

				string key = PaintHelpers.GetPaintColor( item ).ToString()+"_"+PaintHelpers.GetPaintType(item);

				if( !paintInfo.ContainsKey( key ) ) {
					paintInfo[key] = new PaintDisplayInfo( idx, item );
				} else {
					paintInfo[key].Copies++;
				}
			}

			return paintInfo;
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
				var colorCart = (ColorCartridgeItem)this.PaintItem.modItem;

				color = colorCart.MyColor;
				percent = colorCart.Quantity / (float)mymod.Config.PaintCartridgeCapacity;
				stack = this.Copies;
			} else if( this.PaintItem.modItem is GlowCartridgeItem ) {
				var glowCart = (GlowCartridgeItem)this.PaintItem.modItem;

				color = glowCart.MyColor;
				percent = glowCart.Quantity / (float)mymod.Config.PaintCartridgeCapacity;
				stack = this.Copies;
			} else if( ItemGroupIdentityHelpers.Paints.Group.Contains(this.PaintItem.type) ) {
				color = WorldGen.paintColor( this.PaintItem.paint );
				percent = (float)this.PaintItem.stack / 999f;
				stack = this.PaintItem.stack;
			} else {
				throw new ModHelpersException( "Not implemented." );
			}
		}
	}
}
