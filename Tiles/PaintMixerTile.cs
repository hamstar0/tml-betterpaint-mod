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
			Main.tileLavaDeath[ this.Type ] = true;
			TileObjectData.newTile.CopyFrom( TileObjectData.Style2x2 );
			TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
			TileObjectData.addTile( this.Type );

			this.AddToArray( ref TileID.Sets.RoomNeeds.CountsAsTable );

			ModTranslation name = this.CreateMapEntryName();
			name.SetDefault( "Paint Mixer" );

			this.AddMapEntry( new Color( 200, 200, 200 ), name );
			this.dustType = mod.DustType( "Sparkle" );
			this.disableSmartCursor = true;
			//this.adjTiles = new int[] { TileID.WorkBenches };
			this.adjTiles = new int[] { this.Type };
		}

		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile( int i, int j, int frameX, int frameY ) {
			Item.NewItem( i * 16, j * 16, 32, 16, mod.ItemType( "PaintMixerItem" ) );
		}
	}
}
