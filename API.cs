using System;


namespace BetterPaint {
	public static partial class BetterPaintAPI {
		public static BetterPaintConfigData GetModSettings() {
			return BetterPaintMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			BetterPaintMod.Instance.ConfigJson.SaveFile();
		}


		// TODO: Apply specific brush stroke

		// TODO: Reset all paint
	}
}
