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
			var paint_recipe = new ColorCartridgePaintRecipe( (BetterPaintMod)this.mod, this );
			paint_recipe.AddRecipe();

			var blend_recipe = new ColorCartridgeBlendRecipe( (BetterPaintMod)this.mod, this );
			blend_recipe.AddRecipe();
		}
	}



	class ColorCartridgeBlendRecipe : ModRecipe {
		private ColorCartridgeItem First = null;
		private ColorCartridgeItem Second = null;



		public ColorCartridgeBlendRecipe( BetterPaintMod mymod, ColorCartridgeItem myitem ) : base( mymod ) {
			this.AddTile( mymod.TileType<PaintMixerTile>() );
			this.AddIngredient( myitem, 2 );
			this.SetResult( myitem, 1 );
		}

		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.PaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int item_type, int num_required ) {
			var mymod = (BetterPaintMod)this.mod;
			var inv = Main.LocalPlayer.inventory;

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != item_type ) { continue; }

				if( this.First == null ) {
					this.First = (ColorCartridgeItem)inv[i].modItem;
					continue;
				}
				if( this.Second == null ) {
					this.Second = (ColorCartridgeItem)inv[i].modItem;
				}
				break;
			}

			if( this.First != null && this.Second == null ) {
				this.First = null;
			}

			return base.ConsumeItem( item_type, num_required );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;

			int item2_idx = ItemHelpers.CreateItem( Main.LocalPlayer.position, item.type, 1, ColorCartridgeItem.Width, ColorCartridgeItem.Height );

			var mycart1 = (ColorCartridgeItem)item.modItem;
			var mycart2 = (ColorCartridgeItem)Main.item[ item2_idx ].modItem;

			float volume = (this.First.PaintQuantity + this.Second.PaintQuantity) / 2f;
			float shift = this.Second.PaintQuantity / (this.First.PaintQuantity + this.Second.PaintQuantity);

			Color mix = Color.Lerp( this.First.MyColor, this.Second.MyColor, shift );

			mycart1.SetPaint( mix, volume );
			mycart2.SetPaint( mix, volume );

			this.First = null;
			this.Second = null;
		}
	}



	class ColorCartridgePaintRecipe : ModRecipe {
		private Color CraftColor;


		////////////////

		public ColorCartridgePaintRecipe( BetterPaintMod mymod, ColorCartridgeItem myitem ) : base ( mymod ) {
			this.AddTile( mymod.TileType<PaintMixerTile>() );
			this.AddIngredient( ItemID.Gel, mymod.Config.PaintRecipeGelIngredientQuantity );
			this.AddRecipeGroup( "HamstarHelpers:Paints", mymod.Config.PaintRecipePaintIngredientQuantity );
			this.SetResult( myitem, 1 );
		}

		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.PaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int item_type, int num_required ) {
			if( !ItemIdentityHelpers.Paints.Item2.Contains( item_type ) ) {   // Not paint
				return base.ConsumeItem( item_type, num_required );
			}

			var mymod = (BetterPaintMod)this.mod;
			var inv = Main.LocalPlayer.inventory;
			int max_paints = mymod.Config.PaintRecipePaintIngredientQuantity;
			ISet<int> paints = ItemIdentityHelpers.Paints.Item2;
			int count = 0;

			int r = 0;
			int g = 0;
			int b = 0;
			int a = 0;

			for( int i = 0; i < inv.Length; i++ ) {
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

			this.CraftColor = new Color( r / max_paints, g / max_paints, b / max_paints, a / max_paints );

			return base.ConsumeItem( item_type, num_required );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			var myitem = (ColorCartridgeItem)item.modItem;
			
			myitem.SetPaint( this.CraftColor, mymod.Config.PaintCartridgeCapacity );
		}
	}
}
