using BetterPaint.Painting;
using HamstarHelpers.Components.Network;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocol {
		public static void SyncToAll( PaintLayerType layer, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var protocol = new PaintStrokeProtocol( layer, brush_type, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
			protocol.SendToServer( true );
		}



		////////////////
		
		public int Layer = (int)PaintLayerType.Foreground;
		public int BrushType = (int)PaintBrushType.Stream;
		public Color MyColor = Color.White;
		public int BrushSize = (int)PaintBrushSize.Small;
		public float PressurePercent = 1;
		public int RandSeed = -1;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		public PaintStrokeProtocol() { }

		private PaintStrokeProtocol( PaintLayerType layer, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			this.Layer = (int)layer;
			this.BrushType = (int)brush_type;
			this.MyColor = color;
			this.BrushSize = (int)brush_size;
			this.PressurePercent = pressure_percent;
			this.WorldX = world_x;
			this.WorldY = world_y;
		}

		////////////////

		public override void SetServerDefaults() { }

		////////////////

		private void Receive() {
			var mymod = BetterPaintMod.Instance;
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();
			var layer = (PaintLayerType)this.Layer;
			var brush_type = (PaintBrushType)this.BrushType;

			switch( layer ) {
			case PaintLayerType.Foreground:
				myworld.Layers.AddForegroundColorNoSync( mymod, brush_type, this.MyColor, (PaintBrushSize)this.BrushSize, this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
				break;
			case PaintLayerType.Background:
				myworld.Layers.AddBackgroundColorNoSync( mymod, brush_type, this.MyColor, (PaintBrushSize)this.BrushSize, this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
				break;
			case PaintLayerType.Anyground:
				myworld.Layers.AddAnygroundColorNoSync( mymod, brush_type, this.MyColor, (PaintBrushSize)this.BrushSize, this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
				break;
			default:
				throw new NotImplementedException();
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
