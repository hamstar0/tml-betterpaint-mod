using BetterPaint.NetProtocols;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Tiles;
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

		public PaintLayer Foreground;
		public PaintLayer Background;



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

		public float ApplyColorAt( BetterPaintMod mymod, PaintLayerType layer, PaintBrushType brushType,
				Color color, byte glow, PaintBrushSize brushSize, float pressurePrecent, int worldX, int worldY ) {
			if( Main.netMode == 2 ) { throw new HamstarException( "No server." ); }
			
			int randSeed = DateTime.Now.Millisecond;
			float paintsUsed = this.ApplyColorAtNoSync( mymod, layer, brushType, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );

			if( Main.netMode == 1 ) {
				PaintStrokeProtocol.SyncToAll( layer, brushType, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );
			}

			return paintsUsed;
		}

		internal float ApplyColorAtNoSync( BetterPaintMod mymod, PaintLayerType layer, PaintBrushType brushType,
				Color color, byte glow, PaintBrushSize brushSize, float pressurePrecent,
				int randSeed, int worldX, int worldY ) {
			if( Main.netMode == 2 ) { throw new HamstarException( "No server." ); }
			
			PaintBrush brush = mymod.Modes[ brushType ];
			float paintsUsed = 0f;
			
			switch( layer ) {
			case PaintLayerType.Background:
				paintsUsed += brush.Apply( this.Background, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );
				break;
			case PaintLayerType.Foreground:
				paintsUsed += brush.Apply( this.Foreground, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );
				break;
			case PaintLayerType.Anyground:
				paintsUsed += brush.Apply( this.Background, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );
				paintsUsed += brush.Apply( this.Foreground, color, glow, brushSize, pressurePrecent, randSeed, worldX, worldY );
				break;
			default:
				throw new HamstarException( "Not implemented." );
			}

			return paintsUsed;
		}
	}
}
