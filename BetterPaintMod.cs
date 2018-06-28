using BetterPaint.Items;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.Components.Config;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace BetterPaint {
    public class BetterPaintMod : Mod {
		public static BetterPaintMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-betterpaint-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + BetterPaintConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !BetterPaintMod.Instance.ConfigJson.LoadFile() ) {
				BetterPaintMod.Instance.ConfigJson.SaveFile();
			}
		}
		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}
			BetterPaintMod.Instance.ConfigJson.SetData( new BetterPaintConfigData() );
			BetterPaintMod.Instance.ConfigJson.SaveFile();
		}


		////////////////

		public JsonConfig<BetterPaintConfigData> ConfigJson { get; private set; }
		public BetterPaintConfigData Config { get { return this.ConfigJson.Data; } }

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

			this.ConfigJson = new JsonConfig<BetterPaintConfigData>( BetterPaintConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new BetterPaintConfigData() );
		}

		////////////////

		public override void Load() {
			BetterPaintMod.Instance = this;

			this.LoadConfig();

			Promises.AddPostWorldLoadEachPromise( () => {
				string intro1 = "1 of 5 - Eager to try out better painting? You'll need a Paint Blaster, crafted via " + (this.Config.PaintBlasterRecipeClentaminator ? "Clentaminator" : "Illegal Gun Parts") + " and Paint Sprayer at a Tinkerer's Workshop.";
				string intro2 = "2 of 5 - To make paint, you'll need a Paint Mixer, crafted via Dye Vat" + ( this.Config.PaintMixerRecipeBlendOMatic ? ", Blend-O-Matic, " : " " ) + "and Extractinator at a Tinkerer's Workshop.";
				string intro3 = "3 of 5 - To paint, you'll need Color Cartridges, crafted via colored Paints (any " + this.Config.PaintRecipePaints + ") and Gel (" + this.Config.PaintRecipeGels + ") at a Paint Mixer.";
				string post_intro = "4 of 5 - Use the Control Panel (single player only) to configure settings, including whether the Painter NPC should sell Better Paint items, if crafting isn't your cup of tea.";
				string pander = "5 of 5 - If you enjoy this mod and want to see more, please give your support at: https://www.patreon.com/hamstar0";

				InboxMessages.SetMessage( "BetterPaintIntro1", intro1, false );
				InboxMessages.SetMessage( "BetterPaintIntro2", intro2, false );
				InboxMessages.SetMessage( "BetterPaintIntro3", intro3, false );
				InboxMessages.SetMessage( "BetterPaintPostIntro", post_intro, false );
				InboxMessages.SetMessage( "BetterPaintPander", pander, false );

				if( Main.netMode != 0 ) {
					InboxMessages.SetMessage( "BetterPaintNoMuliYet", "Better Paint (as of v1.2.0) is not yet compatible with multiplayer.", true );
				}
			} );
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Better Paint config " + BetterPaintConfigData.ConfigVersion.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Better Paint updated to " + BetterPaintConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			BetterPaintMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return MagicPaintAPI.Call( call_type, new_args );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Inventory" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod func = delegate {
				Player player = Main.LocalPlayer;
				Item held_item = player.HeldItem;

				if( held_item == null || held_item.IsAir || held_item.type != this.ItemType<PaintBlasterItem>() ) {
					return true;
				}

				var myitem = (PaintBlasterItem)held_item.modItem;

				if( myitem.IsUsingUI ) {
					myitem.DrawPainterUI( Main.spriteBatch );
				}
				myitem.DrawScreen( Main.spriteBatch );
				myitem.DrawHUD( Main.spriteBatch );

				return true;
			};

			var interface_layer = new LegacyGameInterfaceLayer( "BetterPaint: Paint Blaster UI", func, InterfaceScaleType.UI );

			layers.Insert( idx, interface_layer );
		}
	}
}
