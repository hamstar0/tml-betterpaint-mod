using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintTile : GlobalTile {
		public override void DrawEffects( int i, int j, int type, SpriteBatch sb, ref Color draw_color, ref int next_special_draw_idx ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;
			
			if( myworld.Layers.Foreground.HasColorAt(tile_x, tile_y) ) {
				draw_color = myworld.Layers.Foreground.ComputeTileColor( tile_x, tile_y );
			}
		}


		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effect_only, ref bool no_item ) {
			if( effect_only ) { return; }

			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;

			if( myworld.Layers.Foreground.HasColorAt( tile_x, tile_y ) ) {
				myworld.Layers.Foreground.RemoveColorAt( tile_x, tile_y );
			}
		}
	}




	class BetterPaintWall : GlobalWall {
		public override bool PreDraw( int i, int j, int type, SpriteBatch sb ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;

			if( myworld.Layers.Background.HasColorAt( tile_x, tile_y ) ) {
				Color draw_color = myworld.Layers.Background.ComputeTileColor( tile_x, tile_y );

				Vector2 zero = new Vector2( (float)Main.offScreenRange, (float)Main.offScreenRange );
				if( Main.drawToScreen ) {
					zero = Vector2.Zero;
				}

				var pos = (new Vector2( (i * 16) - 8, (j * 16) - 8 ) - Main.screenPosition) + zero;

				Tile tile = Main.tile[i, j];
				int y_offset = (int)( Main.wallFrame[type] * 180 );
				var frame = new Rectangle( tile.wallFrameX(), tile.wallFrameY() + y_offset, 32, 32 );

				Main.spriteBatch.Draw( Main.wallTexture[type], pos, frame, draw_color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
				return false;
			}

			return true;
		}


		public override void KillWall( int i, int j, int type, ref bool fail ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;

			if( myworld.Layers.Background.HasColorAt( tile_x, tile_y ) ) {
				myworld.Layers.Background.RemoveColorAt( tile_x, tile_y );
			}
		}
	}
}
