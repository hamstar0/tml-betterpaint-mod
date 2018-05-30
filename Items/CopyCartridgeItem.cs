using BetterPaint.Tiles;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class CopyCartridgeItem : ModItem {
		public static void SetWithColor( Player player, int copy_cart_inv_idx, Color color ) {
			ItemHelpers.DestroyItem( player.inventory[copy_cart_inv_idx] );
			
			player.inventory[copy_cart_inv_idx] = new Item();

			int idx = ItemHelpers.CreateItem( player.position, BetterPaintMod.Instance.ItemType<ColorCartridgeItem>(), 1,
				ColorCartridgeItem.Width, ColorCartridgeItem.Height );
			var myitem = (ColorCartridgeItem)Main.item[ idx ].modItem;
			myitem.SetColor( color );
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


		////////////////

		public override void AddRecipes() {
			var mymod = (BetterPaintMod)this.mod;
			var recipe = new ModRecipe( (BetterPaintMod)this.mod );

			recipe.AddTile( TileID.WorkBenches );
			recipe.AddTile( mymod.TileType<PaintMixerTile>() );
			recipe.AddIngredient( mymod.GetItem<ColorCartridgeItem>(), 1 );
			recipe.AddIngredient( ItemID.GreaterHealingPotion, mymod.Config.CopyPaintManaPotionIngredientQuantity );
			recipe.SetResult( this, 1 );
			recipe.AddRecipe();
		}
	}
}
