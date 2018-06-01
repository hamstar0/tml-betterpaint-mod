using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	class PaintMixerItem : ModItem {
		public const int Width = 30;
		public const int Height = 30;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Paint Mixer" );
			this.Tooltip.SetDefault( "Mixes color cartridges from paint." );
		}

		public override void SetDefaults() {
			this.item.width = ColorCartridgeItem.Width;
			this.item.height = ColorCartridgeItem.Height;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );
			this.item.maxStack = 99;
			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.useStyle = 1;
			this.item.consumable = true;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.createTile = mod.TileType( "PaintMixerTile" );
		}


		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe( mod );
			recipe.AddTile( TileID.HeavyWorkBench );
			recipe.AddIngredient( ItemID.DyeVat, 1 );
			recipe.AddIngredient( ItemID.BlendOMatic, 1 );
			recipe.AddIngredient( ItemID.Extractinator, 1 );
			recipe.SetResult( this );
			recipe.AddRecipe();
		}
	}
}
