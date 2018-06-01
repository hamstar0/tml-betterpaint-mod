using BetterPaint.Painting;
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
		public PaintLayer Layer {
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
		public int CurrentCartridgeInventoryIndex {
			get { return this.UI.CurrentCartridgeInventoryIndex; }
			private set { this.UI.CurrentCartridgeInventoryIndex = value; }
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
			this.Tooltip.SetDefault( "Paints with color cartridges in various ways." + '\n' +
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
