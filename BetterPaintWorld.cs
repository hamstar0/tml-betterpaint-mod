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

		public int AddForegroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int paints_used = this.AddForegroundColorNoSync( color, size, mode, x, y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( true, color, size, mode, x, y );
			}

			return paints_used;
		}

		public int AddBackgroundColor( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int paints_used = this.AddBackgroundColorNoSync( color, size, mode, x, y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( false, color, size, mode, x, y );
			}

			return paints_used;
		}

		////

		public int AddForegroundColorNoSync( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			switch( mode ) {
			case PaintMode.Stream:
				StreamBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Spray:
				//SprayBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Flood:
				//FloodBrush.Paint( this.FgColors, color, size, x, y );
				break;
			case PaintMode.Erase:
				//EraserBrush.Paint( this.FgColors, color, size, x, y );
				break;
			}

			return 1;
		}
		
		public int AddBackgroundColorNoSync( Color color, int size, PaintMode mode, ushort x, ushort y ) {
			switch( mode ) {
			case PaintMode.Stream:
				StreamBrush.Paint( this.BgColors, color, size, x, y );
				break;
			case PaintMode.Spray:
				//SprayBrush.Paint( this.BgColors, color, size, x, y );
				break;
			case PaintMode.Flood:
				//FloodBrush.Paint( this.BgColors, color, size, x, y );
				break;
			case PaintMode.Erase:
				//EraserBrush.Paint( this.BgColors, color, size, x, y );
				break;
			}

			return 1;
		}


		////////////////

		public bool HasFgColor( Color color, int i, int j ) {
			return this.FgColors.HasColor( (ushort)i, (ushort)j );
		}

		public bool HasBgColor( Color color, int i, int j ) {
			return this.BgColors.HasColor( (ushort)i, (ushort)j );
		}
	}
}
