﻿using BetterPaint.Items;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.Helpers.TModLoader.Mods;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Messages.Inbox;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace BetterPaint {
    partial class BetterPaintMod : Mod {
		public static BetterPaintMod Instance { get; private set; }



		////////////////

		public BetterPaintConfig Config => ModContent.GetInstance<BetterPaintConfig>();

		public IDictionary<PaintBrushType, PaintBrush> Modes { get; private set; }


		////////////////

		public BetterPaintMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.Modes = new Dictionary<PaintBrushType, PaintBrush> {
				{ PaintBrushType.Stream, new PaintBrushStream() },
				{ PaintBrushType.Spray, new PaintBrushSpray() },
				{ PaintBrushType.Spatter, new PaintBrushSpatter() },
				{ PaintBrushType.Erase, new PaintBrushErase() }
			};
		}

		////////////////

		public override void Load() {
			BetterPaintMod.Instance = this;

			LoadHooks.AddPostWorldLoadEachHook( () => {
				string intro1 = "1 of 5 - Eager to try out better painting? You'll need a Paint Blaster, crafted via " + (this.Config.PaintBlasterRecipeClentaminator ? "Clentaminator" : "Illegal Gun Parts") + " and Paint Sprayer at a Tinkerer's Workshop.";
				string intro2 = "2 of 5 - To make paint, you'll need a Paint Mixer, crafted via Dye Vat" + ( this.Config.PaintMixerRecipeBlendOMatic ? ", Blend-O-Matic, " : " " ) + "and Extractinator at a Tinkerer's Workshop.";
				string intro3 = "3 of 5 - To paint, you'll need Color Cartridges, crafted via colored Paints (any " + this.Config.PaintRecipePaints + ") and Gel (" + this.Config.PaintRecipeGels + ") at a Paint Mixer.";
				string postIntro = "4 of 5 - Use the Control Panel (single player only) to configure settings, including whether the Painter NPC should sell Better Paint items, if crafting isn't your cup of tea.";
				string pander = "5 of 5 - If you enjoy this mod and want to see more, please give your support at: https://www.patreon.com/hamstar0";

				InboxMessages.SetMessage( "BetterPaintIntro1", intro1, false );
				InboxMessages.SetMessage( "BetterPaintIntro2", intro2, false );
				InboxMessages.SetMessage( "BetterPaintIntro3", intro3, false );
				InboxMessages.SetMessage( "BetterPaintPostIntro", postIntro, false );
				InboxMessages.SetMessage( "BetterPaintPander", pander, false );

				if( Main.netMode != 0 ) {
					InboxMessages.SetMessage( "BetterPaintNoMuliYet", "Better Paint (as of v1.2.0) is not yet compatible with multiplayer.", true );
				}
			} );
		}

		public override void Unload() {
			BetterPaintMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof(BetterPaintAPI), args );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Inventory" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod func = delegate {
				Player player = Main.LocalPlayer;
				Item heldItem = player.HeldItem;

				if( heldItem == null || heldItem.IsAir || heldItem.type != ModContent.ItemType<PaintBlasterItem>() ) {
					return true;
				}

				var myitem = (PaintBlasterItem)heldItem.modItem;

				if( myitem.IsUsingUI ) {
					myitem.DrawPainterUI( Main.spriteBatch );
				}
				myitem.DrawScreen( Main.spriteBatch );
				myitem.DrawHUD( Main.spriteBatch );

				return true;
			};

			var interfaceLayer = new LegacyGameInterfaceLayer( "BetterPaint: Paint Blaster UI", func, InterfaceScaleType.UI );

			layers.Insert( idx, interfaceLayer );
		}
	}
}
