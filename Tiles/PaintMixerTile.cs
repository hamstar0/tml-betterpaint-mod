using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace BetterPaint.Tiles {
	class PaintMixerTile : ModTile {
		public override void SetDefaults() {
			Main.tileSolidTop[ this.Type ] = true;
			Main.tileFrameImportant[ this.Type ] = true;
			Main.tileNoAttach[ this.Type ] = true;
			Main.tileTable[ this.Type ] = true;
			Main.tileLavaDeath[ this.Type ] = false;
			TileObjectData.newTile.CopyFrom( TileObjectData.Style2x2 );
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.addTile( this.Type );

			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Paint Mixer" );

			this.AddMapEntry( new Color( 200, 200, 200 ), name );
			this.dustType = 1;
			this.disableSmartCursor = true;
			this.adjTiles = new int[] { this.Type };

			this.animationFrameHeight = 36;
		}


		public override void AnimateTile( ref int frame, ref int frame_counter ) {
			if( ++Main.tileFrameCounter[ this.Type ] >= 2 ) {
				Main.tileFrameCounter[ this.Type ] = 0;

				if( ++Main.tileFrame[ this.Type ] >= 2 ) {
					Main.tileFrame[ this.Type ] = 0;
				}
			}
		}


		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile( int i, int j, int frameX, int frameY ) {
			Item.NewItem( i * 16, j * 16, 32, 16, mod.ItemType( "PaintMixerItem" ) );
		}
	}
}
