using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class CopyCartridgeItem : ModItem {
		public static void SetWithColor( Player player, int copy_cart_inv_idx, Color color ) {
			ItemHelpers.DestroyItem( player.inventory[copy_cart_inv_idx] );
			
			player.inventory[copy_cart_inv_idx] = new Item();
			player.inventory[copy_cart_inv_idx].SetDefaults( BetterPaintMod.Instance.ItemType<CopyCartridgeItem>(), true );
		}


		////////////////

		public const int Width = 12;
		public const int Height = 16;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Copy Cartridge" );
			this.Tooltip.SetDefault( "Fill with eyedropper mode (via. Paint Blaster)" );
		}


		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
		}
	}
}
