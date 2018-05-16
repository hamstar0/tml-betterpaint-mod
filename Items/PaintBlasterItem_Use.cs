using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public override void PostUpdate() {
			if( Main.mouseRight ) {
				if( !this.IsModeSwitching ) {
					this.IsModeSwitching = true;
					
					this.SetMode( ( this.Mode + 1 ) % 3 );
				}
			} else {
				this.IsModeSwitching = false;
			}
		}


		////////////////

		public void SetMode( int mode ) {
			this.Mode = mode;
		}
	}
}
