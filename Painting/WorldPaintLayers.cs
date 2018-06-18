using BetterPaint.NetProtocols;
using HamstarHelpers.TileHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	class WorldPaintLayers {
		public static bool CanPaintForeground( Tile tile ) {
			return TileHelpers.IsSolid( tile, true, true );
		}

		public static bool CanPaintBackground( Tile tile ) {
			return !TileHelpers.IsAir( tile ) && tile.wall != 0;
		}


		////////////////

		public PaintLayer Foreground { get; private set; }
		public PaintLayer Background { get; private set; }


		////////////////

		internal WorldPaintLayers() {
			this.Foreground = new ForegroundPaintLayer();
			this.Background = new BackgroundPaintLayer();
		}

		////////////////

		public void Load( BetterPaintMod mymod, TagCompound tags ) {
			this.Background.Load( mymod, tags, "bg" );
			this.Foreground.Load( mymod, tags, "fg" );
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
				PaintStrokeProtocol.SyncToAll( PaintLayerType.Foreground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyBackgroundColor( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddBackgroundColorNoSync( mymod, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayerType.Background, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		public float ApplyAnygroundColor( BetterPaintMod mymod, PaintBrushType brush_type, Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.AddAnygroundColorNoSync( mymod, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( PaintLayerType.Anyground, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
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
