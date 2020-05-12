using BetterPaint.Tiles;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public override void AddRecipes() {
			var paintRecipe = new ColorCartridgePaintRecipe( (BetterPaintMod)this.mod, this );
			paintRecipe.AddRecipe();

			var blendRecipe = new ColorCartridgeBlendRecipe( (BetterPaintMod)this.mod, this );
			blendRecipe.AddRecipe();
		}
	}




	class ColorCartridgeBlendRecipe : ModRecipe {
		private ColorCartridgeItem First = null;
		private ColorCartridgeItem Second = null;



		public ColorCartridgeBlendRecipe( BetterPaintMod mymod, ColorCartridgeItem myitem ) : base( mymod ) {
			this.AddTile( ModContent.TileType<PaintMixerTile>() );
			this.AddIngredient( myitem, 2 );
			this.SetResult( myitem, 1 );
		}

		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.PaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int itemType, int numRequired ) {
			var inv = Main.LocalPlayer.inventory;
			int cartType = ModContent.ItemType<ColorCartridgeItem>();

			if( itemType != cartType ) {	// Won't be invoked, but future-proofing won't hurt
				return base.ConsumeItem( itemType, numRequired );
			}

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != cartType ) { continue; }

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

			return base.ConsumeItem( itemType, numRequired );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;

			int item2Idx = ItemHelpers.CreateItem( Main.LocalPlayer.position, item.type, 1, ColorCartridgeItem.Width, ColorCartridgeItem.Height );

			var mycart1 = (ColorCartridgeItem)item.modItem;
			var mycart2 = (ColorCartridgeItem)Main.item[ item2Idx ].modItem;

			float volume = (this.First.Quantity + this.Second.Quantity) / 2f;
			float shift = this.Second.Quantity / (this.First.Quantity + this.Second.Quantity);

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
			this.AddTile( ModContent.TileType<PaintMixerTile>() );

			if( mymod.Config.PaintRecipeGels > 0 ) {
				this.AddIngredient( ItemID.Gel, mymod.Config.PaintRecipeGels );
			}
			if( mymod.Config.PaintRecipePaints > 0 ) {
				this.AddRecipeGroup( "ModHelpers:Paints", mymod.Config.PaintRecipePaints );
			}

			this.SetResult( myitem, 1 );
		}

		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.PaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int itemType, int numRequired ) {
			if( !ItemGroupIdentityHelpers.Paints.Group.Contains( itemType ) ) {   // Not paint
				return base.ConsumeItem( itemType, numRequired );
			}

			var mymod = (BetterPaintMod)this.mod;
			var inv = Main.LocalPlayer.inventory;
			int maxPaints = mymod.Config.PaintRecipePaints;
			ISet<int> paints = ItemGroupIdentityHelpers.Paints.Group;
			int count = 0;

			int r = 0;
			int g = 0;
			int b = 0;
			int a = 0;

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }

				if( paints.Contains( inv[i].type ) ) {
					Color paintClr = WorldGen.paintColor( inv[i].paint );
					int stack = inv[i].stack;

					count += stack;

					if( count >= maxPaints ) {
						stack -= count - maxPaints;
					}

					r += paintClr.R * stack;
					g += paintClr.G * stack;
					b += paintClr.B * stack;
					a += paintClr.A * stack;

					if( count >= maxPaints ) {
						break;
					}
				}
			}

			this.CraftColor = new Color( r / maxPaints, g / maxPaints, b / maxPaints );

			return base.ConsumeItem( itemType, numRequired );
		}


		public override void OnCraft( Item item ) {
			var mymod = (BetterPaintMod)this.mod;
			var myitem = (ColorCartridgeItem)item.modItem;
			
			myitem.SetPaint( this.CraftColor, mymod.Config.PaintCartridgeCapacity );
		}
	}
}
