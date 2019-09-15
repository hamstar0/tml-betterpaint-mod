using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Map;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public void SetMapColorAt( Color color, ushort tileX, ushort tileY ) {
			MapTile mapTile = Main.Map[tileX, tileY];
			//Color tile_color = MapHelper.GetMapTileXnaColor( ref map_tile );
			//Color lerped_color = Color.Lerp( tile_color, color, (float)color.A / 255f );

			mapTile.Color = HamstarHelpers.Helpers.Misc.PaintHelpers.GetNearestPaintType( color );

			Main.Map.SetTile( tileX, tileY, ref mapTile );
		}
		
		public void RemoveMapColorAt( ushort tileX, ushort tileY ) {
			MapTile mapTile = Main.Map[tileX, tileY];

			mapTile.Color = Main.tile[tileX, tileY].color();

			Main.Map.SetTile( tileX, tileY, ref mapTile );
		}


		////////////////

		private void RefreshTilesOnMap( GameTime gameTime ) {
			if( !BetterPaintMod.Instance.Config.ShowPaintOnMap ) { return; }

			//if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
			//	if( Timers.GetTimerTickDuration( "BetterPaint: Minimap Refresh" ) > 0 ) { return; }
			//	Timers.SetTimer( "BetterPaint: Minimap Refresh", 5, () => { return false; } );
			//}

			foreach( ushort i in this.Colors.Keys ) {
				foreach( ushort j in this.Colors[i].Keys ) {
					MapTile mapTile = Main.Map[i, j];

					if( mapTile.IsChanged ) {
						this.SetMapColorAt( this.Colors[i][j], i, j );
					}
				}
			}
		}


		////////////////

		private void ClearTilesOnMap() {
			foreach( ushort i in this.Colors.Keys ) {
				foreach( ushort j in this.Colors[i].Keys ) {
					MapTile mapTile = Main.Map[i, j];

					if( mapTile.IsChanged ) {
						mapTile.Clear();
						Main.Map.SetTile( i, j, ref mapTile );
					}
				}
			}
		}
	}
}
