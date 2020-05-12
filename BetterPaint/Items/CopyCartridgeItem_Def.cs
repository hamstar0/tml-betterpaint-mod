using BetterPaint.Tiles;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class CopyCartridgeItem : ModItem {
		public static void SetWithColor( Player player, Item copyCartItem, Color color ) {
			ItemHelpers.ReduceStack( copyCartItem, 1 );

			int cartIdx = ItemHelpers.CreateItem( player.position, ModContent.ItemType<ColorCartridgeItem>(), 1,
				ColorCartridgeItem.Width, ColorCartridgeItem.Height );

			var mycart = (ColorCartridgeItem)Main.item[ cartIdx ].modItem;

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
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 0, 50, 0 );
			this.item.rare = 3;
		}


		////////////////

		public override void AddRecipes() {
			var mymod = (BetterPaintMod)this.mod;
			var anew1Recipe = new CopyCartridgeRecipe( mymod, ModContent.GetInstance<ColorCartridgeItem>(), this );
			var anew2Recipe = new CopyCartridgeRecipe( mymod, ModContent.GetInstance<GlowCartridgeItem>(), this );
			var copyRecipe = new CopyCartridgeCopyRecipe( mymod );

			anew1Recipe.AddRecipe();
			anew2Recipe.AddRecipe();
			copyRecipe.AddRecipe();
		}
	}




	class CopyCartridgeRecipe : ModRecipe {
		public CopyCartridgeRecipe( BetterPaintMod mymod, ModItem basecart, CopyCartridgeItem copycart ) : base( mymod ) {
			int manaPotQt = mymod.Config.CopyPaintRecipeManaPotions;
			int naniteQt = mymod.Config.CopyPaintRecipeNanites;

			this.AddTile( ModContent.TileType<PaintMixerTile>() );

			this.AddIngredient( basecart, 1 );

			if( manaPotQt > 0 ) {
				this.AddIngredient( ItemID.GreaterManaPotion, manaPotQt );
			}
			if( naniteQt > 0 ) {
				this.AddIngredient( ItemID.Nanites, naniteQt );
			}

			this.SetResult( copycart );
		}


		public override bool RecipeAvailable() {
			return ( (BetterPaintMod)this.mod ).Config.CopyPaintRecipeEnabled;
		}
	}




	class CopyCartridgeCopyRecipe : ModRecipe {
		private Color CopyColor;


		public CopyCartridgeCopyRecipe( BetterPaintMod mymod ) : base( mymod ) {
			this.AddTile( ModContent.TileType<PaintMixerTile>() );

			this.AddIngredient( ModContent.ItemType<CopyCartridgeItem>(), 1 );
			this.AddIngredient( ModContent.ItemType<ColorCartridgeItem>(), 1 );

			this.SetResult( ModContent.GetInstance<ColorCartridgeItem>() );
		}


		public override int ConsumeItem( int itemType, int numRequired ) {
			var inv = Main.LocalPlayer.inventory;
			int cartType = ModContent.ItemType<ColorCartridgeItem>();

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != cartType ) { continue; }

				var mycart = (ColorCartridgeItem)inv[i].modItem;

				this.CopyColor = mycart.MyColor;
				break;
			}

			return base.ConsumeItem( itemType, numRequired );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			var mycart1 = (ColorCartridgeItem)item.modItem;
			int cartType = ModContent.ItemType<ColorCartridgeItem>();

			//int item_idx = ItemHelpers.CreateItem( Main.LocalPlayer.position, cart_type, 1, ColorCartridgeItem.Width, ColorCartridgeItem.Height );
			//var mycart2 = (ColorCartridgeItem)Main.item[ item_idx ].modItem;

			mycart1.SetPaint( this.CopyColor, mymod.Config.PaintCartridgeCapacity );
			//mycart2.SetPaint( this.CopyColor, mymod.Config.PaintCartridgeCapacity );
		}


		public override bool RecipeAvailable() {
			return ( (BetterPaintMod)this.mod ).Config.CopyPaintRecipeEnabled;
		}
	}
}
