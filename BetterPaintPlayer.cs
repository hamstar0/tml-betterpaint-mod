using BetterPaint.Items;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	partial class BetterPaintPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item held_item = this.player.HeldItem;

			if( held_item != null && !held_item.IsAir && held_item.type == this.mod.ItemType<PaintBlasterItem>() ) {
				var blaster_data = (PaintBlasterItem)held_item.modItem;
				blaster_data.CheckUse();
			}
		}
	}
}
