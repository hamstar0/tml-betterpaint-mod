using BetterPaint.Items;
using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;


namespace BetterPaint.UI {
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
				PaintBlasterHUD.AmmoCan = mymod.GetTexture( "UI/PaintBlasterHUD/PaintAmmoContainer" );
				PaintBlasterHUD.AmmoTop = mymod.GetTexture( "UI/PaintBlasterHUD/PaintAmmoTop" );
				PaintBlasterHUD.AmmoBot = mymod.GetTexture( "UI/PaintBlasterHUD/PaintAmmoBottom" );

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

			Item paint_item = myblaster.GetCurrentPaintItem();

			if( paint_item != null ) {
				Color paint_color = PaintHelpers.FullColor( PaintHelpers.GetPaintColor( paint_item ) );
				float quantity = PaintHelpers.GetPaintAmount( paint_item );

				float capacity_percent = (float)quantity / (float)mymod.Config.PaintCartridgeCapacity;

				int height = (int)( capacity_percent * 50f ) * 2;
				int top = 100 - height;

				Color color = capacity_percent >= 0.01f ? paint_color : paint_color * 0.25f;

				sb.Draw( PaintBlasterHUD.AmmoTop, new Vector2( x, y + 16 + top ), color );
				sb.Draw( PaintBlasterHUD.AmmoBot, new Vector2( x, y + 124 ), color );
				sb.Draw( Main.magicPixel, new Rectangle( x + 4, y + 24 + top, 16, height ), color );

				if( Main.mouseX >= x && Main.mouseX < ( x + PaintBlasterHUD.AmmoCan.Width ) ) {
					if( Main.mouseY >= y && Main.mouseY < ( y + PaintBlasterHUD.AmmoCan.Height ) ) {
						string percent_str = capacity_percent.ToString( "P", CultureInfo.InvariantCulture );

						Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, percent_str, Main.mouseX-40, Main.mouseY+16, Color.White, Color.Black, default(Vector2), 1f );
					}
				}
			} else {
				if( Main.mouseX >= x && Main.mouseX < ( x + PaintBlasterHUD.AmmoCan.Width ) ) {
					if( Main.mouseY >= y && Main.mouseY < ( y + PaintBlasterHUD.AmmoCan.Height ) ) {
						string str = "Color Cartridge needed";

						Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, str, Main.mouseX - 160, Main.mouseY + 16, Color.White, Color.Black, default( Vector2 ), 1f );
					}
				}
			}
		}
	}
}
