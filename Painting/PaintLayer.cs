using BetterPaint.Helpers.XnaHelpers;
using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public IDictionary<ushort, IDictionary<ushort, Color>> Colors { get; private set; }


		////////////////

		public PaintLayer() {
			this.Colors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}


		////////////////
		
		public void Load( BetterPaintMod mymod, TagCompound tags, string prefix ) {
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			this.Colors.Clear();

			if( tags.ContainsKey( prefix + "_x" ) ) {
				int[] fg_x = tags.GetIntArray( prefix + "_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort tile_x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( prefix + "_" + tile_x + "_y" );

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort tile_y = (ushort)fg_y[j];

						byte[] clr_arr = tags.GetByteArray( prefix + "_" + tile_x + "_" + tile_y );
						Color color = new Color( clr_arr[0], clr_arr[1], clr_arr[2], clr_arr[3] );

						Tile tile = Main.tile[tile_x, tile_y];

						if( this.CanPaintAt(tile) ) {
							this.SetColorAt( color, tile_x, tile_y );
						}
					}
				}
			}
		}


		public void Save( TagCompound tags, string prefix ) {
			int[] clr_x_arr = this.Colors.Keys.Select( i => (int)i ).ToArray();

			tags.Set( prefix + "_x", clr_x_arr );

			foreach( var kv in this.Colors ) {
				ushort tile_x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;
				int[] y_arr = y_col.Keys.Select( i => (int)i ).ToArray();

				tags.Set( prefix + "_" + tile_x + "_y", y_arr );

				foreach( var kv2 in y_col ) {
					ushort tile_y = kv2.Key;
					Color clr = kv2.Value;
					byte[] clr_bytes = new byte[] { clr.R, clr.G, clr.B, clr.A };

					tags.Set( prefix + "_" + tile_x + "_" + tile_y, clr_bytes );
				}
			}
		}
	}
}
