using HamstarHelpers.Components.Network;


namespace BetterPaint.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public BetterPaintConfigData Data;

		////////////////

		public override void SetServerDefaults() {
			this.Data = BetterPaintMod.Instance.Config;
		}

		protected override void ReceiveWithClient() {
			BetterPaintMod.Instance.ConfigJson.SetData( this.Data );
		}
	}
}
