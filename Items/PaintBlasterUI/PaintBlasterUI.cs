using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;


namespace BetterPaint.Items {
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
				PaintBlasterUI.BrushStream = mymod.GetTexture( "Items/PaintBlasterUI/BrushStream" );
				PaintBlasterUI.BrushSpray = mymod.GetTexture( "Items/PaintBlasterUI/BrushSpray" );
				PaintBlasterUI.BrushSpatter = mymod.GetTexture( "Items/PaintBlasterUI/BrushSpatter" );
				PaintBlasterUI.BrushEraser = mymod.GetTexture( "Items/PaintBlasterUI/BrushEraser" );

				PaintBlasterUI.LayerFgTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerFg" );
				PaintBlasterUI.LayerBgTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerBg" );
				PaintBlasterUI.LayerBothTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerBoth" );

				PaintBlasterUI.SizeSmallTex = mymod.GetTexture( "Items/PaintBlasterUI/SizeSmall" );
				PaintBlasterUI.SizeLargeTex = mymod.GetTexture( "Items/PaintBlasterUI/SizeLarge" );

				PaintBlasterUI.PressureLowTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureLow" );
				PaintBlasterUI.PressureMidTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureMid" );
				PaintBlasterUI.PressureHiTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureHi" );

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
		public PaintLayer Layer = PaintLayer.Foreground;
		public PaintBrushSize BrushSize = PaintBrushSize.Small;
		public float PressurePercent = 1f;
		public bool IsCopying = false;
	}
}
