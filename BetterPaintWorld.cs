using BetterPaint.Items;
using BetterPaint.NetProtocols;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public IDictionary<ushort, IDictionary<ushort, Color>> FgColors { get; private set; }
		public IDictionary<ushort, IDictionary<ushort, Color>> BgColors { get; private set; }


		////////////////

		public override void Initialize() {
			this.FgColors = new Dictionary<ushort, IDictionary<ushort, Color>>();
			this.BgColors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}

		////////////////

		private void LoadLayer( TagCompound tags, string prefix, IDictionary<ushort, IDictionary<ushort, Color>> storage ) {
			storage.Clear();

			if( tags.ContainsKey( prefix+"_x" ) ) {
				int[] fg_x = tags.GetIntArray( prefix+"_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( prefix+"_" + x + "_y" );

					storage[x] = new Dictionary<ushort, Color>();

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort y = (ushort)fg_y[j];

						byte[] clr = tags.GetByteArray( prefix+"_" + x + "_" + y );
						storage[x][y] = new Color( clr[0], clr[1], clr[2], clr[3] );
					}
				}
			}
		}

		public override void Load( TagCompound tags ) {
			this.LoadLayer( tags, "bg", this.BgColors );
			this.LoadLayer( tags, "fg", this.FgColors );
		}


		private void SaveLayer( TagCompound tags, string prefix, IDictionary<ushort, IDictionary<ushort, Color>> data ) {
			tags[prefix+"_x"] = data.Keys.ToArray();

			foreach( var kv in data ) {
				ushort x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;

				tags[prefix+"_" + x + "_y"] = y_col.Keys.ToArray();

				foreach( var kv2 in y_col ) {
					ushort y = kv2.Key;
					Color clr = kv2.Value;

					tags[prefix+"_" + x + "_" + y] = new byte[] { clr.R, clr.G, clr.B, clr.A };
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound();

			this.SaveLayer( tags, "fg", this.FgColors );
			this.SaveLayer( tags, "bg", this.BgColors );

			return tags;
		}


		////////////////

		public int AddForegroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int paints_used = this.AddForegroundColorNoSync( color, size, mode, x, y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( true, color, size, mode, x, y );
			}

			return paints_used;
		}

		public int AddBackgroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int paints_used = this.AddBackgroundColorNoSync( color, size, mode, x, y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( false, color, size, mode, x, y );
			}

			return paints_used;
		}


		public int AddForegroundColorNoSync( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( !this.FgColors.ContainsKey( x ) ) {
				this.FgColors[x] = new Dictionary<ushort, Color>();
			}
			this.FgColors[x][y] = color;

			return 1;
		}
		
		public int AddBackgroundColorNoSync( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( !this.BgColors.ContainsKey( x ) ) {
				this.BgColors[x] = new Dictionary<ushort, Color>();
			}

			this.BgColors[x][y] = color;

			return 1;
		}


		////////////////

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
