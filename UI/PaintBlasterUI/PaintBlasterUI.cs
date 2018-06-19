using BetterPaint.Painting.Brushes;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;


namespace BetterPaint.UI {
	partial class PaintBlasterUI {
		public static Texture2D BrushStream { get; private set; }
		public static Texture2D BrushSpray { get; private set; }
		public static Texture2D BrushSpatter { get; private set; }
		public static Texture2D BrushEraser { get; private set; }

		public static Texture2D LayerFgTex { get; private set; }
		public static Texture2D LayerBgTex { get; private set; }
		public static Texture2D LayerBothTex { get; private set; }

		public static Texture2D SizeSmallTex { get; private set; }
		public static Texture2D SizeLargeTex { get; private set; }

		public static Texture2D PressureLowTex { get; private set; }
		public static Texture2D PressureMidTex { get; private set; }
		public static Texture2D PressureHiTex { get; private set; }


		static PaintBlasterUI() {
			PaintBlasterUI.BrushStream = null;
			PaintBlasterUI.BrushSpray = null;
			PaintBlasterUI.BrushSpatter = null;
			PaintBlasterUI.BrushEraser = null;
			PaintBlasterUI.LayerFgTex = null;
			PaintBlasterUI.LayerBgTex = null;
			PaintBlasterUI.LayerBothTex = null;
			PaintBlasterUI.SizeSmallTex = null;
			PaintBlasterUI.SizeLargeTex = null;
			PaintBlasterUI.PressureLowTex = null;
			PaintBlasterUI.PressureMidTex = null;
			PaintBlasterUI.PressureHiTex = null;
		}

		public static void InitializeStatic( BetterPaintMod mymod ) {
			if( PaintBlasterUI.BrushStream == null ) {
				PaintBlasterUI.BrushStream = mymod.GetTexture( "UI/PaintBlasterUI/BrushStream" );
				PaintBlasterUI.BrushSpray = mymod.GetTexture( "UI/PaintBlasterUI/BrushSpray" );
				PaintBlasterUI.BrushSpatter = mymod.GetTexture( "UI/PaintBlasterUI/BrushSpatter" );
				PaintBlasterUI.BrushEraser = mymod.GetTexture( "UI/PaintBlasterUI/BrushEraser" );

				PaintBlasterUI.LayerFgTex = mymod.GetTexture( "UI/PaintBlasterUI/LayerFg" );
				PaintBlasterUI.LayerBgTex = mymod.GetTexture( "UI/PaintBlasterUI/LayerBg" );
				PaintBlasterUI.LayerBothTex = mymod.GetTexture( "UI/PaintBlasterUI/LayerBoth" );

				PaintBlasterUI.SizeSmallTex = mymod.GetTexture( "UI/PaintBlasterUI/SizeSmall" );
				PaintBlasterUI.SizeLargeTex = mymod.GetTexture( "UI/PaintBlasterUI/SizeLarge" );

				PaintBlasterUI.PressureLowTex = mymod.GetTexture( "UI/PaintBlasterUI/PressureLow" );
				PaintBlasterUI.PressureMidTex = mymod.GetTexture( "UI/PaintBlasterUI/PressureMid" );
				PaintBlasterUI.PressureHiTex = mymod.GetTexture( "UI/PaintBlasterUI/PressureHi" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					PaintBlasterUI.BrushStream = null;
					PaintBlasterUI.BrushSpray = null;
					PaintBlasterUI.BrushSpatter = null;
					PaintBlasterUI.BrushEraser = null;
					PaintBlasterUI.LayerFgTex = null;
					PaintBlasterUI.LayerBgTex = null;
					PaintBlasterUI.LayerBothTex = null;
					PaintBlasterUI.SizeSmallTex = null;
					PaintBlasterUI.SizeLargeTex = null;
					PaintBlasterUI.PressureLowTex = null;
					PaintBlasterUI.PressureMidTex = null;
					PaintBlasterUI.PressureHiTex = null;
				} );
			}
		}


		////////////////

		public bool IsInteractingWithUI { get; private set; }

		public PaintBrushType CurrentBrush = PaintBrushType.Stream;
		public int CurrentPaintItemInventoryIndex = -1;
		public PaintLayerType Layer = PaintLayerType.Foreground;
		public PaintBrushSize BrushSize = PaintBrushSize.Small;
		public float PressurePercent = 1f;
		public bool IsCopying = false;
	}
}
