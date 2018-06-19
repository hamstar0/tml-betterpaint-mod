using BetterPaint.Painting;
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
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;
			
			if( myworld.Layers.Foreground.HasColor(tile_x, tile_y) ) {
				Color paint_data = myworld.Layers.Foreground.GetColor( tile_x, tile_y );
				Color full_color = PaintHelpers.FullColor( paint_data );
				Color env_color = Lighting.GetColor( i, j, full_color );
				float lit_scale = (float)paint_data.A / 255f;

				draw_color = Color.Lerp( env_color, full_color, lit_scale );
			}
		}


		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effect_only, ref bool no_item ) {
			if( effect_only ) { return; }

			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tile_x = (ushort)i;
			ushort tile_y = (ushort)j;

			if( myworld.Layers.Foreground.HasColor( tile_x, tile_y ) ) {
				myworld.Layers.Foreground.RemoveColorAt( tile_x, tile_y );
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
				Color paint_data = myworld.Layers.Background.GetColor( tile_x, tile_y );
				Color full_color = PaintHelpers.FullColor( paint_data );
				Color env_color = Lighting.GetColor( i, j, full_color );
				float lit_scale = (float)paint_data.A / 255f;

				Color draw_color = Color.Lerp( env_color, full_color, lit_scale );

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

			if( myworld.Layers.Background.HasColor( tile_x, tile_y ) ) {
				myworld.Layers.Background.RemoveColorAt( tile_x, tile_y );
			}
		}
	}
}
