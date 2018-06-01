using HamstarHelpers.Utilities.Config;
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
		public int PaintRecipeGelIngredientQuantity = 10;
		public int PaintRecipePaintIngredientQuantity = 100;
		public int PaintCartridgeCapacity = 2000;

		public int CopyPaintManaPotionIngredientQuantity = 10;
		public int CopyPaintNaniteIngredientQuantity = 5;

		public float BrushSizeMultiplier = 1;

		public float BrushSpatterDensity = 0.15f;

		public int HudPaintAmmoOffsetX = -32;
		public int HudPaintAmmoOffsetY = -160;

		public bool PainterSellsBlaster = false;
		public bool PainterSellsRGBCartridges = true;
		public bool PainterSellsCopyCartridge = false;
		public bool PainterSellsPaintMixer = false;


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
