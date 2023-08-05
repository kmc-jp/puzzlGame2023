using K.NetworkDiscoveryExt;
using LobbyScene.RoomBanner;
using Mirror;
using RoomScene.StartButton;
using UnityEngine;

namespace K.NetworkRoomManagerExt {

    /// <summary>
    /// Manages network processing related to lobby, room, and gameplay.
    /// </summary>
    // REVIEW:
    // The network managing GameObject which this component attaches to supposes to manage the network over three scenes, 
    // lobby, room, and gameplay, but it is not the only case necessary for network management. Therefore, I named this class MatchNetworkRoomManager 
    // with a focus on the usage of network management, but it does not seem to be the best best choice. Is there any better naming?
    [DisallowMultipleComponent]
    public class MatchNetworkRoomManager : NetworkRoomManager {
        public override void OnRoomStartHost() {
            base.OnRoomStartHost();

            MatchNetworkDiscoveryHandler.Singleton.CallAdvertiseServer();
        }

        public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
            base.OnRoomServerPlayersReady();

            var banner = RoomBannerCreator.Singleton.Create();
            banner.GetComponent<RoomBannerProfile>().Address = conn.address;

            NetworkServer.Spawn(banner);
        }

        public override void OnRoomServerPlayersReady() {
            // base.OnRoomServerPlayersReady();

            StartButtonDisplay.Singleton.TargetShow(StartButtonDisplay.Singleton.connectionToClient);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();

            StartButtonDisplay.Singleton.TargetHide(StartButtonDisplay.Singleton.connectionToClient);
        }
    }
}