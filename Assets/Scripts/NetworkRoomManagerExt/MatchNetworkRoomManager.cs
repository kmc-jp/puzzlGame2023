using Mirror;
using UnityEngine;
using NetworkDiscoveryExt;
using RoomScene.StartButton;

namespace NetworkRoomManagerExt {

    [DisallowMultipleComponent]
    public class MatchNetworkRoomManager : NetworkRoomManager {
        public override void OnRoomClientConnect() {
            base.OnRoomClientConnect();

            MatchNetworkDiscovery.Singleton.StopDiscovery();
        }

        public override void OnRoomClientDisconnect() {
            base.OnRoomClientDisconnect();

            MatchNetworkDiscovery.Singleton.StartDiscovery();
        }

        public override void OnRoomStartServer() {
            base.OnRoomStartServer();

            MatchNetworkDiscovery.Singleton.AdvertiseServer();
        }

        public override void OnRoomServerPlayersReady() {
            // NOTE:
            // NetworkRoomManager's OnRoomServerPlayersReady immediately calls ServerChangeScene.
            // Since the game wants to make the transition to the gameplay scene as soon as one of the players triggers the start of the game,
            // the corresponding method of the base class is not called.

            // base.OnRoomServerPlayersReady();

            var buttonOwner = StartButtonDisplay.Singleton.connectionToClient;
            StartButtonDisplay.Singleton.TargetShow(buttonOwner);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();

            var buttonOwner = StartButtonDisplay.Singleton.connectionToClient;
            StartButtonDisplay.Singleton.TargetHide(buttonOwner);
        }
    }
}