using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintTile : GlobalTile {
		public override void DrawEffects( int i, int j, int type, SpriteBatch sb, ref Color draw_color, ref int next_special_draw_idx ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort x = (ushort)i;
			ushort y = (ushort)j;
			
			if( myworld.Layers.Foreground.HasColor(x, y) ) {
				Color painted_color = myworld.Layers.Foreground.GetColor( x, y );
				draw_color = Lighting.GetColor( i, j, painted_color );
			}
		}
	}


	class BetterPaintWall : GlobalWall {
		public static IDictionary<int, IDictionary<int, Color>> Colors = new Dictionary<int, IDictionary<int, Color>>();


		public override bool PreDraw( int i, int j, int type, SpriteBatch sb ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;

			if( myworld.Layers.Background.HasColor( tile_x, tile_y ) ) {
				Color painted_color = myworld.Layers.Background.GetColor( tile_x, tile_y );
				Color color = Lighting.GetColor( i, j, painted_color );

				Vector2 zero = new Vector2( (float)Main.offScreenRange, (float)Main.offScreenRange );
				if( Main.drawToScreen ) {
					zero = Vector2.Zero;
				}

				var pos = (new Vector2( (i * 16) - 8, (j * 16) - 8 ) - Main.screenPosition) + zero;

				Tile tile = Main.tile[i, j];
				int y_offset = (int)( Main.wallFrame[type] * 180 );
				var frame = new Rectangle( tile.wallFrameX(), tile.wallFrameY() + y_offset, 32, 32 );

				Main.spriteBatch.Draw( Main.wallTexture[type], pos, frame, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
				return false;
			}

			return true;
		}
	}
}
