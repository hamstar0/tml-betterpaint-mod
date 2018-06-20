using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Helpers.XnaHelpers {
	class XnaColorHelpers {
		public static Color DifferenceRGB( Color c1, Color c2 ) {
			return new Color(
				Math.Abs( (int)c1.R - (int)c2.R ),
				Math.Abs( (int)c1.G - (int)c2.G ),
				Math.Abs( (int)c1.B - (int)c2.B )
			);
		}
		
		public static int SumRGB( Color c ) {
			return (int)c.R + c.G + (int)c.B;
		}
		
		public static float AvgRGB( Color c ) {
			return (float)XnaColorHelpers.SumRGB( c ) / 3f;
		}

		public static Color GetWhiteBase( Color c ) {
			byte space = c.R > c.G ?
				(c.R > c.B ? c.R : c.B) :
				(c.G > c.B ? c.G : c.B);
			return new Color( c.R + 255 - space, c.G + 255 - space, c.B + 255 - space, c.A );
		}
	}
}
