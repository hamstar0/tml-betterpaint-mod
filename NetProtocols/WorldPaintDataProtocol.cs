using BetterPaint.Painting;
using HamstarHelpers.Components.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace BetterPaint.NetProtocols {
	class WorldPaintDataProtocol {
		private static object MyLock = new object();

		private static int CurrentPacketSetId = 0;

		private static IDictionary<int, IDictionary<int, string>> JsonSegSets = new Dictionary<int, IDictionary<int, string>>();
		private static IDictionary<int, int> JsonSegCounts = new Dictionary<int, int>();



		////////////////

		internal static void StoreSeg( int packetSetId, string jsonSeg, int jsonSegNum, int jsonSegMax ) {
			lock( WorldPaintDataProtocol.MyLock ) {
				if( !WorldPaintDataProtocol.JsonSegSets.ContainsKey( packetSetId ) ) {
					WorldPaintDataProtocol.JsonSegSets[packetSetId] = new Dictionary<int, string>();
				}
				WorldPaintDataProtocol.JsonSegSets[packetSetId][jsonSegNum] = jsonSeg;
				WorldPaintDataProtocol.JsonSegCounts[packetSetId] = jsonSegMax;

				if( WorldPaintDataProtocol.AttemptReassembly( packetSetId ) ) {
					WorldPaintDataProtocol.JsonSegCounts.Remove( packetSetId );
					WorldPaintDataProtocol.JsonSegSets.Remove( packetSetId );
				}
			}
		}
		

		private static bool AttemptReassembly( int packetSetId ) {
			int segMax = WorldPaintDataProtocol.JsonSegCounts[ packetSetId ];
			IDictionary<int, string> segSet = WorldPaintDataProtocol.JsonSegSets[ packetSetId ];

			for( int i = 0; i < segMax; i++ ) {
				if( !segSet.ContainsKey(i) ) {
					return false;
				}
			}

			string json = "";
			for( int i = 0; i < segMax; i++ ) {
				json += segSet[i];
			}

			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();
			myworld.Layers = (WorldPaintLayers)JsonConvert.DeserializeObject( json );
			return true;
		}


		////////////////

		public static void SendToClient( int toWho, int ignoreWho ) {
			var myworld = BetterPaintMod.Instance.GetModWorld<BetterPaintWorld>();
			string json = JsonConvert.SerializeObject( myworld.Layers );
			int segSize = ( 64 * 1024 ) - 1;
			IList<string> segs = new List<string>();

			for( int i = 0; i < json.Length; i += segSize ) {
				int len = Math.Min( segSize, json.Length - i );
				string seg = json.Substring( i, len );

				segs.Add( seg );
			}

			for( int i=0; i<segs.Count; i++ ) {
				WorldPaintDataSegmentProtocol.Send( WorldPaintDataProtocol.CurrentPacketSetId, segs[i], i, segs.Count, toWho, ignoreWho );
			}
			WorldPaintDataProtocol.CurrentPacketSetId++;
		}
	}




	class WorldPaintDataSegmentProtocol : PacketProtocolSendToClient {
		public static void Send( int packetSetId, string jsonSeg, int jsonSegNum, int jsonSegMax, int toWho, int ignoreWho ) {
			var protocol = new WorldPaintDataSegmentProtocol( packetSetId, jsonSeg, jsonSegNum, jsonSegMax );
			protocol.SendToClient( toWho, ignoreWho );
		}



		////////////////

		public int PacketSetId;
		public string JsonSeg;
		public int JsonSegNum;
		public int JsonSegMax;



		////////////////

		private WorldPaintDataSegmentProtocol( int packetSetId, string jsonSeg, int jsonSegNum, int jsonSegMax ) {
			this.PacketSetId = packetSetId;
			this.JsonSeg = jsonSeg;
			this.JsonSegNum = jsonSegNum;
			this.JsonSegMax = jsonSegMax;
		}

		private WorldPaintDataSegmentProtocol() { }

		////////////////

		protected override void InitializeServerSendData( int toWho ) { }

		protected override void Receive() {
			WorldPaintDataProtocol.StoreSeg( this.PacketSetId, this.JsonSeg, this.JsonSegNum, this.JsonSegMax );
		}
	}
}
