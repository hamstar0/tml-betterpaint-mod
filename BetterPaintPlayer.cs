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

			Item held_item = this.player.HeldItem;

			if( held_item != null && !held_item.IsAir && held_item.type == this.mod.ItemType<PaintBlasterItem>() ) {
				var myblaster = (PaintBlasterItem)held_item.modItem;

				myblaster.CheckMenu();
				myblaster.CheckSettings( this.player );
			}
		}
	}
}
