using BetterPaint.Tiles;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class CopyCartridgeItem : ModItem {
		public static void SetWithColor( Player player, Item copy_cart_item, Color color ) {
			ItemHelpers.ReduceStack( copy_cart_item, 1 );

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
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 0, 50, 0 );
			this.item.rare = 3;
		}


		////////////////

		public override void AddRecipes() {
			var mymod = (BetterPaintMod)this.mod;
			var anew1_recipe = new CopyCartridgeRecipe( mymod, mymod.GetItem<ColorCartridgeItem>(), this );
			var anew2_recipe = new CopyCartridgeRecipe( mymod, mymod.GetItem<GlowCartridgeItem>(), this );
			var copy_recipe = new CopyCartridgeCopyRecipe( mymod );

			anew1_recipe.AddRecipe();
			anew2_recipe.AddRecipe();
			copy_recipe.AddRecipe();
		}
	}




	class CopyCartridgeRecipe : ModRecipe {
		public CopyCartridgeRecipe( BetterPaintMod mymod, ModItem basecart, CopyCartridgeItem copycart ) : base( mymod ) {
			int mana_pot_qt = mymod.Config.CopyPaintRecipeManaPotions;
			int nanite_qt = mymod.Config.CopyPaintRecipeNanites;

			this.AddTile( mymod.TileType<PaintMixerTile>() );

			this.AddIngredient( basecart, 1 );

			if( mana_pot_qt > 0 ) {
				this.AddIngredient( ItemID.GreaterManaPotion, mymod.Config.CopyPaintRecipeManaPotions );
			}
			if( nanite_qt > 0 ) {
				this.AddIngredient( ItemID.Nanites, mymod.Config.CopyPaintRecipeNanites );
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
			this.AddTile( mymod.TileType<PaintMixerTile>() );

			this.AddIngredient( mymod.ItemType<CopyCartridgeItem>(), 1 );
			this.AddIngredient( mymod.ItemType<ColorCartridgeItem>(), 1 );

			this.SetResult( mymod.GetItem<ColorCartridgeItem>() );
		}


		public override int ConsumeItem( int item_type, int num_required ) {
			var mymod = (BetterPaintMod)this.mod;
			var inv = Main.LocalPlayer.inventory;
			int cart_type = mymod.ItemType<ColorCartridgeItem>();

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != cart_type ) { continue; }

				var mycart = (ColorCartridgeItem)inv[i].modItem;

				this.CopyColor = mycart.MyColor;
				break;
			}

			return base.ConsumeItem( item_type, num_required );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			var mycart1 = (ColorCartridgeItem)item.modItem;
			int cart_type = mymod.ItemType<ColorCartridgeItem>();

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
