using Microsoft.Xna.Framework;


namespace BetterPaint.Painting {
	public enum PaintModeType : int {
		Stream,
		Spray,
		Fill,
		Erase
	}



	public abstract class PaintMode {
		public abstract int Paint( PaintData data, Color color, int brush_size, int world_x, int world_y );
		public abstract double PaintAt( PaintData data, Color color, int brush_size, double dist, ushort tile_x, ushort tile_y );
	}
}
