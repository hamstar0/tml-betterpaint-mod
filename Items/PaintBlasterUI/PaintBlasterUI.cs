﻿using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;


namespace BetterPaint.Items {
	partial class PaintBlasterUI {
		public static Texture2D LayerFgTex { get; private set; }
		public static Texture2D LayerBgTex { get; private set; }
		public static Texture2D LayerBothTex { get; private set; }

		public static Texture2D BrushSmallTex { get; private set; }
		public static Texture2D BrushLargeTex { get; private set; }

		public static Texture2D PressureLowTex { get; private set; }
		public static Texture2D PressureMidTex { get; private set; }
		public static Texture2D PressureHiTex { get; private set; }


		static PaintBlasterUI() {
			PaintBlasterUI.LayerFgTex = null;
			PaintBlasterUI.LayerBgTex = null;
			PaintBlasterUI.LayerBothTex = null;
			PaintBlasterUI.BrushSmallTex = null;
			PaintBlasterUI.BrushLargeTex = null;
			PaintBlasterUI.PressureLowTex = null;
			PaintBlasterUI.PressureMidTex = null;
			PaintBlasterUI.PressureHiTex = null;
		}

		public static void InitializeStatic( BetterPaintMod mymod ) {
			if( PaintBlasterUI.LayerFgTex == null ) {
				PaintBlasterUI.LayerFgTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerFg" );
				PaintBlasterUI.LayerBgTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerBg" );
				PaintBlasterUI.LayerBothTex = mymod.GetTexture( "Items/PaintBlasterUI/LayerBoth" );

				PaintBlasterUI.BrushSmallTex = mymod.GetTexture( "Items/PaintBlasterUI/BrushSmall" );
				PaintBlasterUI.BrushLargeTex = mymod.GetTexture( "Items/PaintBlasterUI/BrushLarge" );

				PaintBlasterUI.PressureLowTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureLow" );
				PaintBlasterUI.PressureMidTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureMid" );
				PaintBlasterUI.PressureHiTex = mymod.GetTexture( "Items/PaintBlasterUI/PressureHi" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					PaintBlasterUI.LayerFgTex = null;
					PaintBlasterUI.LayerBgTex = null;
					PaintBlasterUI.LayerBothTex = null;
					PaintBlasterUI.BrushSmallTex = null;
					PaintBlasterUI.BrushLargeTex = null;
					PaintBlasterUI.PressureLowTex = null;
					PaintBlasterUI.PressureMidTex = null;
					PaintBlasterUI.PressureHiTex = null;
				} );
			}
		}


		////////////////

		public bool IsInteractingWithUI { get; private set; }

		public PaintBrushType CurrentBrush = PaintBrushType.Stream;
		public int CurrentCartridgeInventoryIndex = -1;
		public PaintLayer Layer = PaintLayer.Foreground;
		public PaintBrushSize BrushSize = PaintBrushSize.Small;
		public float PressurePercent = 1f;
		public bool IsCopying = false;
	}
}
