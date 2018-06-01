using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
				
				float capacity_percent = (float)mycart.UsageRemaining / (float)mymod.Config.PaintCartridgeCapacity;

				int height = (int)( capacity_percent * 50f ) * 2;
				int top = 100 - height;

				sb.Draw( PaintBlasterHUD.AmmoTop, new Vector2( x, y + 16 + top ), mycart.MyColor );
				sb.Draw( PaintBlasterHUD.AmmoBot, new Vector2( x, y + 124 ), mycart.MyColor );
				sb.Draw( Main.magicPixel, new Rectangle( x + 4, y + 24 + top, 16, height ), mycart.MyColor );
			}
		}
	}
}
