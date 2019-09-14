using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Services.AnimatedColor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		public bool IsHoveringIcon( double paletteAngle, double angleStep ) {
			var screenMid = new Vector2( Main.screenWidth / 2, Main.screenHeight / 2 );
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			if( Vector2.Distance(screenMid, mousePos) < (PaintBlasterUI.BrushesRingRadius + 16) ) {
				return false;
			}

			double myangle = Math.Atan2( (double)( mousePos.Y - screenMid.Y ), (double)( mousePos.X - screenMid.X ) ) * ( 180d / Math.PI );
			myangle = myangle < 0 ? 360 + myangle : myangle;

			return Math.Abs( paletteAngle - myangle ) <= ( angleStep * 0.5d ) ||
				Math.Abs( (360 + paletteAngle) - myangle ) <= ( angleStep * 0.5d );
			//return rect.Contains( Main.mouseX, Main.mouseY );
		}



		public IDictionary<int, float> DrawColorPalette( BetterPaintMod mymod, SpriteBatch sb ) {
			IDictionary<string, PaintDisplayInfo> infoSet = PaintDisplayInfo.GetPaintSelection( Main.LocalPlayer );
			var angles = new Dictionary<int, float>( infoSet.Count );
			
			double angleStep = 360d / (double)infoSet.Count;
			double angle = 0d;
			double radpi = Math.PI / 180d;
			
			foreach( var info in infoSet.Values ) {
				int x = ( Main.screenWidth / 2 ) + (int)( 128d * Math.Cos( angle * radpi ) );
				int y = ( Main.screenHeight / 2 ) + (int)( 128d * Math.Sin( angle * radpi ) );
				int stack;
				float percent;
				Color color;

				info.GetDrawInfo( mymod, x, y, out color, out percent, out stack );
				
				this.DrawColorIcon( mymod, sb, info.PaintItem.type, color, percent, stack, x, y, angle, angleStep,
					(info.FirstInventoryIndex == this.CurrentPaintItemInventoryIndex) );

				angles[ info.FirstInventoryIndex ] = (float)angle;

				angle += angleStep;
			}

			return angles;
		}


		////////////////

		public Texture2D GetPaintTexture( BetterPaintMod mymod, int itemType ) {
			if( itemType == mymod.ItemType<ColorCartridgeItem>() ) {
				return ColorCartridgeItem.ColorCartridgeTex;
			} else if( itemType == mymod.ItemType<GlowCartridgeItem>() ) {
				return GlowCartridgeItem.GlowCartridgeTex;
			} else if( ItemGroupIdentityHelpers.Paints.Group.Contains( itemType ) ) {
				return Main.itemTexture[itemType];
			} else {
				throw new ModHelpersException( "Not implemented." );
			}
		}

		public Texture2D GetPaintOverlayTexture( BetterPaintMod mymod, int itemType, out bool hasGlow ) {
			if( itemType == mymod.ItemType<ColorCartridgeItem>() ) {
				hasGlow = false;
				return ColorCartridgeItem.ColorOverlayTex;
			} else if( itemType == mymod.ItemType<GlowCartridgeItem>() ) {
				hasGlow = true;
				return GlowCartridgeItem.ColorOverlayTex;
			} else if( ItemGroupIdentityHelpers.Paints.Group.Contains( itemType ) ) {
				hasGlow = false;
				return null;
			} else {
				throw new ModHelpersException( "Not implemented." );
			}
		}


		////////////////

		public Rectangle DrawColorIcon( BetterPaintMod mymod, SpriteBatch sb, int itemType, Color color, float amountPercent,
				int stack, int x, int y, double paletteAngle, double angleStep, bool isSelected ) {
			bool hasGlow;
			Texture2D cartTex = this.GetPaintTexture( mymod, itemType );
			Texture2D overTex = this.GetPaintOverlayTexture( mymod, itemType, out hasGlow );

			bool isHover = this.IsHoveringIcon( paletteAngle, angleStep );

			var rect = new Rectangle( x - (cartTex.Width / 2), y - (cartTex.Height / 2), cartTex.Width, cartTex.Height );
			float colorMul = isSelected ? PaintBlasterUI.SelectedScale :
				( isHover ? PaintBlasterUI.HoveredScale : PaintBlasterUI.IdleScale );

			sb.Draw( cartTex, rect, Color.White * colorMul );
			if( overTex != null ) {
				sb.Draw( overTex, rect, (hasGlow ? color : color * colorMul) );
			}

			if( isHover ) {
				Color textColor = ColorCartridgeItem.GetCapacityColor( amountPercent );
				Color labelColor = hasGlow ? Color.GreenYellow * PaintBlasterUI.HoveredScale
					: Color.White * PaintBlasterUI.HoveredScale;
				Color bgColor = Color.Black * PaintBlasterUI.HoveredScale;

				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Capacity:", Main.mouseX, Main.mouseY-16, labelColor, bgColor, default( Vector2 ), 1f );
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, (int)( amountPercent * 100 ) + "%", Main.mouseX+72, Main.mouseY - 16, labelColor, bgColor, default( Vector2 ), 1f );

				string colorStr = PaintBlasterHelpers.ColorString( color );

				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, "Color:", Main.mouseX, Main.mouseY + 8, labelColor, bgColor, default( Vector2 ), 1f );
				Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, colorStr, Main.mouseX + 56, Main.mouseY + 8, color, bgColor, default( Vector2 ), 1f );
			}

			if( isSelected ) {
				Rectangle selRect = rect;
				selRect.X -= 3;
				selRect.Y -= 3;
				selRect.Width += 6;
				selRect.Height += 6;

				HUDHelpers.DrawBorderedRect( sb, Color.Transparent, AnimatedColors.Air.CurrentColor * 0.5f, selRect, 2 );
			}

			Utils.DrawBorderStringFourWay( sb, Main.fontItemStack, stack + "", (rect.X+cartTex.Width)-4, (rect.Y+cartTex.Height)-12, Color.White, Color.Black, default( Vector2 ), 1f );

			return rect;
		}
	}
}
