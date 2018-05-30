using BetterPaint.Painting;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( bool fg_only, PaintBrushType mode, Color color, bool brush_size_small, float pressure, int rand_seed, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( fg_only, mode, color, brush_size_small, pressure, rand_seed, world_x, world_y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public bool FgOnly = false;
		public PaintBrushType Mode = PaintBrushType.Stream;
		public Color MyColor = Color.White;
		public bool BrushSizeSmall = true;
		public float Pressure = 1;
		public int RandSeed = -1;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( bool fg_only, PaintBrushType mode, Color color, bool brush_size_small, float pressure, int rand_seed, int world_x, int world_y ) {
			this.FgOnly = fg_only;
			this.Mode = mode;
			this.MyColor = color;
			this.BrushSizeSmall = brush_size_small;
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
				myworld.AddForegroundColorNoSync( this.Mode, this.MyColor, this.BrushSizeSmall, this.Pressure, this.RandSeed, this.WorldX, this.WorldY );
			} else {
				myworld.AddBackgroundColorNoSync( this.Mode, this.MyColor, this.BrushSizeSmall, this.Pressure, this.RandSeed, this.WorldX, this.WorldY );
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
