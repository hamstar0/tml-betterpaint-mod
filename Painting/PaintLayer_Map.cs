using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Map;


namespace BetterPaint.Painting {
	public abstract partial class PaintLayer {
		public void SetMapColorAt( Color color, ushort tile_x, ushort tile_y ) {
			MapTile map_tile = Main.Map[tile_x, tile_y];
			//Color tile_color = MapHelper.GetMapTileXnaColor( ref map_tile );
			//Color lerped_color = Color.Lerp( tile_color, color, (float)color.A / 255f );

			map_tile.Color = HamstarHelpers.Helpers.MiscHelpers.PaintHelpers.GetNearestPaintType( color );

			Main.Map.SetTile( tile_x, tile_y, ref map_tile );
		}
		
		public void RemoveMapColorAt( ushort tile_x, ushort tile_y ) {
			MapTile map_tile = Main.Map[tile_x, tile_y];

			map_tile.Color = Main.tile[tile_x, tile_y].color();

			Main.Map.SetTile( tile_x, tile_y, ref map_tile );
		}


		////////////////

		private void RefreshTilesOnMap( GameTime game_time ) {
			if( !BetterPaintMod.Instance.Config.ShowPaintOnMap ) { return; }

			//if( LoadHelpers.IsWorldSafelyBeingPlayed() ) {
			//	if( Timers.GetTimerTickDuration( "BetterPaint: Minimap Refresh" ) > 0 ) { return; }
			//	Timers.SetTimer( "BetterPaint: Minimap Refresh", 5, () => { return false; } );
			//}

			foreach( ushort i in this.Colors.Keys ) {
				foreach( ushort j in this.Colors[i].Keys ) {
					if( Main.Map[i, j].IsChanged ) {
						this.SetMapColorAt( this.Colors[i][j], i, j );
					}
				}
			}
		}
	}
}
