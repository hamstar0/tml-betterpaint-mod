﻿using BetterPaint.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class GlowCartridgeItem : ModItem {
		public override void AddRecipes() {
			var paint_recipe = new GlowCartridgeRecipe( (BetterPaintMod)this.mod, this );
			paint_recipe.AddRecipe();
		}
	}




	class GlowCartridgeRecipe : ModRecipe {
		private ColorCartridgeItem BaseCartridge = null;


		////////////////

		public GlowCartridgeRecipe( BetterPaintMod mymod, GlowCartridgeItem myitem ) : base( mymod ) {
			this.AddTile( mymod.TileType<PaintMixerTile>() );
			this.AddIngredient( mymod.GetItem<ColorCartridgeItem>(), 1 );
			this.AddIngredient( ItemID.JungleSpores, mymod.Config.GlowPaintRecipeSpores );
			this.SetResult( myitem, 1 );
		}
		
		public override bool RecipeAvailable() {
			var mymod = (BetterPaintMod)this.mod;
			return mymod.Config.GlowPaintRecipeEnabled;
		}


		////////////////

		public override int ConsumeItem( int item_type, int num_required ) {
			var mymod = (BetterPaintMod)this.mod;
			var inv = Main.LocalPlayer.inventory;
			int cart_type = mymod.ItemType<ColorCartridgeItem>();

			if( item_type != cart_type ) {
				return base.ConsumeItem( item_type, num_required );
			}

			for( int i = 0; i < inv.Length; i++ ) {
				if( inv[i] == null || inv[i].IsAir ) { continue; }
				if( inv[i].type != cart_type ) { continue; }
				
				this.BaseCartridge = (ColorCartridgeItem)inv[i].modItem;
				break;
			}

			return base.ConsumeItem( item_type, num_required );
		}


		public override void OnCraft( Item item ) {
			if( this.BaseCartridge == null ) { return; }

			var mymod = (BetterPaintMod)this.mod;
			var myitem = (GlowCartridgeItem)item.modItem;

			myitem.SetQuantity( this.BaseCartridge.PaintQuantity );
		}
	}
}
