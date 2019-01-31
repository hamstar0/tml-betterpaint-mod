using BetterPaint.Items;
using BetterPaint.NetProtocols;
using HamstarHelpers.Components.Network;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	partial class BetterPaintPlayer : ModPlayer {
		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (BetterPaintMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
				}
			}

			if( Main.netMode == 1 ) {
				PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			}
		}


		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			Item heldItem = this.player.HeldItem;

			if( heldItem != null && !heldItem.IsAir && heldItem.type == this.mod.ItemType<PaintBlasterItem>() ) {
				var myblaster = (PaintBlasterItem)heldItem.modItem;

				myblaster.CheckMenu();
				myblaster.CheckSettings( this.player );
			}
		}
	}
}
