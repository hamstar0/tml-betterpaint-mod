using System;


namespace BetterPaint {
	public static partial class MagicPaintAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return MagicPaintAPI.GetModSettings();
			case "SaveModSettingsChanges":
				MagicPaintAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
