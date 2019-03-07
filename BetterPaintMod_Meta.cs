using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
    partial class BetterPaintMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-betterpaint-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + BetterPaintConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reload configs outside of single player." );
			}
			if( !BetterPaintMod.Instance.ConfigJson.LoadFile() ) {
				BetterPaintMod.Instance.ConfigJson.SaveFile();
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reset to default configs outside of single player." );
			}
			BetterPaintMod.Instance.ConfigJson.SetData( new BetterPaintConfigData() );
			BetterPaintMod.Instance.ConfigJson.SaveFile();
		}
	}
}
