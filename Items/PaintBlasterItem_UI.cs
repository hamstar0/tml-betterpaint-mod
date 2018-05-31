using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const int Width = 50;
		public const int Height = 18;

		public static Texture2D AmmoCan { get; private set; }
		public static Texture2D AmmoTop { get; private set; }
		public static Texture2D AmmoBot { get; private set; }


		static PaintBlasterItem() {
			PaintBlasterItem.AmmoCan = null;
			PaintBlasterItem.AmmoTop = null;
			PaintBlasterItem.AmmoBot = null;
		}

		public static void InitializeStatic( BetterPaintMod mymod ) {
			if( PaintBlasterItem.AmmoCan == null ) {
				PaintBlasterItem.AmmoCan = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoContainer" );
				PaintBlasterItem.AmmoTop = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoTop" );
				PaintBlasterItem.AmmoBot = mymod.GetTexture( "Items/PaintBlasterHUD/PaintAmmoBottom" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					PaintBlasterItem.AmmoCan = null;
					PaintBlasterItem.AmmoTop = null;
					PaintBlasterItem.AmmoBot = null;
				} );
			}
		}



		////////////////

		public void DrawHUD( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;

			int x = mymod.Config.HudPaintAmmoOffsetX >= 0 ?
				mymod.Config.HudPaintAmmoOffsetX :
				( Main.screenWidth + mymod.Config.HudPaintAmmoOffsetX );
			int y = mymod.Config.HudPaintAmmoOffsetY >= 0 ?
				mymod.Config.HudPaintAmmoOffsetY :
				( Main.screenHeight + mymod.Config.HudPaintAmmoOffsetY );

			sb.Draw( PaintBlasterItem.AmmoCan, new Vector2( x, y ), Color.White );

			Item cart_item = this.GetCurrentPaintItem();
			if( cart_item != null ) {
				var mycart = (ColorCartridgeItem)cart_item.modItem;

				float remaining_capacity = mycart.RemainingCapacity();
				float capacity_percent = (float)remaining_capacity / (float)mymod.Config.PaintCartridgeCapacity;

				int height = (int)( capacity_percent * 50f ) * 2;
				int top = 100 - height;

				sb.Draw( PaintBlasterItem.AmmoTop, new Vector2( x, y + 16 + top ), mycart.MyColor );
				sb.Draw( PaintBlasterItem.AmmoBot, new Vector2( x, y + 124 ), mycart.MyColor );
				sb.Draw( Main.magicPixel, new Rectangle( x + 4, y + 24 + top, 16, height ), mycart.MyColor );
			}
		}


		public void DrawPainterUI( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.UI.DrawUI( mymod, sb );
		}
	}
}
