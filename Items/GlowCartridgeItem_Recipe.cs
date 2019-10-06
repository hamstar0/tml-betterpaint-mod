using BetterPaint.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class GlowCartridgeItem : ModItem {
		public override void AddRecipes() {
			var paintRecipe = new GlowCartridgeRecipe( (BetterPaintMod)this.mod, this );
			paintRecipe.AddRecipe();
		}
	}




	class GlowCartridgeRecipe : ModRecipe {
		private Color BaseColor;
		private float Quantity;


		////////////////

		public GlowCartridgeRecipe( BetterPaintMod mymod, GlowCartridgeItem myitem ) : base( mymod ) {
			this.AddTile( ModContent.TileType<PaintMixerTile>() );
			this.AddIngredient( ModContent.GetInstance<ColorCartridgeItem>(), 1 );
			if( mymod.Config.GlowPaintRecipeSpores > 0 ) {
				this.AddIngredient( ItemID.JungleSpores, mymod.Config.GlowPaintRecipeSpores );
			}
			this.SetResult( myitem, 1 );
		}
		
		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.GlowPaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int itemType, int numRequired ) {
			var inv = Main.LocalPlayer.inventory;
			int cartType = ModContent.ItemType<ColorCartridgeItem>();

			if( itemType != cartType ) {
				return base.ConsumeItem( itemType, numRequired );
			}

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != cartType ) { continue; }

				var cart = (ColorCartridgeItem)inv[i].modItem;

				this.Quantity = cart.Quantity;
				this.BaseColor = cart.MyColor;
				break;
			}

			return base.ConsumeItem( itemType, numRequired );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			var myitem = (GlowCartridgeItem)item.modItem;

			myitem.SetPaint( this.BaseColor, this.Quantity );
		}
	}
}
