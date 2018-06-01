using BetterPaint.Items;
using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintNPC : GlobalNPC {
		public override void SetupShop( int npc_type, Chest shop, ref int next_slot ) {
			if( npc_type != NPCID.Painter ) { return; }

			var mymod = (BetterPaintMod)this.mod;
			int max = shop.item.Length;
			
			if( mymod.Config.PainterSellsBlaster && next_slot < max ) {
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<PaintBlasterItem>() );
				next_slot++;
			}

			if( mymod.Config.PainterSellsPaintMixer && next_slot < max ) {
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<PaintMixerItem>() );
				next_slot++;
			}

			if( mymod.Config.PainterSellsRGBCartridges && next_slot < (max - 3) ) {
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[next_slot].modItem ).SetPaint( new Color(255, 0, 0), mymod.Config.PaintCartridgeCapacity );
				next_slot++;
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[next_slot].modItem ).SetPaint( new Color(0, 255, 0), mymod.Config.PaintCartridgeCapacity );
				next_slot++;
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<ColorCartridgeItem>() );
				( (ColorCartridgeItem)shop.item[next_slot].modItem ).SetPaint( new Color(0, 0, 255), mymod.Config.PaintCartridgeCapacity );
				next_slot++;
			}

			if( mymod.Config.PainterSellsCopyCartridge && next_slot < max ) {
				shop.item[next_slot] = new Item();
				shop.item[next_slot].SetDefaults( mymod.ItemType<CopyCartridgeItem>() );
				next_slot++;
			}
		}
	}
}
