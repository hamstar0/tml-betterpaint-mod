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

			public MyFactory( PaintLayerType layer, PaintBrushType brushType, Color color, byte glow,
					PaintBrushSize brushSize, float pressurePercent, int randSeed, int worldX, int worldY ) {
				this.Layer = (int)layer;
				this.BrushType = (int)brushType;
				this.MyColor = color;
				this.Glow = glow;
				this.BrushSize = (int)brushSize;
				this.PressurePercent = pressurePercent;
				this.WorldX = worldX;
				this.WorldY = worldY;
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

		public static void SyncToAll( PaintLayerType layer, PaintBrushType brushType, Color color, byte glow,
				PaintBrushSize brushSize, float pressurePercent, int randSeed, int worldX, int worldY ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var factory = new MyFactory( layer, brushType, color, glow, brushSize, pressurePercent, randSeed, worldX, worldY );
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

		private PaintStrokeProtocol() { }

		////////////////

		protected override void SetServerDefaults( int toWho ) { }


		////////////////

		private void Receive() {
			var mymod = BetterPaintMod.Instance;
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();
			var layer = (PaintLayerType)this.Layer;
			var brushType = (PaintBrushType)this.BrushType;

			myworld.Layers.ApplyColorAtNoSync( mymod, layer, brushType, this.MyColor, this.Glow, (PaintBrushSize)this.BrushSize,
				this.PressurePercent, this.RandSeed, this.WorldX, this.WorldY );
		}


		protected override void ReceiveOnServer( int fromWho ) {
			this.Receive();
		}

		protected override void ReceiveOnClient() {
			this.Receive();
		}
	}
}
