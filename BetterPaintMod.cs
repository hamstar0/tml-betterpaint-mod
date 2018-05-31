using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.Utilities.Config;
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
			if( !BetterPaintMod.Instance.JsonConfig.LoadFile() ) {
				BetterPaintMod.Instance.JsonConfig.SaveFile();
			}
		}


		////////////////

		public JsonConfig<BetterPaintConfigData> JsonConfig { get; private set; }
		public BetterPaintConfigData Config { get { return this.JsonConfig.Data; } }

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

			this.JsonConfig = new JsonConfig<BetterPaintConfigData>( BetterPaintConfigData.ConfigFileName,
					ConfigurationDataBase.RelativePath, new BetterPaintConfigData() );
		}

		////////////////

		public override void Load() {
			BetterPaintMod.Instance = this;

			this.LoadConfig();
		}

		private void LoadConfig() {
			if( !this.JsonConfig.LoadFile() ) {
				this.JsonConfig.SaveFile();
				ErrorLogger.Log( "Better Paint config " + BetterPaintConfigData.ConfigVersion.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Better Paint updated to " + BetterPaintConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
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

				if( myitem.IsModeSelecting ) {
					myitem.DrawPainterUI( Main.spriteBatch );
				}

				myitem.DrawHUD( Main.spriteBatch );

				return true;
			};

			var interface_layer = new LegacyGameInterfaceLayer( "BetterPaint: Paint Blaster UI", func, InterfaceScaleType.UI );

			layers.Insert( idx, interface_layer );
		}
	}
}
