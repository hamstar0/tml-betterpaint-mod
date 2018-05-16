using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace BetterPaint.Items {
	class ColorCartridgeItemData : GlobalItem {
		public int Uses { get; private set; }
		public Color MyColor { get; private set; }


		////////////////

		public ColorCartridgeItemData() : base() {
			this.Uses = 0;
			this.MyColor = Color.White;
		}
		

		public override bool InstancePerEntity { get { return true; } }

		public override GlobalItem Clone( Item item, Item item_clone ) {
			var myclone = (ColorCartridgeItemData)base.Clone( item, item_clone );
			myclone.MyColor = this.MyColor;
			return myclone;
		}

		////////////////

		public override bool NeedsSaving( Item item ) {
			return item.type == this.mod.ItemType<ColorCartridgeItem>();
		}

		public override void Load( Item item, TagCompound tag ) {
			if( tag.ContainsKey( "color" ) ) {
				byte[] bytes = tag.GetByteArray( "color" );

				this.MyColor = new Color( bytes[0], bytes[1], bytes[2], bytes[3] );
			}
		}

		public override TagCompound Save( Item item ) {
			return new TagCompound {
				{ "color", new byte[] { this.MyColor.R, this.MyColor.G, this.MyColor.B, this.MyColor.A } }
			};
		}

		////////////////

		public override void NetSend( Item item, BinaryWriter writer ) {
			if( this.NeedsSaving(item) ) {
				writer.Write( (byte)this.MyColor.R );
				writer.Write( (byte)this.MyColor.G );
				writer.Write( (byte)this.MyColor.B );
				writer.Write( (byte)this.MyColor.A );
			}
		}

		public override void NetReceive( Item item, BinaryReader reader ) {
			if( this.NeedsSaving( item ) ) {
				this.MyColor = new Color( reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte() );
			}
		}


		////////////////

		public void SetColor( Color color ) {
			this.MyColor = color;
		}

		public void SetUses( int uses ) {
			this.Uses = uses;
		}
	}
}
