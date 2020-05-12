using BetterPaint.Items;
using BetterPaint.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	partial class BetterPaintPlayer : ModPlayer {
		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					WorldPaintDataProtocol.SendToClient( toWho, -1 );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }
		}


		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item heldItem = this.player.HeldItem;

			if( heldItem != null && !heldItem.IsAir && heldItem.type == ModContent.ItemType<PaintBlasterItem>() ) {
				var myblaster = (PaintBlasterItem)heldItem.modItem;

				myblaster.CheckMenu();
				myblaster.CheckSettings( this.player );
			}
		}
	}
}
