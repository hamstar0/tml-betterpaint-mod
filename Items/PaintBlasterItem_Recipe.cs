using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public override void AddRecipes() {
			var recipe = new PaintBlasterRecipe( (BetterPaintMod)this.mod, this );
			recipe.AddRecipe();
		}
	}



	class PaintBlasterRecipe : ModRecipe {
		public PaintBlasterRecipe( BetterPaintMod mymod, PaintBlasterItem myblaster ) : base( mymod ) {
			this.AddTile( TileID.TinkerersWorkbench );

			this.AddIngredient( ItemID.Clentaminator );
			this.AddIngredient( ItemID.Flamethrower );
			this.AddIngredient( ItemID.PaintSprayer );

			this.SetResult( myblaster );
		}


		public override bool RecipeAvailable() {
			return ( (BetterPaintMod)this.mod ).Config.PaintBlasterRecipeEnabled;
		}
	}
}
