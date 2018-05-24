﻿using HamstarHelpers.Utilities.Config;
using System;


namespace BetterPaint {
	public class BetterPaintConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 0, 0 );
		public readonly static string ConfigFileName = "Better Paint Config.json";


		////////////////

		public string VersionSinceUpdate = BetterPaintConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		public bool DebugModeCheats = false;

		public bool PaintRecipeEnabled = true;
		public int PaintRecipeNaniteIngredientQuantity = 5;
		public int PaintRecipePaintIngredientQuantity = 25;
		public int PaintCartridgeCapacity = 100;


		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new BetterPaintConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= BetterPaintConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = BetterPaintConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}