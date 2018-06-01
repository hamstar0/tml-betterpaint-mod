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
			this.Tooltip.SetDefault( "Set color with Paint Blaster's copy function" );
		}


		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 0, 50, 0 );
			this.item.rare = 3;
		}


		////////////////

		public override void AddRecipes() {
			var mymod = (BetterPaintMod)this.mod;
			var recipe = new CopyCartridgeRecipe( mymod, this );
			recipe.AddRecipe();
		}
	}



	class CopyCartridgeRecipe : ModRecipe {
		public CopyCartridgeRecipe( BetterPaintMod mymod, CopyCartridgeItem mycart ) : base( mymod ) {
			int mana_pot_qt = mymod.Config.CopyPaintRecipeManaPotions;
			int nanite_qt = mymod.Config.CopyPaintRecipeNanites;

			this.AddTile( mymod.TileType<PaintMixerTile>() );

			this.AddIngredient( mymod.ItemType<ColorCartridgeItem>(), 1 );
			if( mana_pot_qt > 0 ) {
				this.AddIngredient( ItemID.GreaterManaPotion, mymod.Config.CopyPaintRecipeManaPotions );
			}
			if( nanite_qt > 0 ) {
				this.AddIngredient( ItemID.Nanites, mymod.Config.CopyPaintRecipeNanites );
			}

			this.SetResult( mycart );
		}


		public override bool RecipeAvailable() {
			return ( (BetterPaintMod)this.mod ).Config.CopyPaintRecipeEnabled;
		}
	}
}
