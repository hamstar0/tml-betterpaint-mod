using BetterPaint.Painting.Brushes;
using HamstarHelpers.Helpers.HudHelpers;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		public void DrawBrushes( SpriteBatch sb, out Rectangle streamRect, out Rectangle sprayRect, out Rectangle spatterRect,
				out Rectangle eraserRect ) {
			int brushesDist = PaintBlasterUI.BrushesRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			/*Texture2D tex_stream = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_spatter = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_eraser = Main.itemTexture[ItemID.PaintScraper];*/
			Texture2D texStream = PaintBlasterUI.BrushStream;
			Texture2D texSpray = PaintBlasterUI.BrushSpray;
			Texture2D texSpatter = PaintBlasterUI.BrushSpatter;
			Texture2D texEraser = PaintBlasterUI.BrushEraser;

			var streamOffset = new Vector2( texStream.Width, texStream.Height ) * 0.5f;
			var sprayOffset = new Vector2( texSpray.Width, texSpray.Height ) * 0.5f;
			var spatterOffset = new Vector2( texSpatter.Width, texSpatter.Height ) * 0.5f;
			var eraserOffset = new Vector2( texEraser.Width, texEraser.Height ) * 0.5f;

			streamRect = new Rectangle( (int)( ( x - brushesDist ) - streamOffset.X ), (int)( y - streamOffset.Y ), texStream.Width, texStream.Height );
			sprayRect = new Rectangle( (int)( x - sprayOffset.X ), (int)( ( y - brushesDist ) - sprayOffset.Y ), texSpray.Width, texSpray.Height );
			spatterRect = new Rectangle( (int)( ( x + brushesDist ) - spatterOffset.X ), (int)( y - spatterOffset.Y ), texSpatter.Width, texSpatter.Height );
			eraserRect = new Rectangle( (int)( x - eraserOffset.X ), (int)( ( y + brushesDist ) - eraserOffset.Y ), texEraser.Width, texEraser.Height );

			bool streamHover = streamRect.Contains( Main.mouseX, Main.mouseY );
			bool sprayHover = streamHover ? false : sprayRect.Contains( Main.mouseX, Main.mouseY );
			bool spatterHover = sprayHover ? false : spatterRect.Contains( Main.mouseX, Main.mouseY );
			bool eraserHover = spatterHover ? false : eraserRect.Contains( Main.mouseX, Main.mouseY );
			
			sb.Draw( texStream, streamRect, Color.White * ( this.CurrentBrush == PaintBrushType.Stream ? hilit : ( streamHover ? lit : unlit ) ) );
			sb.Draw( texSpray, sprayRect, Color.White * ( this.CurrentBrush == PaintBrushType.Spray ? hilit : ( sprayHover ? lit : unlit ) ) );
			sb.Draw( texSpatter, spatterRect, Color.White * ( this.CurrentBrush == PaintBrushType.Spatter ? hilit : ( spatterHover ? lit : unlit ) ) );
			sb.Draw( texEraser, eraserRect, Color.White * ( this.CurrentBrush == PaintBrushType.Erase ? hilit : ( eraserHover ? lit : unlit ) ) );

			if( streamHover ) {
				var toolColor = this.CurrentBrush == PaintBrushType.Stream ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Stream Mode", streamRect.X, streamRect.Y + streamRect.Height, toolColor, Color.Black, default( Vector2 ), 1f );
			} else if( sprayHover ) {
				var toolColor = this.CurrentBrush == PaintBrushType.Spray ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Spray Mode", sprayRect.X, sprayRect.Y + sprayRect.Height, toolColor, Color.Black, default( Vector2 ), 1f );
			} else if( spatterHover ) {
				var toolColor = this.CurrentBrush == PaintBrushType.Spatter ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Spatter Mode", spatterRect.X, spatterRect.Y + spatterRect.Height, toolColor, Color.Black, default( Vector2 ), 1f );
			} else if( eraserHover ) {
				var toolColor = this.CurrentBrush == PaintBrushType.Erase ? Color.White : Color.LightGray;
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Eraser Mode", eraserRect.X, eraserRect.Y + eraserRect.Height, toolColor, Color.Black, default( Vector2 ), 1f );
			}

			switch( this.CurrentBrush ) {
			case PaintBrushType.Stream:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, streamRect, 2 );
				break;
			case PaintBrushType.Spray:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, sprayRect, 2 );
				break;
			case PaintBrushType.Spatter:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, spatterRect, 2 );
				break;
			case PaintBrushType.Erase:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, eraserRect, 2 );
				break;
			}
		}
	}
}
