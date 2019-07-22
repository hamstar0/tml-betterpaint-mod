using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint {
	class BetterPaintTile : GlobalTile {
		public override void DrawEffects( int i, int j, int type, SpriteBatch sb, ref Color drawColor, ref int nextSpecialDrawIdx ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tileX = (ushort)i;
			ushort tileY = (ushort)j;
			
			if( myworld.Layers.Foreground.HasColorAt(tileX, tileY) ) {
				drawColor = myworld.Layers.Foreground.ComputeTileColor( tileX, tileY );
			}
		}


		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			if( effectOnly ) { return; }

			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tileX = (ushort)i;
			ushort tileY = (ushort)j;

			if( myworld.Layers.Foreground.HasColorAt( tileX, tileY ) ) {
				myworld.Layers.Foreground.RemoveRawColorAt( tileX, tileY );
			}
		}
	}




	class BetterPaintWall : GlobalWall {
		public override bool PreDraw( int i, int j, int type, SpriteBatch sb ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tileX = (ushort)i;
			ushort tileY = (ushort)j;

			if( myworld.Layers.Background.HasColorAt( tileX, tileY ) ) {
				Color drawDolor = myworld.Layers.Background.ComputeTileColor( tileX, tileY );

				var zero = new Vector2( (float)Main.offScreenRange, (float)Main.offScreenRange );
				if( Main.drawToScreen ) {
					zero = Vector2.Zero;
				}

				var pos = (new Vector2( (i * 16) - 8, (j * 16) - 8 ) - Main.screenPosition) + zero;

				Tile tile = Main.tile[i, j];
				int yOffset = (int)( Main.wallFrame[type] * 180 );
				var frame = new Rectangle( tile.wallFrameX(), tile.wallFrameY() + yOffset, 32, 32 );

				Main.spriteBatch.Draw( Main.wallTexture[type], pos, frame, drawDolor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f );
				return false;
			}

			return true;
		}


		public override void KillWall( int i, int j, int type, ref bool fail ) {
			var myworld = this.mod.GetModWorld<BetterPaintWorld>();
			ushort tileX = (ushort)i;
			ushort tileY = (ushort)j;

			if( myworld.Layers.Background.HasColorAt( tileX, tileY ) ) {
				myworld.Layers.Background.RemoveRawColorAt( tileX, tileY );
			}
		}
	}
}
