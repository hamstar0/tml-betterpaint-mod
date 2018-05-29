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

		public float AddForegroundColor( PaintModeType mode, Color color, int size, float pressure, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddForegroundColorNoSync( mode, color, size, pressure, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( true, mode, color, size, pressure, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float AddBackgroundColor( PaintModeType mode, Color color, int size, float pressure, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddBackgroundColorNoSync( mode, color, size, pressure, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( false, mode, color, size, pressure, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		////

		public float AddForegroundColorNoSync( PaintModeType mode, Color color, int size, float pressure, int rand_seed, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[ mode ].Apply( this.FgColors, color, size, pressure, rand_seed, world_x, world_y );
		}
		
		public float AddBackgroundColorNoSync( PaintModeType mode, Color color, int size, float pressure, int rand_seed, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[mode].Apply( this.BgColors, color, size, pressure, rand_seed, world_x, world_y );
		}
	}
}
