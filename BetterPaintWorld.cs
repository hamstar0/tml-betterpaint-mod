using BetterPaint.NetProtocols;
using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public PaintData FgColors { get; private set; }
		public PaintData BgColors { get; private set; }


		////////////////

		public override void Initialize() {
			this.FgColors = new PaintData();
			this.BgColors = new PaintData();
		}

		////////////////

		public override void Load( TagCompound tags ) {
			this.BgColors.Load( tags, "bg" );
			this.FgColors.Load( tags, "fg" );
		}

		public override TagCompound Save() {
			var tags = new TagCompound();

			this.FgColors.Save( tags, "fg" );
			this.BgColors.Save( tags, "bg" );

			return tags;
		}


		////////////////

		public float AddForegroundColor( Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			float paints_used = this.AddForegroundColorNoSync( color, size, mode, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( true, color, size, mode, world_x, world_y );
			}

			return paints_used;
		}

		public float AddBackgroundColor( Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			float paints_used = this.AddBackgroundColorNoSync( color, size, mode, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( false, color, size, mode, world_x, world_y );
			}

			return paints_used;
		}

		////

		public float AddForegroundColorNoSync( Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[ mode ].Paint( this.FgColors, color, size, world_x, world_y );
		}
		
		public float AddBackgroundColorNoSync( Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[mode].Paint( this.BgColors, color, size, world_x, world_y );
		}
	}
}
