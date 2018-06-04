using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public void CheckMenu() {
			if( Main.mouseRight ) {
				if( !this.IsUsingUI ) {
					this.IsUsingUI = true;
					Main.PlaySound( SoundID.MenuOpen );
				}
			} else if( this.IsUsingUI ) {
				this.IsUsingUI = false;
				Main.PlaySound( SoundID.MenuClose );
			}
		}


		////////////////

		public void DrawHUD( SpriteBatch sb ) {
			var mymod = (BetterPaintMod)this.mod;
			this.HUD.DrawHUD( mymod, sb, this );
		}


		public void DrawPainterUI( SpriteBatch sb ) {
			if( Main.playerInventory ) { return; }

			var mymod = (BetterPaintMod)this.mod;
			this.UI.DrawUI( mymod, sb );
		}


		public void DrawScreen( SpriteBatch sb ) {
			this.UI.DrawScreen( (BetterPaintMod)this.mod, sb );
		}
	}
}
