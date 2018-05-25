using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const int Width = 50;
		public const int Height = 18;

		////////////////

		public bool IsModeSelecting { get; private set; }
		public PaintMode CurrentMode { get; private set; }
		public int CurrentCartridgeInventoryIndex { get; private set; }


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Paint Blaster" );
			this.Tooltip.SetDefault( "Paints with color cartridges in various ways." + '\n' +
				"Overlays all existing paint" + '\n' +
				"Right-click to adjust settings" );
		}

		public override void SetDefaults() {
			this.IsModeSelecting = false;
			this.CurrentMode = PaintMode.Stream;
			this.CurrentCartridgeInventoryIndex = -1;

			this.item.width = PaintBlasterItem.Width;
			this.item.height = PaintBlasterItem.Height;
			this.item.useStyle = 5;
			this.item.autoReuse = true;
			this.item.useAnimation = 3;
			this.item.useTime = 6;
			this.item.shoot = ProjectileID.PainterPaintball;
			this.item.UseSound = SoundID.Item34;
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
