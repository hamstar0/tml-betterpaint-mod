using BetterPaint.Painting;
using HamstarHelpers.HudHelpers;
using HamstarHelpers.Utilities.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public const float SelectedScale = 0.85f;
		public const float HoveredScale = 0.6f;
		public const float IdleScale = 0.35f;

		public const int BrushesRingRadius = 72;
		public const int OptionsRingRadius = 28;


		////////////////

		public void DrawUI( BetterPaintMod mymod, SpriteBatch sb ) {
			Rectangle stream_rect, spray_rect, bucket_rect, scrape_rect;
			Rectangle bg_rect, size_rect, copy_rect;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			IDictionary<int, Rectangle> palette_rects = this.DrawColorPalette( mymod, sb );
			this.DrawBrushes( sb, out stream_rect, out spray_rect, out bucket_rect, out scrape_rect );
			this.DrawOptionLayer( sb, x, y, out bg_rect );
			this.DrawOptionSize( sb, x, y, out size_rect );
			this.DrawOptionCopy( sb, x, y, out copy_rect );

			if( Main.mouseLeft ) {
				if( !this.IsInteractingWithUI ) {
					this.IsInteractingWithUI = true;

					this.CheckUISettingsInteractions( ref bg_rect, ref size_rect, ref copy_rect );
					this.CheckUIBrushInteractions( ref stream_rect, ref spray_rect, ref bucket_rect, ref scrape_rect );
					this.CheckUIColorInteractions( palette_rects );
				}
			} else {
				this.IsInteractingWithUI = false;
			}
			
			this.UpdateUI( mymod, Main.LocalPlayer );

			this.PostDrawUI( sb );
		}


		private void PostDrawUI( SpriteBatch sb ) {
			if( this.IsCopying ) {
				Texture2D copy_tex = Main.itemTexture[ItemID.EmptyDropper];
				var mouse_pos = new Vector2( Main.mouseX, Main.mouseY );

				sb.Draw( copy_tex, mouse_pos, Color.White );
			}
		}


		////////////////

		public void DrawBrushes( SpriteBatch sb, out Rectangle stream_rect, out Rectangle spray_rect, out Rectangle spatter_rect, out Rectangle eraser_rect ) {
			int brushes_dist = PaintBlasterUI.BrushesRingRadius;
			float hilit = PaintBlasterUI.SelectedScale;
			float lit = PaintBlasterUI.HoveredScale;
			float unlit = PaintBlasterUI.IdleScale;
			int x = Main.screenWidth / 2;
			int y = Main.screenHeight / 2;

			Texture2D tex_stream = Main.itemTexture[ItemID.Paintbrush];
			Texture2D tex_spray = Main.itemTexture[ItemID.PaintSprayer];
			Texture2D tex_spatter = Main.itemTexture[ItemID.HoneyBucket];
			Texture2D tex_eraser = Main.itemTexture[ItemID.PaintScraper];

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
				sb.DrawString( Main.fontMouseText, "Stream Mode", new Vector2( stream_rect.X, stream_rect.Y + stream_rect.Height ), tool_color );
			} else if( spray_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Spray ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Spray Mode", new Vector2( spray_rect.X, spray_rect.Y + spray_rect.Height ), tool_color );
			} else if( spatter_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Spatter ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Spatter Mode", new Vector2( spatter_rect.X, spatter_rect.Y + spatter_rect.Height ), tool_color );
			} else if( eraser_hover ) {
				var tool_color = this.CurrentBrush == PaintBrushType.Erase ? Color.White : Color.LightGray;
				sb.DrawString( Main.fontMouseText, "Eraser Mode", new Vector2( eraser_rect.X, eraser_rect.Y + eraser_rect.Height ), tool_color );
			}

			switch( this.CurrentBrush ) {
			case PaintBrushType.Stream:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, stream_rect, 2 );
				break;
			case PaintBrushType.Spray:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, spray_rect, 2 );
				break;
			case PaintBrushType.Spatter:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, spatter_rect, 2 );
				break;
			case PaintBrushType.Erase:
				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, eraser_rect, 2 );
				break;
			}
		}


		////////////////
		
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


		////////////////

		public IDictionary<int, Rectangle> DrawColorPalette( BetterPaintMod mymod, SpriteBatch sb ) {
			IList<int> item_idxs = ColorCartridgeItem.GetPaintCartridges( Main.LocalPlayer );
			var rects = new Dictionary<int, Rectangle>();

			double angle_step = 360d / (double)item_idxs.Count;
			double angle = 0d;
			
			foreach( int idx in item_idxs ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * (Math.PI / 180d) ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * (Math.PI / 180d) ) );

				Item item = Main.LocalPlayer.inventory[ idx ];
				var myitem = (ColorCartridgeItem)item.modItem;

				rects[ idx ] = this.DrawPaintIcon( mymod, sb, myitem.MyColor, myitem.TimesUsed, x, y, (idx == this.CurrentCartridgeInventoryIndex) );

				angle += angle_step;
			}

			return rects;
		}


		////////////////

		public Rectangle DrawPaintIcon( BetterPaintMod mymod, SpriteBatch sb, Color color, float uses, int x, int y, bool is_selected ) {
			Texture2D cart_tex = ColorCartridgeItem.CartridgeTex;
			Texture2D over_tex = ColorCartridgeItem.OverlayTex;

			var rect = new Rectangle( x - (cart_tex.Width / 2), y - (cart_tex.Height / 2), cart_tex.Width, cart_tex.Height );
			bool is_hover = rect.Contains( Main.mouseX, Main.mouseY );
			float color_mul = is_selected ? PaintBlasterUI.SelectedScale :
				( is_hover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );

			sb.Draw( cart_tex, rect, Color.White * color_mul );
			sb.Draw( over_tex, rect, color * color_mul );

			if( is_hover ) {
				float percent = 1f - (uses / (float)mymod.Config.PaintCartridgeCapacity);
				Color text_color = ColorCartridgeItem.GetCapacityColor( percent );
				Color label_color = Color.White * PaintBlasterUI.HoveredScale;

				sb.DrawString( Main.fontMouseText, "Capacity:", new Vector2(Main.mouseX, Main.mouseY-16), label_color );
				sb.DrawString( Main.fontMouseText, (int)(percent * 100)+"%", new Vector2(Main.mouseX+72, Main.mouseY-16), text_color );

				string color_str = "R:"+color.R+" G:"+color.G+" B:"+color.B+" A:"+color.A;

				sb.DrawString( Main.fontMouseText, "Color:", new Vector2( Main.mouseX, Main.mouseY + 8 ), label_color );
				sb.DrawString( Main.fontMouseText, color_str, new Vector2( Main.mouseX+56, Main.mouseY + 8 ), color );
			}

			if( is_selected ) {
				Rectangle sel_rect = rect;
				sel_rect.X -= 3;
				sel_rect.Y -= 3;
				sel_rect.Width += 6;
				sel_rect.Height += 6;

				HudHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Strobe.CurrentColor, sel_rect, 2 );
			}

			return rect;
		}
	}
}
