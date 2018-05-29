using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
	public enum PaintModeType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}



	public abstract class PaintMode {
		public abstract float Paint( PaintData data, Color color, int brush_size, int world_x, int world_y );
	}
}
