using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class ColorCartridgeItem : ModItem {
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			var mymod = (BetterPaintMod)this.mod;
			
			sb.Draw( ColorCartridgeItem.OverlayTex, pos, frame, this.MyColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_color, float rotation, float scale, int whoAmI ) {
			var mymod = (BetterPaintMod)this.mod;
			var pos = new Vector2( this.item.position.X - Main.screenPosition.X, this.item.position.Y - Main.screenPosition.Y );

			sb.Draw( ColorCartridgeItem.OverlayTex, pos, light_color.MultiplyRGBA( this.MyColor ) );
		}
	}
}
