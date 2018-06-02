﻿using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;


namespace BetterPaint.Items {
	class PaintBlasterHUD {
		public static Texture2D AmmoCan { get; private set; }
		public static Texture2D AmmoTop { get; private set; }
		public static Texture2D AmmoBot { get; private set; }


		static PaintBlasterHUD() {
			PaintBlasterHUD.AmmoCan = null;
			PaintBlasterHUD.AmmoTop = null;
			PaintBlasterHUD.AmmoBot = null;
		}

		public static void InitializeStatic( BetterPaintMod mymod ) {
			if( PaintBlasterHUD.AmmoCan == null ) {
				PaintBlasterHUD.AmmoCan = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoContainer" );
				PaintBlasterHUD.AmmoTop = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoTop" );
				PaintBlasterHUD.AmmoBot = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoBottom" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					PaintBlasterHUD.AmmoCan = null;
					PaintBlasterHUD.AmmoTop = null;
					PaintBlasterHUD.AmmoBot = null;
				} );
			}
		}


		////////////////

		public void DrawHUD( BetterPaintMod mymod, SpriteBatch sb, PaintBlasterItem myblaster ) {
			int x = mymod.Config.HudPaintAmmoOffsetX >= 0 ?
				mymod.Config.HudPaintAmmoOffsetX :
				( Main.screenWidth + mymod.Config.HudPaintAmmoOffsetX );
			int y = mymod.Config.HudPaintAmmoOffsetY >= 0 ?
				mymod.Config.HudPaintAmmoOffsetY :
				( Main.screenHeight + mymod.Config.HudPaintAmmoOffsetY );

			sb.Draw( PaintBlasterHUD.AmmoCan, new Vector2( x, y ), Color.White );

			Item cart_item = myblaster.GetCurrentPaintItem();
			if( cart_item != null ) {
				var mycart = (ColorCartridgeItem)cart_item.modItem;

				float capacity_percent = (float)mycart.PaintQuantity / (float)mymod.Config.PaintCartridgeCapacity;

				int height = (int)( capacity_percent * 50f ) * 2;
				int top = 100 - height;

				Color color = capacity_percent >= 0.01f ? mycart.MyColor : mycart.MyColor * 0.25f;

				sb.Draw( PaintBlasterHUD.AmmoTop, new Vector2( x, y + 16 + top ), color );
				sb.Draw( PaintBlasterHUD.AmmoBot, new Vector2( x, y + 124 ), color );
				sb.Draw( Main.magicPixel, new Rectangle( x + 4, y + 24 + top, 16, height ), color );

				if( Main.mouseX >= x && Main.mouseX < ( x + PaintBlasterHUD.AmmoCan.Width ) ) {
					if( Main.mouseY >= y && Main.mouseY < ( y + PaintBlasterHUD.AmmoCan.Height ) ) {
						string percent_str = capacity_percent.ToString( "P", CultureInfo.InvariantCulture );

						Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, percent_str, Main.mouseX-40, Main.mouseY+20, Color.White, Color.Black, default(Vector2), 1f );
						//sb.DrawString( Main.fontMouseText, percent_str, new Vector2(Main.mouseX, Main.mouseY), Color.White );
					}
				}
			}
		}
	}
}
