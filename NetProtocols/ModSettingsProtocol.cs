using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace BetterPaint.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public BetterPaintConfigData Data;


		////////////////

		protected ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		protected override void InitializeServerSendData( int to_who ) {
			this.Data = BetterPaintMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveReply() {
			BetterPaintMod.Instance.ConfigJson.SetData( this.Data );
		}
	}
}
