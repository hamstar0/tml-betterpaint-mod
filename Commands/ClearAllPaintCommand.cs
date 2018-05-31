using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Commands {
	class ClearAllPaintCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "paintclearall"; } }
		public override string Usage { get { return "/paintclearall"; } }
		public override string Description { get { return "Gives player a random-hued color cartridge."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			myworld.BgColors.Colors.Clear();
			myworld.FgColors.Colors.Clear();

			caller.Reply( "All world paint reset." );
		}
	}
}
