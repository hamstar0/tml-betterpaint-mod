using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using HamstarHelpers.HudHelpers;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		public void DrawBrushes( SpriteBatch sb, out Rectangle stream_rect, out Rectangle spray_rect, out Rectangle spatter_rect, out Rectangle eraser_rect ) {
			int brushes_dist = PaintBlasterUI.BrushesRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			/*Texture2D tex_stream = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_spatter = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_eraser = Main.itemTexture[ItemID.PaintScraper];*/
			Texture2D tex_stream = PaintBlasterUI.BrushStream;
			Texture2D tex_spray = PaintBlasterUI.BrushSpray;
			Texture2D tex_spatter = PaintBlasterUI.BrushSpatter;
			Texture2D tex_eraser = PaintBlasterUI.BrushEraser;

			var stream_offset = new Vector2( tex_stream.Width, tex_stream.Height ) * 0.5f;
			var spray_offset = new Vector2( tex_spray.Width, tex_spray.Height ) * 0.5f;
			var spatter_offset = new Vector2( tex_spatter.Width, tex_spatter.Height ) * 0.5f;
			var eraser_offset = new Vector2( tex_eraser.Width, tex_eraser.Height ) * 0.5f;

			stream_rect = new Rectangle( (int)( ( x - brushes_dist ) - stream_offset.X ), (int)( y - stream_offset.Y ), tex_stream.Width, tex_stream.Height );
			spray_rect = new Rectangle( (int)( x - spray_offset.X ), (int)( ( y - brushes_dist ) - spray_offset.Y ), tex_spray.Width, tex_spray.Height );
			spatter_rect = new Rectangle( (int)( ( x + brushes_dist ) - spatter_offset.X ), (int)( y - spatter_offset.Y ), tex_spatter.Width, tex_spatter.Height );
			eraser_rect = new Rectangle( (int)( x - eraser_offset.X ), (int)( ( y + brushes_dist ) - eraser_offset.Y ), tex_eraser.Width, tex_eraser.Height );

			bool stream_hover = stream_rect.Contains( Main.mouseX, Main.mouseY );
			bool spray_hover = stream_hover ? false : spray_rect.Contains( Main.mouseX, Main.mouseY );
			bool spatter_hover = spray_hover ? false : spatter_rect.Contains( Main.mouseX, Main.mouseY );
			bool eraser_hover = spatter_hover ? false : eraser_rect.Contains( Main.mouseX, Main.mouseY );
			
			sb.Draw( tex_stream, stream_rect, Color.White * ( this.CurrentBrush == PaintBrushType.Stream ? hilit : ( stream_hover ? lit : unlit ) ) );
			sb.Draw( tex_spray, spray_rect, Color.White * ( this.CurrentBrush == PaintBrushType.Spray ? hilit : ( spray_hover ? lit : unlit ) ) );
			sb.Draw( tex_spatter, spatter_rect, Color.White * ( this.CurrentBrush == PaintBrushType.Spatter ? hilit : ( spatter_hover ? lit : unlit ) ) );
			sb.Draw( tex_eraser, eraser_rect, Color.White * ( this.CurrentBrush == PaintBrushType.Erase ? hilit : ( eraser_hover ? lit : unlit ) ) );

			if( stream_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Stream ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Stream Mode", stream_rect.X, stream_rect.Y + stream_rect.Height, tool_color, Color.Black, default( Vector2 ), 1f );
			} else if( spray_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Spray ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Spray Mode", spray_rect.X, spray_rect.Y + spray_rect.Height, tool_color, Color.Black, default( Vector2 ), 1f );
			} else if( spatter_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Spatter ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Spatter Mode", spatter_rect.X, spatter_rect.Y + spatter_rect.Height, tool_color, Color.Black, default( Vector2 ), 1f );
			} else if( eraser_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Erase ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Eraser Mode", eraser_rect.X, eraser_rect.Y + eraser_rect.Height, tool_color, Color.Black, default( Vector2 ), 1f );
			}

			switch( this.CurrentBrush ) {
			case PaintBrushType.Stream:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, stream_rect, 2 );
				break;
			case PaintBrushType.Spray:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, spray_rect, 2 );
				break;
			case PaintBrushType.Spatter:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, spatter_rect, 2 );
				break;
			case PaintBrushType.Erase:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, eraser_rect, 2 );
				break;
			}
		}
	}
}
