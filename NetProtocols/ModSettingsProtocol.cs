using HamstarHelpers.Components.Network;


namespace BetterPaint.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public BetterPaintConfigData Data;


		////////////////

		private ModSettingsProtocol() { }

		protected override void InitializeServerSendData( int toWho ) {
			this.Data = BetterPaintMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveReply() {
			BetterPaintMod.Instance.ConfigJson.SetData( this.Data );
		}
	}
}
