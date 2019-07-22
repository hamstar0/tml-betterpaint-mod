using HamstarHelpers.Helpers.User;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace BetterPaint {
	public class BetterPaintConfig : ModConfig {
		[Header("Debug modes")]
		[Label( "Debug Mode Info" )]
		public bool DebugModeInfo = false;

		[Label( "Debug Mode Cheats" )]
		public bool DebugModeCheats = false;


		[Header("Paint settings")]
		[Label( "Paint Mixer Recipe Enabled" )]
		[DefaultValue(true)]
		public bool PaintMixerRecipeEnabled = true;

		[Label( "Paint Mixer Recipe Blend-O-Matic" )]
		[DefaultValue( true )]
		public bool PaintMixerRecipeBlendOMatic = true;


		[Label( "Paint Recipe Enabled" )]
		[DefaultValue( true )]
		public bool PaintRecipeEnabled = true;

		[Label( "Paint Recipe Gels" )]
		[DefaultValue( 10 )]
		public int PaintRecipeGels = 10;

		[Label( "Paint Recipe Paints" )]
		[DefaultValue( 100 )]
		public int PaintRecipePaints = 100;

		[Label( "Paint Cartridge Capacity" )]
		[DefaultValue( 2000 )]
		public int PaintCartridgeCapacity = 2000;


		[Label( "Copy Paint Recipe Enabled" )]
		[DefaultValue( true )]
		public bool CopyPaintRecipeEnabled = true;
		[Label( "Copy Paint Recipe Mana Potions" )]
		[DefaultValue( 10 )]
		public int CopyPaintRecipeManaPotions = 10;
		[Label( "Copy Paint Recipe Nanites" )]
		[DefaultValue( 5 )]
		public int CopyPaintRecipeNanites = 5;

		[Label( "Glow Paint Recipe Enabled" )]
		[DefaultValue( true )]
		public bool GlowPaintRecipeEnabled = true;
		[Label( "Glow Paint Recipe Spores" )]
		[DefaultValue( 5 )]
		public int GlowPaintRecipeSpores = 5;

		[Label( "Paint Blaster Recipe Enabled" )]
		[DefaultValue( true )]
		public bool PaintBlasterRecipeEnabled = true;
		[Label( "Paint Blaster Recipe Clentaminator" )]
		[DefaultValue( true )]
		public bool PaintBlasterRecipeClentaminator = true;

		[Label( "Brush Size Multiplier" )]
		[DefaultValue( 1 )]
		public float BrushSizeMultiplier = 1;

		[Label( "Brush Spatter Density" )]
		[DefaultValue( 0.15f )]
		public float BrushSpatterDensity = 0.15f;


		[Header("UI settings")]
		[Label( "Hud Paint Ammo Offset X" )]
		[DefaultValue( -32 )]
		public int HudPaintAmmoOffsetX = -32;

		[Label( "Hud Paint Ammo Offset Y" )]
		[DefaultValue( -160 )]
		public int HudPaintAmmoOffsetY = -160;

		[Label( "Show Paint On Map" )]
		[DefaultValue( true )]
		public bool ShowPaintOnMap = true;


		[Header("Painter shop settings")]
		[Label( "Painter Sells Blaster" )]
		public bool PainterSellsBlaster = false;

		[Label( "Painter Sells RGB Cartridges" )]
		[DefaultValue( true )]
		public bool PainterSellsRGBCartridges = true;

		[Label( "Painter Sells Copy Cartridge" )]
		public bool PainterSellsCopyCartridge = false;

		[Label( "Painter Sells Paint Mixer" )]
		public bool PainterSellsPaintMixer = false;



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public override bool AcceptClientChanges( ModConfig pendingConfig, int whoAmI, ref string message ) {
			if( !UserHelpers.HasBasicServerPrivilege( Main.player[whoAmI] ) ) {
				message = "Server privilege only.";
				return false;
			}
			return true;
		}
	}
}
