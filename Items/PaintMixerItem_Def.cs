using BetterPaint.Tiles;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class PaintMixerItem : ModItem {
		public const int Width = 24;
		public const int Height = 24;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Paint Mixer" );
			this.Tooltip.SetDefault( "Mixes color cartridges from paint." );
		}

		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.createTile = this.mod.TileType<PaintMixerTile>();
		}
	}
}
