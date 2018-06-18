using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public void DrawOptionLayer( SpriteBatch sb, int origin_x, int origin_y, out Rectangle rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			//float unlit = PaintBlasterUI.IdleScale;

			Texture2D tex;

			switch( this.Layer ) {
			case PaintLayerType.Foreground:
				tex = PaintBlasterUI.LayerFgTex;
				break;
			case PaintLayerType.Background:
				tex = PaintBlasterUI.LayerBgTex;
				break;
			case PaintLayerType.Anyground:
				tex = PaintBlasterUI.LayerBothTex;
				break;
			default:
				throw new NotImplementedException();
			}

			var offset = new Vector2( tex.Width, tex.Height ) * 0.5f;

			int x = ( origin_x - options_dist ) - (int)offset.X;
			int y = ( origin_y - options_dist ) - (int)offset.Y;
			rect = new Rectangle( x, y, tex.Width, tex.Height );

			bool hover = rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( tex, rect, Color.White * ( hover ? hilit : lit ) );

			if( hover ) {
				string str;

				switch( this.Layer ) {
				case PaintLayerType.Foreground:
					str = "Foreground Only";
					break;
				case PaintLayerType.Background:
					str = "Background Only";
					break;
				case PaintLayerType.Anyground:
					str = "All layers";
					break;
				default:
					throw new NotImplementedException();
				}
				
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, str, rect.X, rect.Y - 16, Color.White, Color.Black, default( Vector2 ), 1f );
			}

			rect.X -= 4;
			rect.Y -= 4;
			rect.Width += 8;
			rect.Height += 8;
		}


		public void DrawOptionSize( SpriteBatch sb, int origin_x, int origin_y, out Rectangle rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			//float unlit = PaintBlasterUI.IdleScale;

			Texture2D tex;

			switch( this.BrushSize ) {
			case PaintBrushSize.Small:
				tex = PaintBlasterUI.SizeSmallTex;
				break;
			case PaintBrushSize.Large:
				tex = PaintBlasterUI.SizeLargeTex;
				break;
			default:
				throw new NotImplementedException();
			}

			var offset = new Vector2( tex.Width, tex.Height ) * 0.5f;

			int x = ( origin_x + options_dist ) - (int)offset.X;
			int y = ( origin_y + options_dist ) - (int)offset.Y;
			rect = new Rectangle( x, y, tex.Width, tex.Height );

			bool hover = rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( tex, rect, Color.White * ( hover ? hilit : lit ) );

			if( hover ) {
				string str;

				switch( this.BrushSize ) {
				case PaintBrushSize.Small:
					str = "Small brush";
					break;
				case PaintBrushSize.Large:
					str = "Large brush";
					break;
				default:
					throw new NotImplementedException();
				}
				
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, str, rect.X, rect.Y + rect.Height, Color.White, Color.Black, default( Vector2 ), 1f );
			}

			rect.X -= 4;
			rect.Y -= 4;
			rect.Width += 8;
			rect.Height += 8;
		}
		

		public void DrawOptionCopy( SpriteBatch sb, int origin_x, int origin_y, out Rectangle rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			//float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;

			Texture2D tex = Main.itemTexture[ ItemID.EmptyDropper ];

			var offset = new Vector2( tex.Width, tex.Height ) * 0.5f;

			int x = ( origin_x + options_dist ) - (int)offset.X;
			int y = ( origin_y - options_dist ) - (int)offset.Y;
			rect = new Rectangle( x, y, tex.Width, tex.Height );

			bool hover = rect.Contains( Main.mouseX, Main.mouseY );
			Color color = Color.White * ( this.IsCopying ? 1f : ( hover ? lit : unlit ) );

			sb.Draw( tex, rect, color );

			if( hover ) {
				string str = "Copy color";
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, str, rect.X, rect.Y - 16, Color.White, Color.Black, default( Vector2 ), 1f );
			}

			rect.X -= 4;
			rect.Y -= 4;
			rect.Width += 8;
			rect.Height += 8;
		}


		public void DrawOptionPressure( SpriteBatch sb, int origin_x, int origin_y, out Rectangle rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			//float unlit = PaintBlasterUI.IdleScale;

			Texture2D tex = this.PressurePercent >= 0.75 ? PaintBlasterUI.PressureHiTex :
				(this.PressurePercent <= 0.25f ? PaintBlasterUI.PressureLowTex : PaintBlasterUI.PressureMidTex);

			var offset = new Vector2( tex.Width, tex.Height ) * 0.5f;

			int x = ( origin_x - options_dist ) - (int)offset.X;
			int y = ( origin_y + options_dist ) - (int)offset.Y;
			rect = new Rectangle( x, y, tex.Width, tex.Height );

			bool hover = rect.Contains( Main.mouseX, Main.mouseY );
			Color color = Color.White * ( this.IsCopying ? 1f : ( hover ? hilit : lit ) );

			sb.Draw( tex, rect, color );

			if( hover ) {
				string str = "Brush pressure";
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, str, rect.X, rect.Y + rect.Height, Color.White, Color.Black, default( Vector2 ), 1f );
			}

			rect.X -= 4;
			rect.Y -= 4;
			rect.Width += 8;
			rect.Height += 8;
		}
	}
}
