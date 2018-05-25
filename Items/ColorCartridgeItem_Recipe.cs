using BetterPaint.Tiles;
using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public override void AddRecipes() {
			var recipe = new ColorCartridgeRecipe( (BetterPaintMod)this.mod, this );
			recipe.AddRecipe();
		}
	}



	class ColorCartridgeRecipe : ModRecipe {
		public ColorCartridgeRecipe( BetterPaintMod mymod, ColorCartridgeItem myitem ) : base ( mymod ) {
			this.AddTile( mymod.TileType<PaintMixerTile>() );
			this.AddIngredient( ItemID.Gel, mymod.Config.PaintRecipeGelIngredientQuantity );
			this.AddRecipeGroup( "HamstarHelpers:Paints", mymod.Config.PaintRecipePaintIngredientQuantity );
			this.SetResult( myitem, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.PaintRecipeEnabled;
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			ISet<int> paints = ItemIdentityHelpers.Paints.Item2;
			var myitem = (ColorCartridgeItem)item.modItem;
			var inv = Main.LocalPlayer.inventory;
			
			int max_paints = mymod.Config.PaintRecipePaintIngredientQuantity;
			int count = 0;

			int r = 0;
			int g = 0;
			int b = 0;
			int a = 0;

			for( int i=0; i<inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }

				if( paints.Contains( inv[i].type ) ) {
					Color paint_clr = WorldGen.paintColor( inv[i].paint );
					int stack = inv[i].stack;

					count += stack;

					if( count >= max_paints ) {
						stack -= count - max_paints;
					}

					r += paint_clr.R * stack;
					g += paint_clr.G * stack;
					b += paint_clr.B * stack;
					a += paint_clr.A * stack;

					if( count >= max_paints ) {
						break;
					}
				}
			}

			var clr = new Color( r / max_paints, g / max_paints, b / max_paints, a / max_paints );

			myitem.SetColor( clr );
		}
	}
}
