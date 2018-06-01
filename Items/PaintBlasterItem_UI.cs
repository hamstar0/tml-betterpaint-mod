using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public void DrawHUD( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.HUD.DrawHUD( mymod, sb, this );
		}


		public void DrawPainterUI( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.UI.DrawUI( mymod, sb );
		}
	}
}
