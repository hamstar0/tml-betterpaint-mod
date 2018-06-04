﻿using BetterPaint.Items;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.TileHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ModLoader.IO;


namespace BetterPaint.Painting {
	public enum PaintType {
		Can,
		Cartridge
	}



	public class PaintData {
		public static bool IsPaint( Item item ) {
			if( item == null || item.IsAir ) { return false; }

			if( item.type == BetterPaintMod.Instance.ItemType<ColorCartridgeItem>() ) {
				return true;
			}
			if( ItemIdentityHelpers.Paints.Item2.Contains( item.type ) ) {
				return true;
			}
			return false;
		}

		public static PaintType GetPaintType( Item paint_item ) {
			if( paint_item.type == BetterPaintMod.Instance.ItemType<ColorCartridgeItem>() ) {
				return PaintType.Cartridge;
			}
			if( ItemIdentityHelpers.Paints.Item2.Contains( paint_item.type ) ) {
				return PaintType.Can;
			} else {
				throw new NotImplementedException();
			}
		}

		public static float GetPaintAmount( Item paint_item ) {
			PaintType paint_type = PaintData.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				return paint_item.stack;
			case PaintType.Cartridge:
				var mycart = (ColorCartridgeItem)paint_item.modItem;
				return mycart.PaintQuantity;
			default:
				throw new NotImplementedException();
			}
		}

		public static Color GetPaintColor( Item paint_item ) {
			PaintType paint_type = PaintData.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				return WorldGen.paintColor( paint_item.paint );
			case PaintType.Cartridge:
				var mycart = (ColorCartridgeItem)paint_item.modItem;
				return mycart.MyColor;
			default:
				throw new NotImplementedException();
			}
		}
		

		public static void ConsumePaint( Item paint_item, float amount ) {
			PaintType paint_type = PaintData.GetPaintType( paint_item );

			switch( paint_type ) {
			case PaintType.Can:
				paint_item.stack -= (int)amount;
				if( paint_item.stack < 0 ) { paint_item.stack = 0; }
				break;
			case PaintType.Cartridge:
				var mycart = (ColorCartridgeItem)paint_item.modItem;
				mycart.ConsumePaint( amount );
				break;
			default:
				throw new NotImplementedException();
			}
		}


		////////////////

		public IDictionary<ushort, IDictionary<ushort, Color>> Colors { get; private set; }


		////////////////

		public PaintData() {
			this.Colors = new Dictionary<ushort, IDictionary<ushort, Color>>();
		}


		////////////////
		
		public void Load( BetterPaintMod mymod, TagCompound tags, string prefix, bool is_foreground ) {
			var myworld = mymod.GetModWorld<BetterPaintWorld>();

			this.Colors.Clear();

			if( tags.ContainsKey( prefix + "_x" ) ) {
				int[] fg_x = tags.GetIntArray( prefix + "_x" );

				for( int i = 0; i < fg_x.Length; i++ ) {
					ushort tile_x = (ushort)fg_x[i];
					int[] fg_y = tags.GetIntArray( prefix + "_" + tile_x + "_y" );

					for( int j = 0; j < fg_y.Length; j++ ) {
						ushort tile_y = (ushort)fg_y[j];

						byte[] clr_arr = tags.GetByteArray( prefix + "_" + tile_x + "_" + tile_y );
						Color color = new Color( clr_arr[0], clr_arr[1], clr_arr[2], clr_arr[3] );

						Tile tile = Main.tile[tile_x, tile_y];

						if( is_foreground ) {
							if( TileHelpers.IsSolid( tile, true, true ) ) {
								this.SetColorAt( color, tile_x, tile_y );
							}
						} else {
							if( !TileHelpers.IsAir( tile ) && tile.wall != 0 ) {
								this.SetColorAt( color, tile_x, tile_y );
							}
						}
					}
				}
			}
		}


		public void Save( TagCompound tags, string prefix ) {
			int[] x_arr = this.Colors.Keys.Select( i => (int)i ).ToArray();

			tags.Set( prefix + "_x", x_arr );

			foreach( var kv in this.Colors ) {
				ushort tile_x = kv.Key;
				IDictionary<ushort, Color> y_col = kv.Value;
				int[] y_arr = y_col.Keys.Select( i => (int)i ).ToArray();

				tags.Set( prefix + "_" + tile_x + "_y", y_arr );

				foreach( var kv2 in y_col ) {
					ushort tile_y = kv2.Key;
					Color clr = kv2.Value;
					byte[] clr_bytes = new byte[] { clr.R, clr.G, clr.B, clr.A };

					tags.Set( prefix + "_" + tile_x + "_" + tile_y, clr_bytes );
				}
			}
		}


		////////////////

		public bool HasColor( ushort tile_x, ushort tile_y ) {
			return this.Colors.ContainsKey( tile_x ) && this.Colors[tile_x].ContainsKey( tile_y );
		}

		public Color GetColor( ushort tile_x, ushort tile_y ) {
			if( this.Colors.ContainsKey( tile_x ) ) {
				if( this.Colors[tile_x].ContainsKey( tile_y ) ) {
					return this.Colors[tile_x][tile_y];
				}
			}
			return Color.White;
		}


		public void SetColorAt( Color color, ushort tile_x, ushort tile_y ) {
			if( !this.Colors.ContainsKey(tile_x) ) {
				this.Colors[tile_x] = new Dictionary<ushort, Color>();
			}
			this.Colors[tile_x][tile_y] = color;
		}

		public void RemoveColorAt( ushort tile_x, ushort tile_y ) {
			if( this.Colors.ContainsKey( tile_x ) ) {
				this.Colors[tile_x].Remove( tile_y );
			}
		}
	}
}
