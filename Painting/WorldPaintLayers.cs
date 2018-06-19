using BetterPaint.NetProtocols;
using BetterPaint.Painting.Brushes;
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

		public float ApplyColorAt( BetterPaintMod mymod, PaintLayerType layer, PaintBrushType brush_type,
				Color color, PaintBrushSize brush_size, float pressure_precent, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }

			int rand_seed = DateTime.Now.Millisecond;
			float paints_used = this.ApplyColorAtNoSync( mymod, layer, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( layer, brush_type, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
			}

			return paints_used;
		}

		internal float ApplyColorAtNoSync( BetterPaintMod mymod, PaintLayerType layer, PaintBrushType brush_type,
				Color color, PaintBrushSize brush_size, float pressure_precent,
				int rand_seed, int world_x, int world_y ) {
			if( Main.netMode == 2 ) { throw new Exception( "No server." ); }
			
			PaintBrush brush = mymod.Modes[ brush_type ];
			float paints_used = 0f;

			switch( layer ) {
			case PaintLayerType.Background:
				paints_used += brush.Apply( this.Background, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
				break;
			case PaintLayerType.Foreground:
				paints_used += brush.Apply( this.Foreground, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
				break;
			case PaintLayerType.Anyground:
				paints_used += brush.Apply( this.Background, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
				paints_used += brush.Apply( this.Foreground, color, brush_size, pressure_precent, rand_seed, world_x, world_y );
				break;
			default:
				throw new NotImplementedException();
			}

			return paints_used;
		}
	}
}
