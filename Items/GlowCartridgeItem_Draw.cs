﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class GlowCartridgeItem : ModItem {
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle frame, Color draw_color, Color item_color, Vector2 origin, float scale ) {
			var mymod = (BetterPaintMod)this.mod;
			Texture2D tex = ColorCartridgeItem.OverlayTex;

			sb.Draw( tex, pos, frame, this.MyColor, 0f, default(Vector2), scale, SpriteEffects.None, 0f );

			int percent = (int)( 100 * ( (float)this.Quantity / (float)mymod.Config.PaintCartridgeCapacity ) );

			Utils.DrawBorderStringFourWay( sb, Main.fontItemStack, percent + "%", pos.X - 6, pos.Y + 10, this.MyColor * 0.75f, Color.Black * 0.5f, default( Vector2 ), 0.75f );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color light_color, Color alpha_color, float rotation, float scale, int whoAmI ) {
			var mymod = (BetterPaintMod)this.mod;
			var pos = new Vector2( this.item.position.X - Main.screenPosition.X, this.item.position.Y - Main.screenPosition.Y );
			Texture2D tex = ColorCartridgeItem.OverlayTex;

			sb.Draw( tex, pos, this.MyColor );
		}
	}
}
