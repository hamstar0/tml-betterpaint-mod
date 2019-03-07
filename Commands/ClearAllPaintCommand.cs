using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Commands {
	class ClearAllPaintCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "bp-clear-all";
		public override string Usage => "/"+this.Command;
		public override string Description => "Clears all paint from the map.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (BetterPaintMod)this.mod;
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			myworld.Layers.Background.Colors.Clear();
			myworld.Layers.Foreground.Colors.Clear();

			caller.Reply( "All world paint reset." );
		}
	}
}
