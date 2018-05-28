using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace BetterPaint.Items {
	partial class PaintBlasterItem : ModItem {
		public const int Width = 50;
		public const int Height = 18;

		public static Texture2D BgOffButtonTex { get; internal set; }
		public static Texture2D BgOnButtonTex { get; internal set; }
		public static Texture2D BrushSmallTex { get; internal set; }
		public static Texture2D BrushLargeTex { get; internal set; }
		
		static PaintBlasterItem() {
			PaintBlasterItem.BgOffButtonTex = null;
			PaintBlasterItem.BgOnButtonTex = null;
			PaintBlasterItem.BrushSmallTex = null;
			PaintBlasterItem.BrushLargeTex = null;
		}

		////////////////

		public bool IsModeSelecting { get; private set; }
		public bool IsInteractingWithUI { get; private set; }

		public PaintMode CurrentMode { get; private set; }
		public int CurrentCartridgeInventoryIndex { get; private set; }
		public bool Foreground { get; private set; }
		public int BrushSize { get; private set; }


		////////////////

		public PaintBlasterItem() : base() {
			this.IsModeSelecting = false;
			this.CurrentMode = PaintMode.Stream;
			this.CurrentCartridgeInventoryIndex = -1;
			this.Foreground = true;
			this.BrushSize = 1;
		}

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Paint Blaster" );
			this.Tooltip.SetDefault( "Paints with color cartridges in various ways." + '\n' +
				"Overlays all existing paint" + '\n' +
				"Right-click to adjust settings" );

			if( PaintBlasterItem.BgOffButtonTex == null ) {
				PaintBlasterItem.BgOffButtonTex = this.mod.GetTexture( "Items/PaintBlasterItem_BgOffButton" );
				PaintBlasterItem.BgOnButtonTex = this.mod.GetTexture( "Items/PaintBlasterItem_BgOnButton" );
				PaintBlasterItem.BrushSmallTex = this.mod.GetTexture( "Items/PaintBlasterItem_BrushSmall" );
				PaintBlasterItem.BrushLargeTex = this.mod.GetTexture( "Items/PaintBlasterItem_BrushLarge" );
			}

			TmlLoadHelpers.AddModUnloadPromise( () => {
				PaintBlasterItem.BgOffButtonTex = null;
				PaintBlasterItem.BgOnButtonTex = null;
				PaintBlasterItem.BrushSmallTex = null;
				PaintBlasterItem.BrushLargeTex = null;
			} );
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
