using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public static Color GetCapacityColor( float percent ) {
			return percent == 0f ? new Color( 64, 64, 64, 255 ) : (
				percent <= 0.15f ? Color.Red : (
					percent <= 0.35f ? Color.Yellow : (
						percent < 1.0f ? Color.White : Color.LimeGreen
					)
				)
			);
		}



		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			var mymod = (BetterPaintMod)this.mod;
			Texture2D tex = ColorCartridgeItem.ColorOverlayTex;

			sb.Draw( tex, pos, frame, this.MyColor, 0f, default(Vector2), scale, SpriteEffects.None, 0f );

			int percent = (int)( 100 * ( (float)this.Quantity / (float)mymod.Config.PaintCartridgeCapacity ) );

			Utils.DrawBorderStringFourWay( sb, Main.fontItemStack, percent + "%", pos.X - 6, pos.Y + 10, this.MyColor * 0.75f, Color.Black * 0.5f, default( Vector2 ), 0.75f );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_color, float rotation, float scale, int whoAmI ) {
			var mymod = (BetterPaintMod)this.mod;
			var pos = new Vector2( this.item.position.X - Main.screenPosition.X, this.item.position.Y - Main.screenPosition.Y );
			Texture2D overlay_tex = ColorCartridgeItem.ColorOverlayTex;

			sb.Draw( overlay_tex, pos, light_color.MultiplyRGBA( this.MyColor ) );
		}
	}
}
