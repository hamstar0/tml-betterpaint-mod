using BetterPaint.Painting;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;


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

		public PaintBrushType CurrentBrush { get; private set; }
		public int CurrentCartridgeInventoryIndex { get; private set; }
		public PaintLayer Layer { get; private set; }
		public PaintBrushSize BrushSize { get; private set; }
		public float PressurePercent { get; private set; }
		public bool IsCopying { get; private set; }


		////////////////

		public PaintBlasterUI() : base() {
			this.CurrentBrush = PaintBrushType.Stream;
			this.CurrentCartridgeInventoryIndex = -1;
			this.Layer = PaintLayer.Foreground;
			this.BrushSize = PaintBrushSize.Small;
			this.PressurePercent = 1f;
		}


		////////////////
		
		private void UpdateUI( BetterPaintMod mymod, Player player ) {
			if( this.IsCopying ) {
				var set = new HashSet<int> { mymod.ItemType<CopyCartridgeItem>() };
				var copy_cart_item = PlayerItemFinderHelpers.FindFirstOfItemFor( player, set );

				if( copy_cart_item == null ) {
					Main.NewText( "Copy Cartridges needed.", Color.Red );
					this.IsCopying = false;
				}
			}
		}
	}
}
