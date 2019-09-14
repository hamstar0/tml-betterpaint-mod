using HamstarHelpers.Helpers.User;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace BetterPaint {
	public class BetterPaintConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		[Header("Debug modes")]
		[Label( "Debug mode: Info display" )]
		public bool DebugModeInfo = false;

		[Label( "Debug mode: Cheat mode" )]
		public bool DebugModeCheats = false;


		[Header("Paint settings")]
		[Label( "Paint Mixer recipe enabled" )]
		[DefaultValue(true)]
		public bool PaintMixerRecipeEnabled = true;

		[Label( "Paint Mixer recipe requires Blend-O-Matic" )]
		[DefaultValue( true )]
		public bool PaintMixerRecipeBlendOMatic = true;


		[Label( "Paint cartridge recipe enabled" )]
		[DefaultValue( true )]
		public bool PaintRecipeEnabled = true;

		[Label( "Paint cartridge recipe's needed Gels" )]
		[Range( 0, 999 )]
		[DefaultValue( 10 )]
		public int PaintRecipeGels = 10;

		[Label( "Paint cartridge recipe's needed Paint Cans" )]
		[Range( 0, 9999 )]
		[DefaultValue( 100 )]
		public int PaintRecipePaints = 100;

		[Label( "Paint cartridge capacity" )]
		[Range( 1, 9999999 )]
		[DefaultValue( 2000 )]
		public int PaintCartridgeCapacity = 2000;


		[Label( "Copy Paint Cartridge recipe enabled" )]
		[DefaultValue( true )]
		public bool CopyPaintRecipeEnabled = true;

		[Label( "Copy Paint Cartridge recipe's needed Mana Potions" )]
		[Range( 0, 999 )]
		[DefaultValue( 10 )]
		public int CopyPaintRecipeManaPotions = 10;

		[Label( "Copy Paint Cartridge recipe's needed Nanites" )]
		[Range( 0, 999 )]
		[DefaultValue( 5 )]
		public int CopyPaintRecipeNanites = 5;

		[Label( "Glow Paint Cartridge recipe enabled" )]
		[DefaultValue( true )]
		public bool GlowPaintRecipeEnabled = true;

		[Label( "Glow Paint Cartridge recipe's needed Jungle Spores" )]
		[Range( 0, 999 )]
		[DefaultValue( 5 )]
		public int GlowPaintRecipeSpores = 5;


		[Label( "Paint Blaster recipe enabled" )]
		[DefaultValue( true )]
		public bool PaintBlasterRecipeEnabled = true;

		[Label( "Paint Blaster recipe requires Clentaminator" )]
		[DefaultValue( true )]
		public bool PaintBlasterRecipeClentaminator = true;


		[Label( "Brush Size Multiplier" )]
		[Range( 0f, 256f )]
		[DefaultValue( 1 )]
		public float BrushSizeMultiplier = 1;

		[Label( "Brush Spatter Density" )]
		[Range( 0f, 256f )]
		[DefaultValue( 0.15f )]
		public float BrushSpatterDensity = 0.15f;


		[Header("UI settings")]
		[Label( "Hud Paint Ammo Offset X" )]
		[Range( -4096, 4096 )]
		[DefaultValue( -32 )]
		public int HudPaintAmmoOffsetX = -32;

		[Label( "Hud Paint Ammo Offset Y" )]
		[Range( -2160, 2160 )]
		[DefaultValue( -160 )]
		public int HudPaintAmmoOffsetY = -160;

		[Label( "Show Paint On Map" )]
		[DefaultValue( true )]
		public bool ShowPaintOnMap = true;


		[Header("Painter shop settings")]
		[Label( "Painter Sells Blaster" )]
		[DefaultValue( false )]
		public bool PainterSellsBlaster = false;

		[Label( "Painter Sells RGB Cartridges" )]
		[DefaultValue( true )]
		public bool PainterSellsRGBCartridges = true;

		[Label( "Painter Sells Copy Cartridge" )]
		[DefaultValue( false )]
		public bool PainterSellsCopyCartridge = false;

		[Label( "Painter Sells Paint Mixer" )]
		[DefaultValue( false )]
		public bool PainterSellsPaintMixer = false;



		////////////////

		public override bool AcceptClientChanges( ModConfig pendingConfig, int whoAmI, ref string message ) {
			if( !UserHelpers.HasBasicServerPrivilege( Main.player[whoAmI] ) ) {
				message = "Insufficient privilege.";
				return false;
			}
			return true;
		}
	}
}
