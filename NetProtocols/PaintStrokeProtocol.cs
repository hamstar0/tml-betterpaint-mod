using BetterPaint.Painting.Brushes;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.NetProtocols {
	class PaintStrokeProtocol : PacketProtocolSentToEither {
		protected class MyFactory : Factory<PaintStrokeProtocol> {
			public int Layer = (int)PaintLayerType.Foreground;
			public int BrushType = (int)PaintBrushType.Stream;
			public Color MyColor = Color.White;
			public byte Glow = 0;
			public int BrushSize = (int)PaintBrushSize.Small;
			public float PressurePercent = 1;
			public int RandSeed = -1;
			public int WorldX = 0;
			public int WorldY = 0;

			public MyFactory( PaintLayerType layer, PaintBrushType brush_type, Color color, byte glow,
				PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
				this.Layer = (int)layer;
				this.BrushType = (int)brush_type;
				this.MyColor = color;
				this.Glow = glow;
				this.BrushSize = (int)brush_size;
				this.PressurePercent = pressure_percent;
				this.WorldX = world_x;
				this.WorldY = world_y;
			}

			protected override void Initialize( PaintStrokeProtocol data ) {
				data.Layer = this.Layer;
				data.BrushType = this.BrushType;
				data.MyColor = this.MyColor;
				data.Glow = this.Glow;
				data.BrushSize = this.BrushSize;
				data.PressurePercent = this.PressurePercent;
				data.WorldX = this.WorldX;
				data.WorldY = this.WorldY;
			}
		}



		////////////////

		public static void SyncToAll( PaintLayerType layer, PaintBrushType brush_type, Color color, byte glow,
				PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var factory = new MyFactory( layer, brush_type, color, glow, brush_size, pressure_percent, rand_seed, world_x, world_y );
			PaintStrokeProtocol protocol = factory.Create();

			protocol.SendToServer( true );
		}



		////////////////
		
		public int Layer = (int)PaintLayerType.Foreground;
		public int BrushType = (int)PaintBrushType.Stream;
		public Color MyColor = Color.White;
		public byte Glow = 0;
		public int BrushSize = (int)PaintBrushSize.Small;
		public float PressurePercent = 1;
		public int RandSeed = -1;
		public int WorldX = 0;
		public int WorldY = 0;


		////////////////

		protected PaintStrokeProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults( int to_who ) { }


		////////////////

		private void Receive() {
			var mymod = BetterPaintMod.Instance;
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();
			var layer = (PaintLayerType)this.Layer;
			var brush_type = (PaintBrushType)this.BrushType;

			myworld.Layers.ApplyColorAtNoSync( mymod, layer, brush_type, this.MyColor, this.Glow, (PaintBrushSize)this.BrushSize,
				this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
		}


		protected override void ReceiveOnServer( int from_who ) {
			this.Receive();
		}

		protected override void ReceiveOnClient() {
			this.Receive();
		}
	}
}
