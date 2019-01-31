using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using Microsoft.Xna.Framework;
using BetterPaint.Items;
using System;
using Terraria;
using Terraria.ModLoader;
using BetterPaint.Painting;


namespace BetterPaint.Commands {
	class GivePaintCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "bp-paint-give"; } }
		public override string Usage { get { return "/"+this.Command; } }
		public override string Description { get { return "Gives player a random-hued color cartridge."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (BetterPaintMod)this.mod;
			int paintType = this.mod.ItemType<ColorCartridgeItem>();

			int itemIdx = ItemHelpers.CreateItem( Main.LocalPlayer.position, paintType, 1, ColorCartridgeItem.Width, ColorCartridgeItem.Height );
			Item paintItem = Main.item[itemIdx];

			if( paintItem == null || paintItem.IsAir ) {
				throw new Exception( "Could not create cheaty paint." );
			}

			Func<byte> rand = () => (byte)Main.rand.Next( 0, 255 );
			var randClr = new Color( rand(), rand(), rand() );

			var myitem = (ColorCartridgeItem)paintItem.modItem;
			myitem.SetPaint( randClr, mymod.Config.PaintCartridgeCapacity );
			
			caller.Reply( "Random color cartridge created: " + PaintHelpers.ColorString(randClr), randClr );
		}
	}
}
