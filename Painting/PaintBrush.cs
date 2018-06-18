using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
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
			Color diff_color = XnaColorHelpers.DifferenceRGBA( old_color, new_color );
			float diff_value = XnaColorHelpers.AvgRGBA( diff_color );

			return diff_value / 255f;
		}


		////////////////

		public abstract float Apply( PaintLayer data, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y );
	}
}
