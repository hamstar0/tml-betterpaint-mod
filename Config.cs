using HamstarHelpers.Components.Config;
using System;


namespace BetterPaint {
	public class BetterPaintConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 2, 2 );
		public readonly static string ConfigFileName = "Better Paint Config.json";


		////////////////

		public string VersionSinceUpdate = BetterPaintConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		public bool DebugModeCheats = false;

		public bool PaintMixerRecipeEnabled = true;
		public bool PaintMixerRecipeBlendOMatic = true;

		public bool PaintRecipeEnabled = true;
		public int PaintRecipeGels = 10;
		public int PaintRecipePaints = 100;
		public int PaintCartridgeCapacity = 2000;

		public bool CopyPaintRecipeEnabled = true;
		public int CopyPaintRecipeManaPotions = 10;
		public int CopyPaintRecipeNanites = 5;

		public bool GlowPaintRecipeEnabled = true;
		public int GlowPaintRecipeSpores = 5;

		public bool PaintBlasterRecipeEnabled = true;
		public bool PaintBlasterRecipeClentaminator = true;

		public float BrushSizeMultiplier = 1;

		public float BrushSpatterDensity = 0.15f;

		public int HudPaintAmmoOffsetX = -32;
		public int HudPaintAmmoOffsetY = -160;

		public bool PainterSellsBlaster = false;
		public bool PainterSellsRGBCartridges = true;
		public bool PainterSellsCopyCartridge = false;
		public bool PainterSellsPaintMixer = false;

		public bool ShowPaintOnMap = true;


		////////////////

		public bool UpdateToLatestVersion() {
			var newConfig = new BetterPaintConfigData();
			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= BetterPaintConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = BetterPaintConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
