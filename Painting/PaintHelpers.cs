using BetterPaint.Items;
using HamstarHelpers.Helpers.ItemHelpers;
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
			int paint_type = item.type;

			if( paint_type == mymod.ItemType<ColorCartridgeItem>() ) {
				return true;
			}
			if( paint_type == mymod.ItemType<GlowCartridgeItem>() ) {
				return true;
			}
			if( ItemIdentityHelpers.Paints.Item2.Contains( paint_type ) ) {
				return true;
			}
			return false;
		}

		public static PaintType GetPaintType( Item paint_item ) {
			var mymod = BetterPaintMod.Instance;
			int paint_type = paint_item.type;

			if( paint_type == mymod.ItemType<ColorCartridgeItem>() ) {
				return PaintType.ColorCartridge;
			}
			if( paint_type == mymod.ItemType<GlowCartridgeItem>() ) {
				return PaintType.GlowCartridge;
			}
			if( ItemIdentityHelpers.Paints.Item2.Contains( paint_type ) ) {
				return PaintType.Can;
			} else {
				throw new NotImplementedException();
			}
		}

		public static float GetPaintAmount( Item paint_item ) {
			PaintType paint_type = PaintHelpers.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				return paint_item.stack;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paint_item.modItem;
				return mycolorcart.Quantity;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paint_item.modItem;
				return myglowcart.Quantity;
			default:
				throw new NotImplementedException();
			}
		}

		public static Color GetPaintColor( Item paint_item ) {
			PaintType paint_type = PaintHelpers.GetPaintType( paint_item );
			Color color;

			switch( paint_type ) {
			case PaintType.Can:
				color = WorldGen.paintColor( paint_item.paint );
				break;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paint_item.modItem;
				color = mycolorcart.MyColor;
				break;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paint_item.modItem;
				color = myglowcart.MyColor;
				break;
			default:
				throw new NotImplementedException();
			}
			return color;
		}
		
		public static byte GetPaintGlow( Item paint_item ) {
			PaintType paint_type = PaintHelpers.GetPaintType( paint_item );
			byte glow;

			switch( paint_type ) {
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
				throw new NotImplementedException();
			}

			return glow;
		}


		////////////////

		public static void ConsumePaint( Item paint_item, float amount ) {
			PaintType paint_type = PaintHelpers.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				paint_item.stack -= (int)amount;
				if( paint_item.stack < 0 ) { paint_item.stack = 0; }
				break;
			case PaintType.ColorCartridge:
				var mycolorcart = (ColorCartridgeItem)paint_item.modItem;
				mycolorcart.ConsumePaint( amount );
				break;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paint_item.modItem;
				myglowcart.ConsumePaint( amount );
				break;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
