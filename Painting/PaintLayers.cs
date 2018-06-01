using BetterPaint.NetProtocols;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	class PaintLayers {
		public PaintData Foreground { get; private set; }
		public PaintData Background { get; private set; }


		////////////////

		internal PaintLayers() {
			this.Foreground = new PaintData();
			this.Background = new PaintData();
		}

		////////////////

		public void Load( TagCompound tags ) {
			this.Background.Load( tags, "bg" );
			this.Foreground.Load( tags, "fg" );
		}

		public TagCompound Save() {
			var tags = new TagCompound();

			this.Foreground.Save( tags, "fg" );
			this.Background.Save( tags, "bg" );

			return tags;
		}


		////////////////

		public float ApplyForegroundColor( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddForegroundColorNoSync( mymod, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Foreground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyBackgroundColor( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddBackgroundColorNoSync( mymod, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Background, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyAnygroundColor( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddAnygroundColorNoSync( mymod, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Anyground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		////

		public float AddForegroundColorNoSync( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			return mymod.Modes[brush_type].Apply( this.Foreground, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
		}

		public float AddBackgroundColorNoSync( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			return mymod.Modes[brush_type].Apply( this.Background, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
		}

		public float AddAnygroundColorNoSync( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			ushort tile_x = (ushort)( world_x / 16 );
			ushort tile_y = (ushort)( world_y / 16 );
			float paints_used = 0;
			
			paints_used += this.AddForegroundColorNoSync( mymod, brush_type, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
			paints_used += this.AddBackgroundColorNoSync( mymod, brush_type, color, brush_size, pressure_percent, rand_seed, world_x, world_y );

			return paints_used;
		}
	}
}
