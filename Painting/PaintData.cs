using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace BetterPaint.Painting {
	public class PaintData {
		public IDictionary<ushort, IDictionary<ushort, Color>> Colors { get; private set; }


		////////////////

		public PaintData() {
			this.Colors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}


		////////////////

		public void Clear() {
			this.Colors.Clear();
		}

		////////////////

		public bool HasColor( ushort x, ushort y ) {
			return !this.Colors.ContainsKey( x ) || !this.Colors[x].ContainsKey( y );
		}

		public void ColorAt( Color color, ushort x, ushort y ) {
			if( !this.Colors.ContainsKey(x) ) {
				this.Colors[x] = new Dictionary<ushort, Color>();
			}
			this.Colors[x][y] = color;
		}
	}
}
