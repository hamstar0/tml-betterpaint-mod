using BetterPaint.Painting;
using HamstarHelpers.Utilities.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( bool fg_only, PaintBrushType mode, Color color, PaintBrushSize brush_size, float pressure, int rand_seed, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( fg_only, mode, color, brush_size, pressure, rand_seed, world_x, world_y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public bool FgOnly = false;
		public int BrushType = (int)PaintBrushType.Stream;
		public Color MyColor = Color.White;
		public int BrushSize = (int)PaintBrushSize.Small;
		public float PressurePercent = 1;
		public int RandSeed = -1;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( bool fg_only, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure, int rand_seed, int world_x, int world_y ) {
			this.FgOnly = fg_only;
			this.BrushType = (int)brush_type;
			this.MyColor = color;
			this.BrushSize = (int)brush_size;
			this.PressurePercent = pressure;
			this.WorldX = world_x;
			this.WorldY = world_y;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		private void Receive() {
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();

			if( this.FgOnly ) {
				myworld.AddForegroundColorNoSync( (PaintBrushType)this.BrushType, this.MyColor, (PaintBrushSize)this.BrushSize, this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
			} else {
				myworld.AddBackgroundColorNoSync( (PaintBrushType)this.BrushType, this.MyColor, (PaintBrushSize)this.BrushSize, this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
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
