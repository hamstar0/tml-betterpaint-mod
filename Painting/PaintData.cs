using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	public class PaintData {
		public IDictionary<ushort, IDictionary<ushort, Color>> Colors { get; private set; }


		////////////////

		public PaintData() {
			this.Colors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}


		////////////////
		
		public void Load( TagCompound tags, string prefix ) {
			this.Colors.Clear();

			if( tags.ContainsKey( prefix + "_x" ) ) {
				int[] fg_x = tags.GetIntArray( prefix + "_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( prefix + "_" + x + "_y" );

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort y = (ushort)fg_y[j];

						byte[] clr_arr = tags.GetByteArray( prefix + "_" + x + "_" + y );
						Color color = new Color( clr_arr[0], clr_arr[1], clr_arr[2], clr_arr[3] );

						this.SetColorAt( color, x, y );
					}
				}
			}
		}


		public void Save( TagCompound tags, string prefix ) {
			int[] x_arr = this.Colors.Keys.Select( i => (int)i ).ToArray();

			tags.Set( prefix + "_x", x_arr );

			foreach( var kv in this.Colors ) {
				ushort x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;
				int[] y_arr = y_col.Keys.Select( i => (int)i ).ToArray();

				tags.Set( prefix + "_" + x + "_y", y_arr );

				foreach( var kv2 in y_col ) {
					ushort y = kv2.Key;
					Color clr = kv2.Value;
					byte[] clr_bytes = new byte[] { clr.R, clr.G, clr.B, clr.A };

					tags.Set( prefix + "_" + x + "_" + y, clr_bytes );
				}
			}
		}


		////////////////

		public bool HasColor( ushort x, ushort y ) {
			return this.Colors.ContainsKey( x ) && this.Colors[x].ContainsKey( y );
		}

		public Color GetColor( ushort x, ushort y ) {
			if( this.Colors.ContainsKey( x ) ) {
				if( this.Colors[x].ContainsKey( y ) ) {
					return this.Colors[x][y];
				}
			}
			return Color.White;
		}

		public void SetColorAt( Color color, ushort x, ushort y ) {
			if( !this.Colors.ContainsKey(x) ) {
				this.Colors[x] = new Dictionary<ushort, Color>();
			}
			this.Colors[x][y] = color;
		}
	}
}
