using BetterPaint.Painting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public void DrawOptionLayer( SpriteBatch sb, int origin_x, int origin_y, out Rectangle layer_rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;

			Texture2D layer_tex;

			switch( this.Layer ) {
			case PaintLayer.Foreground:
				layer_tex = PaintBlasterUI.LayerFgTex;
				break;
			case PaintLayer.Background:
				layer_tex = PaintBlasterUI.LayerBgTex;
				break;
			case PaintLayer.Anyground:
				layer_tex = PaintBlasterUI.LayerBothTex;
				break;
			default:
				throw new NotImplementedException();
			}

			var layer_offset = new Vector2( layer_tex.Width, layer_tex.Height ) * 0.5f;

			int layer_x = ( origin_x - options_dist ) - (int)layer_offset.X;
			int layer_y = ( origin_y - options_dist ) - (int)layer_offset.Y;
			layer_rect = new Rectangle( layer_x, layer_y, layer_tex.Width, layer_tex.Height );

			bool bg_hover = layer_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( layer_tex, layer_rect, Color.White * ( bg_hover ? hilit : lit ) );

			if( bg_hover ) {
				string layer_str;

				switch( this.Layer ) {
				case PaintLayer.Foreground:
					layer_str = "Foreground Only";
					break;
				case PaintLayer.Background:
					layer_str = "Background Only";
					break;
				case PaintLayer.Anyground:
					layer_str = "All layers";
					break;
				default:
					throw new NotImplementedException();
				}

				sb.DrawString( Main.fontMouseText, layer_str, new Vector2( layer_rect.X, layer_rect.Y - 16 ), Color.White );
			}
		}

		public void DrawOptionSize( SpriteBatch sb, int origin_x, int origin_y, out Rectangle size_rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;

			Texture2D size_tex;

			switch( this.BrushSize ) {
			case PaintBrushSize.Small:
				size_tex = PaintBlasterUI.BrushSmallTex;
				break;
			case PaintBrushSize.Large:
				size_tex = PaintBlasterUI.BrushLargeTex;
				break;
			default:
				throw new NotImplementedException();
			}

			var size_offset = new Vector2( size_tex.Width, size_tex.Height ) * 0.5f;

			int size_x = ( origin_x + options_dist ) - (int)size_offset.X;
			int size_y = ( origin_y + options_dist ) - (int)size_offset.Y;
			size_rect = new Rectangle( size_x, size_y, size_tex.Width, size_tex.Height );

			bool size_hover = size_rect.Contains( Main.mouseX, Main.mouseY );

			sb.Draw( size_tex, size_rect, Color.White * ( size_hover ? hilit : lit ) );

			if( size_hover ) {
				string size_str;

				switch( this.BrushSize ) {
				case PaintBrushSize.Small:
					size_str = "Small brush";
					break;
				case PaintBrushSize.Large:
					size_str = "Large brush";
					break;
				default:
					throw new NotImplementedException();
				}

				sb.DrawString( Main.fontMouseText, size_str, new Vector2( size_rect.X, size_rect.Y + size_rect.Height ), Color.White );
			}
		}
		
		public void DrawOptionCopy( SpriteBatch sb, int origin_x, int origin_y, out Rectangle copy_rect ) {
			int options_dist = PaintBlasterUI.OptionsRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;

			Texture2D copy_tex = Main.itemTexture[ ItemID.EmptyDropper ];

			var copy_offset = new Vector2( copy_tex.Width, copy_tex.Height ) * 0.5f;

			int copy_x = ( origin_x + options_dist ) - (int)copy_offset.X;
			int copy_y = ( origin_y - options_dist ) - (int)copy_offset.Y;
			copy_rect = new Rectangle( copy_x, copy_y, copy_tex.Width, copy_tex.Height );

			bool copy_hover = copy_rect.Contains( Main.mouseX, Main.mouseY );
			Color copy_color = Color.White * ( this.IsCopying ? 1f : ( copy_hover ? lit : unlit ) );

			sb.Draw( copy_tex, copy_rect, copy_color );

			if( copy_hover ) {
				string copy_str = "Copy color";
				sb.DrawString( Main.fontMouseText, copy_str, new Vector2( copy_rect.X, copy_rect.Y - 16 ), Color.White );
			}
		}
	}
}
