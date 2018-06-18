using BetterPaint.Items;
using HamstarHelpers.ItemHelpers;
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
				return mycolorcart.PaintQuantity;
			case PaintType.GlowCartridge:
				var myglowcart = (GlowCartridgeItem)paint_item.modItem;
				return myglowcart.Quantity;
			default:
				throw new NotImplementedException();
			}
		}

		public static Color GetPaintColor( Item paint_item ) {
			PaintType paint_type = PaintHelpers.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				return WorldGen.paintColor( paint_item.paint );
			case PaintType.ColorCartridge:
				var mycart = (ColorCartridgeItem)paint_item.modItem;
				return mycart.MyColor;
			case PaintType.GlowCartridge:
				return Color.White;
			default:
				throw new NotImplementedException();
			}
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
				myglowcart.ConsumeQuantity( amount );
				break;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
