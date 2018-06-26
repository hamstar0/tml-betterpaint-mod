using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting.Brushes {
	public enum PaintLayerType : int {
		Foreground,
		Background,
		Anyground
	}

	
	public enum PaintBrushType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}


	public enum PaintBrushSize : int {
		Small,
		Large
	}



	public abstract class PaintBrush {
		public static float ComputeChangePercent( Color old_color, Color new_color, byte old_glow, byte new_glow ) {
			return PaintBrush.ComputeColorChangePercent( old_color, new_color ) +
				(PaintBrush.ComputeGlowChangePercent( old_glow, new_glow ) / 4);
		}

		public static float ComputeColorChangePercent( Color old_color, Color new_color ) {
			Color diff_color = XnaColorHelpers.DifferenceRGB( old_color, new_color );
			float diff_value = XnaColorHelpers.AvgRGB( diff_color );

			return diff_value / 255f;
		}

		public static float ComputeGlowChangePercent( byte old_glow, byte new_glow ) {
			int diff = Math.Abs( new_glow - old_glow );

			return (float)diff / 255f;
		}


		////////////////

		public abstract float Apply( PaintLayer data, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y );
	}
}
