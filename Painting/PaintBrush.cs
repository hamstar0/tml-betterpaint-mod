using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
	public enum PaintBrushType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}


	public enum PaintMode : int {
		Foreground,
		Background,
		Anyground
	}



	public abstract class PaintBrush {
		public abstract float Apply( PaintData data, Color color, bool brush_size_small, float pressure, int rand_seed, int world_x, int world_y );
	}
}
