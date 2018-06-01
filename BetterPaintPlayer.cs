using BetterPaint.Items;
using BetterPaint.NetProtocols;
using HamstarHelpers.Utilities.Messages;
using HamstarHelpers.Utilities.Network;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	partial class BetterPaintPlayer : ModPlayer {
		public override void OnEnterWorld( Player player ) {
			var mymod = (BetterPaintMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.JsonConfig.LoadFile() ) {
					mymod.JsonConfig.SaveFile();
				}
			} else if( Main.netMode == 1 ) {
				PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			}

			string intro = "Eager to try out better painting? You'll need a Paint Blaster, crafted via Clentaminator, Flamethrower, and Paint Sprayer at a Tinkerer's Workshop. To make blaster paint, you'll need a Paint Mixer, crafted via Dye Vat" + ( mymod.Config.PaintMixerRecipeBlendOMatic ? ", Blend-O-Matic, " : " " ) + "and Extractinator at a Tinkerer's Workshop. To paint, you'll need Color Cartridges, crafted via colored Paints (any " + mymod.Config.PaintRecipePaints + ") and Gel (" + mymod.Config.PaintRecipeGels + ") at a Paint Mixer.";
			string post_intro = "Use the Control Panel (single player only) to configure settings, including whether the Painter NPC should sell Better Paint items, if crafting isn't your cup of tea.";
			string pander = "If you enjoy this mod and want to see more, please give your support at: https://www.patreon.com/hamstar0";

			InboxMessages.SetMessage( "BetterPaintIntro", intro, false );
			InboxMessages.SetMessage( "BetterPaintPostIntro", post_intro, false );
			InboxMessages.SetMessage( "BetterPaintPander", pander, false );
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
