using BetterPaint.Painting;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint {
	class BetterPaintWorld : ModWorld {
		public PaintLayers Layers { get; private set; }


		////////////////

		public override void Initialize() {
			this.Layers = new PaintLayers();
		}

		////////////////

		public override void Load( TagCompound tags ) {
			this.Layers.Load( tags );
		}

		public override TagCompound Save() {
			return this.Layers.Save();
		}
	}
}
