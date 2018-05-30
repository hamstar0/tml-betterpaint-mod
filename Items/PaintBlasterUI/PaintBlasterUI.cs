using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public static Texture2D BgOffButtonTex { get; private set; }
		public static Texture2D BgOnButtonTex { get; private set; }

		public static Texture2D BrushSmallTex { get; private set; }
		public static Texture2D BrushLargeTex { get; private set; }

		public static Texture2D EyedropperTex { get; private set; }

		public static Texture2D PressureLowTex { get; private set; }
		public static Texture2D PressureMidTex { get; private set; }
		public static Texture2D PressureHiTex { get; private set; }


		static PaintBlasterUI() {
			PaintBlasterUI.BgOffButtonTex = null;
			PaintBlasterUI.BgOnButtonTex = null;
			PaintBlasterUI.BrushSmallTex = null;
			PaintBlasterUI.BrushLargeTex = null;
			PaintBlasterUI.EyedropperTex = null;
			PaintBlasterUI.PressureLowTex = null;
			PaintBlasterUI.PressureMidTex = null;
			PaintBlasterUI.PressureHiTex = null;
		}

		public static void SetStaticDefaults( BetterPaintMod mymod ) {
			if( PaintBlasterUI.BgOffButtonTex == null ) {
				PaintBlasterUI.BgOffButtonTex = mymod.GetTexture( "Items/PaintBlasterUI/BgOffButton" );
				PaintBlasterUI.BgOnButtonTex = mymod.GetTexture( "Items/PaintBlasterUI/BgOnButton" );
				PaintBlasterUI.BrushSmallTex = mymod.GetTexture( "Items/PaintBlasterUI/BrushSmall" );
				PaintBlasterUI.BrushLargeTex = mymod.GetTexture( "Items/PaintBlasterUI/BrushLarge" );
				PaintBlasterUI.EyedropperTex = mymod.GetTexture( "Items/PaintBlasterUI/Eyedropper" );
				PaintBlasterUI.PressureLowTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureLow" );
				PaintBlasterUI.PressureMidTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureMid" );
				PaintBlasterUI.PressureHiTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureHi" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					PaintBlasterUI.BgOffButtonTex = null;
					PaintBlasterUI.BgOnButtonTex = null;
					PaintBlasterUI.BrushSmallTex = null;
					PaintBlasterUI.BrushLargeTex = null;
					PaintBlasterUI.EyedropperTex = null;
					PaintBlasterUI.PressureLowTex = null;
					PaintBlasterUI.PressureMidTex = null;
					PaintBlasterUI.PressureHiTex = null;
				} );
			}
		}


		////////////////

		public bool IsInteractingWithUI { get; private set; }

		public PaintModeType CurrentMode { get; private set; }
		public int CurrentCartridgeInventoryIndex { get; private set; }
		public bool Foreground { get; private set; }
		public int BrushSize { get; private set; }
		public float Pressure { get; private set; }
		public bool IsEyedropping { get; private set; }


		////////////////

		public PaintBlasterUI() : base() {
			this.CurrentMode = PaintModeType.Stream;
			this.CurrentCartridgeInventoryIndex = -1;
			this.Foreground = true;
			this.BrushSize = 1;
			this.Pressure = 1f;
		}
	}
}
