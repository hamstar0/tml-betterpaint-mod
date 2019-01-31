using System;


namespace BetterPaint {
	public static partial class MagicPaintAPI {
		internal static object Call( string callType, params object[] args ) {
			switch( callType ) {
			case "GetModSettings":
				return MagicPaintAPI.GetModSettings();
			case "SaveModSettingsChanges":
				MagicPaintAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + callType );
		}
	}
}
