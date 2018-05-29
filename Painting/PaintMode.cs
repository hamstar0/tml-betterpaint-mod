using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
	public enum PaintModeType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}



	public abstract class PaintMode {
		public abstract float Apply( PaintData data, Color color, int brush_size, float pressure, int rand_seed, int world_x, int world_y );
	}
}
