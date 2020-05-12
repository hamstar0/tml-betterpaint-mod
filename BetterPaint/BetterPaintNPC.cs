using BetterPaint.Items;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintNPC : GlobalNPC {
		public override void SetupShop( int npcType, Chest shop, ref int nextSlot ) {
			if( npcType != NPCID.Painter ) { return; }

			var mymod = (BetterPaintMod)this.mod;
			int max = shop.item.Length;
			
			if( mymod.Config.PainterSellsBlaster && nextSlot < max ) {
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<PaintBlasterItem>() );
				nextSlot++;
			}

			if( mymod.Config.PainterSellsPaintMixer && nextSlot < max ) {
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<PaintMixerItem>() );
				nextSlot++;
			}

			if( mymod.Config.PainterSellsRGBCartridges && nextSlot < (max - 3) ) {
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[nextSlot].modItem ).SetPaint( new Color(255, 0, 0), mymod.Config.PaintCartridgeCapacity );
				nextSlot++;
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[nextSlot].modItem ).SetPaint( new Color(0, 255, 0), mymod.Config.PaintCartridgeCapacity );
				nextSlot++;
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[nextSlot].modItem ).SetPaint( new Color(0, 0, 255), mymod.Config.PaintCartridgeCapacity );
				nextSlot++;
			}

			if( mymod.Config.PainterSellsCopyCartridge && nextSlot < max ) {
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults( ModContent.ItemType<CopyCartridgeItem>() );
				nextSlot++;
			}
		}
	}
}
