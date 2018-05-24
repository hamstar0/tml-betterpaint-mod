using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const int Width = 50;
		public const int Height = 18;


		public int Mode { get; private set; }
		private bool IsModeSwitching = false;


		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Paint Blaster" );
			this.Tooltip.SetDefault( "Paints with color cartridges in various ways." + '\n' +
				"Overlaps all existing regular paint" + '\n' +
				"Switch spray modes with right-click" );
		}

		public override void SetDefaults() {
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

		
		public override bool Shoot( Player player, ref Vector2 pos, ref float vel_x, ref float vel_y, ref int type, ref int dmg, ref float kb ) {
			Item mypaint_item = this.GetMyPaintItem();
			if( mypaint_item == null ) { return false; }

			var mypaint_data = mypaint_item.GetGlobalItem<ColorCartridgeItemData>();

			Dust.NewDust( pos, 8, 8, 2, vel_x, vel_y, 0, mypaint_data.MyColor, 1f );

			return false;
		}

		////////////////

		public Item GetMyPaintItem() {
			int paint_type = this.mod.ItemType<ColorCartridgeItem>();
			var inv = Main.LocalPlayer.inventory;

			for( int i=0; i<inv.Length; i++ ) {
				if( inv[i] != null && !inv[i].IsAir && inv[i].type == paint_type ) {
					return inv[i];
				}
			}
			return null;
		}
	}
}
