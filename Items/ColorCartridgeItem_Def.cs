using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public const int Width = 24;
		public const int Height = 24;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Color Cartridge" );
			this.Tooltip.SetDefault( "Paint with super rich color." + '\n' +
				"Overlaps all regular paint" + '\n' +
				"Needs a paint blaster to use" + '\n' +
				"Mix colors at a paint mixer" );
		}

		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 0, 10, 0 );
		}

		////////////////

		public ColorCartridgeItemData GetData() {
			return this.item.GetGlobalItem<ColorCartridgeItemData>();
		}
	}
}
