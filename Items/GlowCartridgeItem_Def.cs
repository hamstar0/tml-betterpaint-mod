using System.Collections.Generic;
using System.IO;
using BetterPaint.Painting;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint.Items {
	partial class GlowCartridgeItem : ModItem {
		public const int Width = 12;
		public const int Height = 16;

		public static Texture2D CartridgeTex { get; internal set; }
		public static Texture2D OverlayTex { get; internal set; }
		public static Texture2D GlowMask1Tex { get; internal set; }


		static GlowCartridgeItem() {
			GlowCartridgeItem.CartridgeTex = null;
			GlowCartridgeItem.OverlayTex = null;
			GlowCartridgeItem.GlowMask1Tex = null;
		}



		////////////////
		
		public float Quantity { get; private set; }
		public Color MyColor { get; private set; }
		private bool IsInitialized = false;


		////////////////

		public override bool CloneNewInstances { get { return true; } }

		public override ModItem Clone() {
			var clone = (GlowCartridgeItem)base.Clone();
			clone.Quantity = this.Quantity;
			clone.MyColor = this.MyColor;
			return clone;
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Glow Cartridge" );
			this.Tooltip.SetDefault( "Needs a Paint Blaster to use" + '\n' +
				"Produces glow-in-the-dark paint" + '\n' +
				"Craft with cartidges and glowing spores at a paint mixer" );

			if( GlowCartridgeItem.OverlayTex == null ) {
				GlowCartridgeItem.CartridgeTex = this.mod.GetTexture( "Items/GlowCartridgeItem" );
				GlowCartridgeItem.OverlayTex = this.mod.GetTexture( "Items/GlowCartridgeItem_Color" );
				GlowCartridgeItem.GlowMask1Tex = this.mod.GetTexture( "Items/GlowCartridgeItem_Glow1" );

				TmlLoadHelpers.AddModUnloadPromise( () => {
					GlowCartridgeItem.CartridgeTex = null;
					GlowCartridgeItem.OverlayTex = null;
					GlowCartridgeItem.GlowMask1Tex = null;
				} );
			}
		}


		public override void SetDefaults() {
			var mymod = (BetterPaintMod)this.mod;

			this.MyColor = Color.White;
			this.Quantity = mymod.Config.PaintCartridgeCapacity;

			this.item.width = GlowCartridgeItem.Width;
			this.item.height = GlowCartridgeItem.Height;
			this.item.ammo = this.item.type;
			this.item.value = Item.buyPrice( 0, 0, 50, 0 );
			this.item.rare = 3;
		}


		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			if( !this.IsInitialized ) { return; }

			var mymod = (BetterPaintMod)this.mod;
			float percent = this.Quantity / mymod.Config.PaintCartridgeCapacity;

			var tip1 = new TooltipLine( this.mod, "BetterPaint: Color Indicator", "Color value: " + PaintHelpers.ColorString( this.MyColor ) ) {
				overrideColor = this.MyColor
			};
			var tip2 = new TooltipLine( this.mod, "BetterPaint: Capacity", "Capacity: " + (int)( percent * 100 ) + "%" ) {
				overrideColor = ColorCartridgeItem.GetCapacityColor( percent )
			};

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( tag.ContainsKey( "color" ) ) {
				byte[] bytes = tag.GetByteArray( "color" );

				this.MyColor = new Color( bytes[0], bytes[1], bytes[2], bytes[3] );
			}
			if( tag.ContainsKey( "quantity" ) ) {
				this.Quantity = tag.GetFloat( "quantity" );
			}
			if( tag.ContainsKey( "is_init" ) ) {
				this.IsInitialized = tag.GetBool("is_init");
			}
		}

		public override TagCompound Save() {
			return new TagCompound {
				{ "quantity", this.Quantity },
				{ "color", new byte[] { this.MyColor.R, this.MyColor.G, this.MyColor.B, this.MyColor.A } },
				{ "is_init", this.IsInitialized }
			};
		}

		////////////////

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( (byte)this.MyColor.R );
			writer.Write( (byte)this.MyColor.G );
			writer.Write( (byte)this.MyColor.B );
			writer.Write( (byte)this.MyColor.A );
			writer.Write( (float)this.Quantity );
			writer.Write( (bool)this.IsInitialized );
		}

		public override void NetRecieve( BinaryReader reader ) {
			this.MyColor = new Color( reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte() );
			this.Quantity = reader.ReadSingle();
			this.IsInitialized = reader.ReadBoolean();
		}


		////////////////

		public void SetPaint( Color color, float amount ) {
			this.MyColor = color;
			this.Quantity = amount;
			this.IsInitialized = true;
		}


		public void ConsumePaint( float amount ) {
			this.Quantity = this.Quantity - amount;
			if( this.Quantity < 0 ) { this.Quantity = 0; }
		}
	}
}
