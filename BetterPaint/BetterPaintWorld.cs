using BetterPaint.Painting;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public WorldPaintLayers Layers { get; internal set; }


		////////////////

		public override void Initialize() {
			this.Layers = new WorldPaintLayers();
		}

		////////////////

		public override void Load( TagCompound tags ) {
			this.Layers.Load( (BetterPaintMod)this.mod, tags );
		}

		public override TagCompound Save() {
			return this.Layers.Save();
		}
	}
}
