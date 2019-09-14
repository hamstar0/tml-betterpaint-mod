using BetterPaint.Painting;
using BetterPaint.Painting.Brushes;
using BetterPaint.UI;
using HamstarHelpers.Helpers.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const int Width = 50;
		public const int Height = 18;


		////////////////

		public bool IsUsingUI { get; private set; }

		public PaintBrushType CurrentBrush {
			get { return this.UI.CurrentBrush; }
			private set { this.UI.CurrentBrush = value; }
		}
		public PaintLayerType Layer {
			get { return this.UI.Layer; }
			private set { this.UI.Layer = value; }
		}
		public PaintBrushSize BrushSize {
			get { return this.UI.BrushSize; }
			private set { this.UI.BrushSize = value; }
		}
		public float PressurePercent {
			get { return this.UI.PressurePercent; }
			private set { this.UI.PressurePercent = value; }
		}
		public int CurrentPaintItemInventoryIndex {
			get { return this.UI.CurrentPaintItemInventoryIndex; }
			private set { this.UI.CurrentPaintItemInventoryIndex = value; }
		}

		public bool IsCopying {
			get { return this.UI.IsCopying; }
			private set { this.UI.IsCopying = value; }
		}

		private PaintBlasterUI UI;
		private PaintBlasterHUD HUD;


		////////////////

		public PaintBlasterItem() : base() {
			this.IsUsingUI = false;
			this.UI = new PaintBlasterUI();
			this.HUD = new PaintBlasterHUD();
		}

		public override void SetStaticDefaults() {
			var mymod = (BetterPaintMod)this.mod;

			this.DisplayName.SetDefault( "Paint Blaster" );
			this.Tooltip.SetDefault( "Use regular paint or Color Cartridges to paint" + '\n' +
				"Overlays all existing paint" + '\n' +
				"Right-click to adjust settings" );
			
			PaintBlasterUI.InitializeStatic( mymod );
			PaintBlasterHUD.InitializeStatic( mymod );
		}


		public override void SetDefaults() {
			this.item.width = PaintBlasterItem.Width;
			this.item.height = PaintBlasterItem.Height;
			this.item.useStyle = 5;
			this.item.autoReuse = true;
			this.item.useAnimation = 3;
			this.item.useTime = 6;
			this.item.shoot = ProjectileID.PainterPaintball;
			this.item.UseSound = SoundID.Item34.WithVolume( 0.5f );
			this.item.shootSpeed = 6f;
			this.item.noMelee = true;
			this.item.value = Item.buyPrice( 2, 50, 0, 0 );
			this.item.rare = 8;
			this.item.ranged = true;
		}


		////////////////

		public void CheckSettings( Player player ) {
			var mymod = (BetterPaintMod)this.mod;

			if( this.IsCopying ) {
				var set = new HashSet<int> { mymod.ItemType<CopyCartridgeItem>() };
				var copyCartItem = PlayerItemFinderHelpers.FindFirstOfPossessedItemFor( player, set, false );

				if( copyCartItem == null ) {
					Main.NewText( "Copy Cartridges needed.", Color.Red );
					this.IsCopying = false;
				}
			}

			if( this.CurrentPaintItemInventoryIndex >= 0 ) {
				Item item = this.GetCurrentPaintItem();
				if( item == null ) {
					this.CurrentPaintItemInventoryIndex = -1;
				}
			}
		}


		////////////////

		public Item GetCurrentPaintItem() {
			if( this.CurrentPaintItemInventoryIndex == -1 ) {
				return null;
			}

			Item paintItem = Main.LocalPlayer.inventory[ this.CurrentPaintItemInventoryIndex ];

			if( !PaintBlasterHelpers.IsPaint(paintItem) ) {
				this.CurrentPaintItemInventoryIndex = -1;
				return null;
			}

			return paintItem;
		}
	}
}
