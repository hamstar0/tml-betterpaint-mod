using System;


namespace BetterPaint {
	public static partial class MagicPaintAPI {
		public static BetterPaintConfigData GetModSettings() {
			return BetterPaintMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			BetterPaintMod.Instance.JsonConfig.SaveFile();
		}


		// TODO Set RGB for current blaster
	}
}
