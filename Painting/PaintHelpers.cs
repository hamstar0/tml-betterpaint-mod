using BetterPaint.Items;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace BetterPaint.Painting {
	public enum PaintType {
		Can,
		ColorCartridge,
		GlowCartridge
	}



	public static class PaintHelpers {
		public static string ColorString( Color color ) {
			return "R:"+color.R+", G:"+color.G+", B:"+color.B;
		}

		public static Color FullColor( Color color ) {
			return new Color( color.R, color.G, color.B, 255 );
		}

		public static bool IsPaint( Item item ) {
			if( item == null || item.IsAir ) { return false; }

			var mymod = BetterPaintMod.Instance;
			int paintType = item.type;

			if( paintType == mymod.ItemType<ColorCartridgeItem>() ) {
				return true;
			}
			if( paintType == mymod.ItemType<GlowCartridgeItem>() ) {
				return true;
			}
			if( ItemGroupIdentityHelpers.Paints.Group.Contains( paintType ) ) {
				return true;
			}
			return false;
		}

		public static PaintType GetPaintType( Item paintItem ) {
			var mymod = BetterPaintMod.Instance;
			int paintType = paintItem.type;

			if( paintType == mymod.ItemType<ColorCartridgeItem>() ) {
				return PaintType.ColorCartridge;
			}
			if( paintType == mymod.ItemType<GlowCartridgeItem>() ) {
				return PaintType.GlowCartridge;
			}
			if( ItemGroupIdentityHelpers.Paints.Group.Contains( paintType ) ) {
				return PaintType.Can;
			} else {
				throw new ModHelpersException( "Not implemented." );
			}
		}

		public static float GetPaintAmount( Item paintItem ) {
			PaintType paintType = PaintHelpers.GetPaintType( paintItem );

			switch( paintType ) {
			case PaintType.Can:
				return paintItem.stack;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				return mycolorcart.Quantity;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				return myglowcart.Quantity;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
		}

		public static Color GetPaintColor( Item paintItem ) {
			PaintType paintType = PaintHelpers.GetPaintType( paintItem );
			Color color;

			switch( paintType ) {
			case PaintType.Can:
				color = WorldGen.paintColor( paintItem.paint );
				break;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				color = mycolorcart.MyColor;
				break;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				color = myglowcart.MyColor;
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
			return color;
		}
		
		public static byte GetPaintGlow( Item paintItem ) {
			PaintType paintType = PaintHelpers.GetPaintType( paintItem );
			byte glow;

			switch( paintType ) {
			case PaintType.Can:
				glow = 0;
				break;
			case PaintType.ColorCartridge:
				glow = 0;
				break;
			case PaintType.GlowCartridge:
				glow = 255;
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}

			return glow;
		}


		////////////////

		public static void ConsumePaint( Item paintItem, float amount ) {
			PaintType paintType = PaintHelpers.GetPaintType( paintItem );

			switch( paintType ) {
			case PaintType.Can:
				paintItem.stack -= (int)amount;
				if( paintItem.stack < 0 ) { paintItem.stack = 0; }
				break;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				mycolorcart.ConsumePaint( amount );
				break;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				myglowcart.ConsumePaint( amount );
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
		}
	}
}
