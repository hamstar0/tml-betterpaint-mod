using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;


namespace BetterPaint.Painting.Brushes {
	public enum PaintLayerType : int {
		Foreground,
		Background,
		Anyground
	}

	
	public enum PaintBrushType : int {
		Stream,
		Spray,
		Spatter,
		Erase
	}


	public enum PaintBrushSize : int {
		Small,
		Large
	}



	public abstract class PaintBrush {
		public static float ComputeChangePercent( Color? old_color, Color new_color, byte old_glow, byte new_glow ) {
			return PaintBrush.ComputeColorChangePercent( old_color, new_color ) +
				(PaintBrush.ComputeGlowChangePercent( old_glow, new_glow ) / 4);
		}

		public static float ComputeColorChangePercent( Color? old_color, Color new_color ) {
			Color base_color = old_color != null ? (Color)old_color : Color.Transparent;

			Color diff_color = XnaColorHelpers.DifferenceRGB( base_color, new_color );
			float diff_value = XnaColorHelpers.AvgRGB( diff_color );

			return diff_value / 255f;
		}

		public static float ComputeGlowChangePercent( byte old_glow, byte new_glow ) {
			int diff = Math.Abs( new_glow - old_glow );

			return (float)diff / 255f;
		}
		
		////////////////

		public static Color GetBlendedColor( PaintLayer layer, Color rgb_color, float pressure_percent, ushort tile_x, ushort tile_y, out Color? old_color ) {
			Color blended_color;
			float old_pressure_percent;
			old_color = layer.GetRawColorAt( tile_x, tile_y );

			if( old_color != null ) {
				Color old_rgb_color = (Color)old_color;

				old_pressure_percent = old_rgb_color.A / 255f;
				float old_pressure_percent_offset = (1f - pressure_percent) * old_pressure_percent;
				
				blended_color = Color.Lerp( rgb_color, old_rgb_color, old_pressure_percent_offset );
			} else {
				old_pressure_percent = 0f;
				blended_color = rgb_color;
			}

			blended_color.A = (byte)(MathHelper.Lerp( old_pressure_percent, 1f, pressure_percent ) * 255f);

			return blended_color;
		}


		public static Color GetErasedColor( PaintLayer layer, float pressure_percent, ushort tile_x, ushort tile_y, out Color? old_color ) {
			old_color = layer.GetRawColorAt( tile_x, tile_y );
			if( old_color == null ) { return Color.Transparent; }

			Color blended_color = (Color)old_color;
			blended_color.A = (byte)( MathHelper.Lerp( blended_color.A, 0f, pressure_percent ) * 255f );

			return blended_color;
		}


		public static byte GetBlendedGlow( PaintLayer layer, byte glow, float pressure_percent, ushort tile_x, ushort tile_y, out byte old_glow ) {
			byte blended_glow;
			
			old_glow = layer.GetGlowAt( tile_x, tile_y );

			if( glow != 0 ) {
				byte pressured_glow = (byte)( (float)glow * pressure_percent );

				blended_glow = (byte)MathHelper.Lerp( (float)old_glow, 255f, (float)pressured_glow / 255f );
			} else {
				blended_glow = (byte)MathHelper.Lerp( (float)old_glow, 0f, pressure_percent );
			}

			return blended_glow;
		}


		////////////////

		public abstract float Apply( PaintLayer layer, Color color, byte glow, PaintBrushSize brush_size, float pressure_percent, int rand_seed, int world_x, int world_y );
	}
}
