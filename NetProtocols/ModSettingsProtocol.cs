using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace BetterPaint.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public BetterPaintConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		protected override void SetServerDefaults() {
			this.Data = BetterPaintMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveWithClient() {
			BetterPaintMod.Instance.ConfigJson.SetData( this.Data );
		}
	}
}
