using BetterPaint.Painting;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public bool IsModeSelecting { get; private set; }

		public PaintBrushType CurrentMode { get { return this.UI.CurrentBrush; } }
		public int CurrentCartridgeInventoryIndex { get { return this.UI.CurrentCartridgeInventoryIndex; } }
		public PaintLayer Layer { get { return this.UI.Layer; } }
		public PaintBrushSize BrushSize { get { return this.UI.BrushSize; } }
		public float PressurePercent { get { return this.UI.PressurePercent; } }
		public bool IsCopying { get { return this.UI.IsCopying; } }

		private PaintBlasterUI UI;


		////////////////

		public PaintBlasterItem() : base() {
			this.IsModeSelecting = false;
			this.UI = new PaintBlasterUI();
		}

		public override void SetStaticDefaults() {
			var mymod = (BetterPaintMod)this.mod;

			this.DisplayName.SetDefault( "Paint Blaster" );
			this.Tooltip.SetDefault( "Paints with color cartridges in various ways." + '\n' +
				"Overlays all existing paint" + '\n' +
				"Right-click to adjust settings" );

			PaintBlasterItem.InitializeStatic( mymod );
			PaintBlasterUI.InitializeStatic( mymod );
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


		public override void AddRecipes() {
			var recipe = new ModRecipe( this.mod );
			recipe.AddIngredient( ItemID.Clentaminator );
			recipe.AddIngredient( ItemID.Flamethrower );
			recipe.AddIngredient( ItemID.PaintSprayer );
			recipe.SetResult( this );
			recipe.AddRecipe();
		}


		////////////////

		public Item GetCurrentPaintItem() {
			if( this.CurrentCartridgeInventoryIndex == -1 ) {
				return null;
			}
			return Main.LocalPlayer.inventory[ this.CurrentCartridgeInventoryIndex ];
		}
	}
}
