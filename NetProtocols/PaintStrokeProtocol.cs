using BetterPaint.Painting;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( bool fg_only, Color color, int size, PaintModeType mode, ushort x, ushort y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( fg_only, color, size, mode, x, y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public bool FgOnly = false;
		public Color MyColor = Color.White;
		public int Size = -1;
		public PaintModeType Mode = PaintModeType.Stream;
		public ushort X = 0;
		public ushort Y = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( bool fg_only, Color color, int size, PaintModeType mode, ushort x, ushort y ) {
			this.FgOnly = fg_only;
			this.MyColor = color;
			this.Size = size;
			this.Mode = mode;
			this.X = y;
			this.Y = y;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		private void Receive() {
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();

			if( this.FgOnly ) {
				myworld.AddForegroundColorNoSync( this.MyColor, this.Size, this.Mode, this.X, this.Y );
			} else {
				myworld.AddBackgroundColorNoSync( this.MyColor, this.Size, this.Mode, this.X, this.Y );
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
