using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public IDictionary<ushort, IDictionary<ushort, Color>> Colors { get; private set; }
		public IDictionary<ushort, IDictionary<ushort, byte>> Glows { get; private set; }



		////////////////

		public PaintLayer() {
			this.Colors = new Dictionary<ushort, IDictionary<ushort, Color>>();
			this.Glows = new Dictionary<ushort, IDictionary<ushort, byte>>();

			LoadHooks.AddWorldLoadEachHook( () => {
				Main.OnPreDraw += this.RefreshTilesOnMap;
			} );
			LoadHooks.AddWorldUnloadEachHook( () => {
				Main.OnPreDraw -= this.RefreshTilesOnMap;
				this.ClearTilesOnMap();
			} );
		}


		////////////////
		
		internal void Load( BetterPaintMod mymod, TagCompound tags, string prefix ) {
			var myworld = ModContent.GetInstance<BetterPaintWorld>();

			this.Colors.Clear();
			this.Glows.Clear();

			if( tags.ContainsKey( prefix + "_x" ) ) {
				int[] fgX = tags.GetIntArray( prefix + "_x" );

				for( int i = 0; i < fgX.Length; i++ ) {
					ushort tileX = (ushort)fgX[i];
					int[] fgY = tags.GetIntArray( prefix + "_" + tileX + "_y" );

					for( int j = 0; j < fgY.Length; j++ ) {
						ushort tileY = (ushort)fgY[j];

						byte[] clrArr = tags.GetByteArray( prefix + "_" + tileX + "_" + tileY );
						Color color = new Color( clrArr[0], clrArr[1], clrArr[2], clrArr[3] );

						Tile tile = Main.tile[tileX, tileY];

						if( this.CanPaintAt(tile) ) {
							this.SetRawColorAt( color, tileX, tileY );
						}
					}
				}
			}

			if( tags.ContainsKey( prefix + "_g_x" ) ) {
				int[] fgX = tags.GetIntArray( prefix + "_g_x" );

				for( int i = 0; i < fgX.Length; i++ ) {
					ushort tileX = (ushort)fgX[i];
					int[] fgY = tags.GetIntArray( prefix + "_g_" + tileX + "_y" );

					for( int j = 0; j < fgY.Length; j++ ) {
						ushort tileY = (ushort)fgY[j];

						byte glow = tags.GetByte( prefix + "_g_" + tileX + "_" + tileY );

						Tile tile = Main.tile[tileX, tileY];

						if( this.CanPaintAt( tile ) ) {
							this.SetGlowAt( glow, tileX, tileY );
						}
					}
				}
			}
		}


		internal void Save( TagCompound tags, string prefix ) {
			int[] clrArrX = this.Colors.Keys.Select( i => (int)i ).ToArray();
			int[] glowArrX = this.Glows.Keys.Select( i => (int)i ).ToArray();

			tags.Set( prefix + "_x", clrArrX );

			foreach( var kv in this.Colors ) {
				ushort tileX = kv.Key;
				IDictionary<ushort, Color> yCol = kv.Value;
				int[] yArr = yCol.Keys.Select( i => (int)i ).ToArray();

				tags.Set( prefix + "_" + tileX + "_y", yArr );

				foreach( var kv2 in yCol ) {
					ushort tileY = kv2.Key;
					Color clr = kv2.Value;
					byte[] clrBytes = new byte[] { clr.R, clr.G, clr.B, clr.A };

					tags.Set( prefix + "_" + tileX + "_" + tileY, clrBytes );
				}
			}
			
			tags.Set( prefix + "_g_x", glowArrX );

			foreach( var kv in this.Glows ) {
				ushort tileX = kv.Key;
				IDictionary<ushort, byte> yCol = kv.Value;
				int[] yArr = yCol.Keys.Select( i => (int)i ).ToArray();

				tags.Set( prefix + "_g_" + tileX + "_y", yArr );

				foreach( var kv2 in yCol ) {
					ushort tileY = kv2.Key;
					byte glow = kv2.Value;

					tags.Set( prefix + "_g_" + tileX + "_" + tileY, glow );
				}
			}
		}
	}
}
