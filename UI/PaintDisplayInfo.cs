using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.ItemHelpers;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	class PaintDisplayInfo {
		public static IDictionary<string, PaintDisplayInfo> GetPaintsByColorKey( Player player ) {
			IList<int> paint_idxs = new List<int>();
			Item[] inv = player.inventory;
			int cartridge_type = BetterPaintMod.Instance.ItemType<ColorCartridgeItem>();

			for( int i = 0; i < inv.Length; i++ ) {
				Item item = inv[i];
				if( item == null || item.IsAir ) { continue; }

				if( item.type == cartridge_type ) {
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
		public Item Paint;

		public PaintDisplayInfo( int idx, Item paint ) {
			this.Copies = 1;
			this.FirstInventoryIndex = idx;
			this.Paint = paint;
		}
	}
}
