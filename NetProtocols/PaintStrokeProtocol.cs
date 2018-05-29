using BetterPaint.Painting;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( bool fg_only, Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( fg_only, color, size, mode, world_x, world_y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public bool FgOnly = false;
		public Color MyColor = Color.White;
		public int Size = -1;
		public PaintModeType Mode = PaintModeType.Stream;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( bool fg_only, Color color, int size, PaintModeType mode, int world_x, int world_y ) {
			this.FgOnly = fg_only;
			this.MyColor = color;
			this.Size = size;
			this.Mode = mode;
			this.WorldX = world_x;
			this.WorldY = world_y;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		private void Receive() {
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();

			if( this.FgOnly ) {
				myworld.AddForegroundColorNoSync( this.MyColor, this.Size, this.Mode, this.WorldX, this.WorldY );
			} else {
				myworld.AddBackgroundColorNoSync( this.MyColor, this.Size, this.Mode, this.WorldX, this.WorldY );
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
