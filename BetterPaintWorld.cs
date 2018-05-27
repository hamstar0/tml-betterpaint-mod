using BetterPaint.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public IDictionary<ushort, IDictionary<ushort, Color>> FgColors { get; private set; }
		public IDictionary<ushort, IDictionary<ushort, Color>> BgColors { get; private set; }


		public override void Initialize() {
			this.FgColors = new Dictionary<ushort, IDictionary<ushort, Color>>();
			this.BgColors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}

		public override void Load( TagCompound tags ) {
			this.FgColors.Clear();
			this.BgColors.Clear();

			if( tags.ContainsKey( "fg_x" ) ) {
				int[] fg_x = tags.GetIntArray( "fg_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( "fg_" + x + "_y" );

					this.FgColors[x] = new Dictionary<ushort, Color>();

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort y = (ushort)fg_y[j];

						byte[] clr = tags.GetByteArray( "fg_" + x + "_" + y );
						this.FgColors[x][y] = new Color( clr[0], clr[1], clr[2], clr[3] );
					}
				}
			}

			if( tags.ContainsKey( "bg_x" ) ) {
				int[] bg_x = tags.GetIntArray( "bg_x" );

				for( int i = 0; i < bg_x.Length; i++ ) {
					ushort x = (ushort)bg_x[i];
					int[] bg_y = tags.GetIntArray( "bg_" + x + "_y" );

					this.BgColors[x] = new Dictionary<ushort, Color>();

					for( int j = 0; j < bg_y.Length; j++ ) {
						ushort y = (ushort)bg_y[j];

						byte[] clr = tags.GetByteArray( "bg_" + x + "_" + y );
						this.BgColors[x][y] = new Color( clr[0], clr[1], clr[2], clr[3] );
					}
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound();

			tags["fg_x"] = this.FgColors.Keys.ToArray();

			foreach( var kv in this.FgColors ) {
				ushort x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;

				tags["fg_" + x + "_y"] = y_col.Keys.ToArray();

				foreach( var kv2 in y_col ) {
					ushort y = kv2.Key;
					Color clr = kv2.Value;

					tags["fg_" + x + "_" + y] = new byte[] { clr.R, clr.G, clr.B, clr.A };
				}
			}

			tags["bg_x"] = this.BgColors.Keys.ToArray();

			foreach( var kv in this.BgColors ) {
				ushort x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;

				tags["bg_" + x + "_y"] = y_col.Keys.ToArray();

				foreach( var kv2 in y_col ) {
					ushort y = kv2.Key;
					Color clr = kv2.Value;

					tags["bg_" + x + "_" + y] = new byte[] { clr.R, clr.G, clr.B, clr.A };
				}
			}

			return tags;
		}
		


		////////////////

		public int AddForegroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( !this.FgColors.ContainsKey( x ) ) {
				this.FgColors[x] = new Dictionary<ushort, Color>();
			}
			this.FgColors[x][y] = color;
			return 1;
		}


		public int AddBackgroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( !this.BgColors.ContainsKey( x ) ) {
				this.BgColors[x] = new Dictionary<ushort, Color>();
			}
			this.BgColors[x][y] = color;
			return 1;
		}


		public bool HasFgColor( Color color, int i, int j ) {
			ushort x = (ushort)i;
			ushort y = (ushort)j;

			if( !this.FgColors.ContainsKey(x) ) { return false; }
			if( !this.FgColors[x].ContainsKey( y ) ) { return false; }
			return this.FgColors[x][y] == color;
		}

		public bool HasBgColor( Color color, int i, int j ) {
			ushort x = (ushort)i;
			ushort y = (ushort)j;

			if( !this.BgColors.ContainsKey( x ) ) { return false; }
			if( !this.BgColors[x].ContainsKey( y ) ) { return false; }
			return this.BgColors[x][y] == color;
		}
	}
}
