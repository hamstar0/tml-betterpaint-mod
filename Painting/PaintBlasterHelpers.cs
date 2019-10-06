using BetterPaint.Items;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace BetterPaint.Painting {
	public enum BlasterPaintType {
		Can,
		ColorCartridge,
		GlowCartridge
	}



	public static class PaintBlasterHelpers {
		public static string ColorString( Color color ) {
			return "R:"+color.R+", G:"+color.G+", B:"+color.B;
		}

		public static Color FullColor( Color color ) {
			return new Color( color.R, color.G, color.B, 255 );
		}

		public static bool IsPaint( Item item ) {
			if( item == null || item.IsAir ) { return false; }

			int paintType = item.type;

			if( paintType == ModContent.ItemType<ColorCartridgeItem>() ) {
				return true;
			}
			if( paintType == ModContent.ItemType<GlowCartridgeItem>() ) {
				return true;
			}
			if( ItemGroupIdentityHelpers.Paints.Group.Contains( paintType ) ) {
				return true;
			}
			return false;
		}

		public static BlasterPaintType GetPaintType( Item paintItem ) {
			int paintType = paintItem.type;

			if( paintType == ModContent.ItemType<ColorCartridgeItem>() ) {
				return BlasterPaintType.ColorCartridge;
			}
			if( paintType == ModContent.ItemType<GlowCartridgeItem>() ) {
				return BlasterPaintType.GlowCartridge;
			}
			if( ItemGroupIdentityHelpers.Paints.Group.Contains( paintType ) ) {
				return BlasterPaintType.Can;
			} else {
				throw new ModHelpersException( "Not implemented." );
			}
		}

		public static float GetPaintAmount( Item paintItem ) {
			BlasterPaintType paintType = PaintBlasterHelpers.GetPaintType( paintItem );

			switch( paintType ) {
			case BlasterPaintType.Can:
				return paintItem.stack;
			case BlasterPaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				return mycolorcart.Quantity;
			case BlasterPaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				return myglowcart.Quantity;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
		}

		public static Color GetPaintColor( Item paintItem ) {
			BlasterPaintType paintType = PaintBlasterHelpers.GetPaintType( paintItem );
			Color color;

			switch( paintType ) {
			case BlasterPaintType.Can:
				color = WorldGen.paintColor( paintItem.paint );
				break;
			case BlasterPaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				color = mycolorcart.MyColor;
				break;
			case BlasterPaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				color = myglowcart.MyColor;
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
			return color;
		}
		
		public static byte GetPaintGlow( Item paintItem ) {
			BlasterPaintType paintType = PaintBlasterHelpers.GetPaintType( paintItem );
			byte glow;

			switch( paintType ) {
			case BlasterPaintType.Can:
				glow = 0;
				break;
			case BlasterPaintType.ColorCartridge:
				glow = 0;
				break;
			case BlasterPaintType.GlowCartridge:
				glow = 255;
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}

			return glow;
		}


		////////////////

		public static void ConsumePaint( Item paintItem, float amount ) {
			BlasterPaintType paintType = PaintBlasterHelpers.GetPaintType( paintItem );

			switch( paintType ) {
			case BlasterPaintType.Can:
				paintItem.stack -= (int)amount;
				if( paintItem.stack < 0 ) { paintItem.stack = 0; }
				break;
			case BlasterPaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paintItem.modItem;
				mycolorcart.ConsumePaint( amount );
				break;
			case BlasterPaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paintItem.modItem;
				myglowcart.ConsumePaint( amount );
				break;
			default:
				throw new ModHelpersException( "Not implemented." );
			}
		}
	}
}
