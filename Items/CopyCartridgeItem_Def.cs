using BetterPaint.Tiles;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class CopyCartridgeItem : ModItem {
		public static void SetWithColor( Player player, Item copy_cart_item, Color color ) {
			ItemHelpers.DestroyItem( copy_cart_item );

			int cart_idx = ItemHelpers.CreateItem( player.position, BetterPaintMod.Instance.ItemType<ColorCartridgeItem>(), 1,
				ColorCartridgeItem.Width, ColorCartridgeItem.Height );
			var mycart = (ColorCartridgeItem)Main.item[ cart_idx ].modItem;

			mycart.SetPaint( color, BetterPaintMod.Instance.Config.PaintCartridgeCapacity );
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

			int mana_pot_qt = mymod.Config.CopyPaintManaPotionIngredientQuantity;
			int nanite_qt = mymod.Config.CopyPaintNaniteIngredientQuantity;
			
			recipe.AddTile( mymod.TileType<PaintMixerTile>() );

			recipe.AddIngredient( mymod.ItemType<ColorCartridgeItem>(), 1 );
			if( mana_pot_qt > 0 ) {
				recipe.AddIngredient( ItemID.GreaterManaPotion, mymod.Config.CopyPaintManaPotionIngredientQuantity );
			}
			if( nanite_qt > 0 ) {
				recipe.AddIngredient( ItemID.Nanites, mymod.Config.CopyPaintNaniteIngredientQuantity );
			}

			recipe.SetResult( this, 1 );
			recipe.AddRecipe();
		}
	}
}
