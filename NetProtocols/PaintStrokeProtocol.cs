using BetterPaint.Painting;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( bool fg_only, PaintModeType mode, Color color, int size, float pressure, int rand_seed, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( fg_only, mode, color, size, pressure, rand_seed, world_x, world_y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public bool FgOnly = false;
		public PaintModeType Mode = PaintModeType.Stream;
		public Color MyColor = Color.White;
		public int Size = -1;
		public float Pressure = 1;
		public int RandSeed = -1;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( bool fg_only, PaintModeType mode, Color color, int size, float pressure, int rand_seed, int world_x, int world_y ) {
			this.FgOnly = fg_only;
			this.Mode = mode;
			this.MyColor = color;
			this.Size = size;
			this.Pressure = pressure;
			this.WorldX = world_x;
			this.WorldY = world_y;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		private void Receive() {
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();

			if( this.FgOnly ) {
				myworld.AddForegroundColorNoSync( this.Mode, this.MyColor, this.Size, this.Pressure, this.RandSeed, this.WorldX, this.WorldY );
			} else {
				myworld.AddBackgroundColorNoSync( this.Mode, this.MyColor, this.Size, this.Pressure, this.RandSeed, this.WorldX, this.WorldY );
			}
		}


		protected override void ReceiveWithServer( int from_who ) {
			this.Receive();
		}

		protected override void ReceiveWithClient() {
			this.Receive();
		}
	}
}
