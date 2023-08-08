using Mirror;
using UnityEngine;
using NetworkDiscoveryExt;
using RoomScene.StartButton;

namespace NetworkRoomManagerExt {

    [DisallowMultipleComponent]
    public class MatchNetworkRoomManager : NetworkRoomManager {
        private MatchNetworkDiscovery _discovery;

        public override void Start() {
            _discovery = GameObject.Find("MatchNetworkDiscovery").GetComponent<MatchNetworkDiscovery>();
        }

        public override void OnRoomClientConnect() {
            base.OnRoomClientConnect();

            _discovery.StopDiscovery();
        }

        public override void OnRoomClientDisconnect() {
            base.OnRoomClientDisconnect();

            _discovery.StartDiscovery();
        }

        public override void OnRoomStartHost() {
            base.OnRoomStartHost();

            _discovery.AdvertiseServer();
        }

        public override void OnRoomServerPlayersReady() {
            // NOTE:
            // NetworkRoomManager's OnRoomServerPlayersReady immediately calls ServerChangeScene.
            // Since the game wants to make the transition to the gameplay scene as soon as one of the players triggers the start of the game,
            // the corresponding method of the base class is not called.

            // base.OnRoomServerPlayersReady();

            var startButton = GameObject.Find("StartButton");
            var display = startButton.GetComponent<StartButtonDisplay>();
            var buttonOwner = display.connectionToClient;
            display.TargetShow(buttonOwner);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();

            var startButton = GameObject.Find("StartButton");
            var display = startButton.GetComponent<StartButtonDisplay>();
            var buttonOwner = display.connectionToClient;
            display.TargetHide(buttonOwner);
        }
    }
}