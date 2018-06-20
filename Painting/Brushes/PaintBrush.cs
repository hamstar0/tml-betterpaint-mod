using BetterPaint.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;


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
		public static float ComputeColorChangePercent( Color old_color, Color new_color ) {
			Color diff_color = XnaColorHelpers.DifferenceRGB( old_color, new_color );
			float diff_value = XnaColorHelpers.AvgRGB( diff_color );

			return diff_value / 255f;
		}


		////////////////

		public abstract float Apply( PaintLayer data, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y );
	}
}
