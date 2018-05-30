using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
	public enum PaintLayer : int {
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
		public abstract float Apply( PaintData data, Color color, PaintBrushSize brush_size, float pressure, int rand_seed, int world_x, int world_y );
	}
}
