using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public void DrawPainterUI( SpriteBatch sb ) {
			this.UI.DrawUI( (BetterPaintMod)this.mod, sb );
		}
	}
}
