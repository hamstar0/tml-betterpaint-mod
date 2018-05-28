using BetterPaint.Items;
using BetterPaint.NetProtocols;
using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public PaintData FgColors { get; private set; }
		public PaintData BgColors { get; private set; }


		////////////////

		public override void Initialize() {
			this.FgColors = new PaintData();
			this.BgColors = new PaintData();
		}

		////////////////

		private void LoadLayer( TagCompound tags, string prefix, PaintData storage ) {
			storage.Clear();

			if( tags.ContainsKey( prefix+"_x" ) ) {
				int[] fg_x = tags.GetIntArray( prefix+"_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( prefix+"_" + x + "_y" );

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort y = (ushort)fg_y[j];

						byte[] clr_arr = tags.GetByteArray( prefix+"_" + x + "_" + y );
						Color color = new Color( clr_arr[0], clr_arr[1], clr_arr[2], clr_arr[3] );

						storage.ColorAt( color, x, y );
					}
				}
			}
		}

		public override void Load( TagCompound tags ) {
			this.LoadLayer( tags, "bg", this.BgColors );
			this.LoadLayer( tags, "fg", this.FgColors );
		}


		private void SaveLayer( TagCompound tags, string prefix, PaintData data ) {
			IDictionary<ushort, IDictionary<ushort, Color>> raw_data = data.Colors;
			tags[prefix+"_x"] = raw_data.Keys.ToArray();

			foreach( var kv in raw_data ) {
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

		////

		public int AddForegroundColorNoSync( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			switch( mode ) {
			case PaintMode.Stream:
				StreamBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Spray:
				SprayBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Flood:
				FloodBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Erase:
				EraserBrush.Paint( this.FgColors, color, size, x, y );
				break;
			}
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
