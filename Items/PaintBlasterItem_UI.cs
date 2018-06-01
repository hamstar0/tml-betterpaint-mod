using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public void CheckMenu() {
			if( Main.mouseRight ) {
				this.IsUsingUI = true;
			} else if( this.IsUsingUI ) {
				this.IsUsingUI = false;
			}
		}


		////////////////

		public void DrawHUD( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.HUD.DrawHUD( mymod, sb, this );
		}


		public void DrawPainterUI( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.UI.DrawUI( mymod, sb );
		}


		public void DrawScreen( SpriteBatch sb ) {
			this.UI.DrawScreen( (BetterPaintMod)this.mod, sb );
		}
	}
}
