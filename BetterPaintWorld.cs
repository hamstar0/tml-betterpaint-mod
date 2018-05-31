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

		public float ApplyForegroundColor( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddForegroundColorNoSync( brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Foreground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyBackgroundColor( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddBackgroundColorNoSync( brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Background, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyAnygroundColor( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddAnygroundColorNoSync( brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayer.Anyground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		////

		public float AddForegroundColorNoSync( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[ brush_type ].Apply( this.FgColors, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
		}
		
		public float AddBackgroundColorNoSync( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;

			return mymod.Modes[brush_type].Apply( this.BgColors, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
		}

		public float AddAnygroundColorNoSync( PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y ) {
			var mymod = (BetterPaintMod)this.mod;
			ushort tile_x = (ushort)( world_x / 16 );
			ushort tile_y = (ushort)( world_y / 16 );

			if( this.FgColors.HasColor(tile_x, tile_y) ) {
				return this.AddForegroundColorNoSync( brush_type, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
			} else {
				return this.AddBackgroundColorNoSync( brush_type, color, brush_size, pressure_percent, rand_seed, world_x, world_y );
			}
		}
	}
}
