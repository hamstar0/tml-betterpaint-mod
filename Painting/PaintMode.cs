using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting {
	public enum PaintModeType : int {
		Stream,
		Spray,
		Fill,
		Erase
	}



	public abstract class PaintMode {
		public abstract int Paint( PaintData data, Color color, int size, int x, int y );
		public abstract int PaintAt( PaintData data, Color color, double dist, int x, int y );
	}
}
