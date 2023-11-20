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

            // FIXME:
            // Since now that the LAN hosted networking are employed,
            // OnRoomClientConnect and OnRoomStartHost are both called when the host starts the server.
            // This potentially calls AdvertiseServer first, and after that calls StopDiscovery, but this prevents from normal working of AdvertiseServer.
            // This phenomenon probablly solved by setting ScriptOrder, but it is no more than the first aid. Is there any more good idea?

            // _discovery.StopDiscovery();
        }

        public override void OnRoomClientDisconnect() {
            base.OnRoomClientDisconnect();

            _discovery.StartDiscovery();
        }

        public override void OnRoomStartServer() {
            base.OnRoomStartServer();

            _discovery.AdvertiseServer();
        }

        public override void OnRoomServerPlayersReady() {
            // NOTE:
            // NetworkRoomManager's OnRoomServerPlayersReady immediately calls ServerChangeScene.
            // Since the game wants to make the transition to the gameplay scene as soon as one of the players triggers the start of the game,
            // the corresponding method of the base class is not called.

            // base.OnRoomServerPlayersReady();

            var startButton = GameObject.Find("StartButton");
            if(startButton == null)
            {
                return;
            }
            var display = startButton.GetComponent<StartButtonDisplay>();
            var buttonOwner = NetworkClient.connection as NetworkConnectionToClient;
            display.TargetShow(buttonOwner);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();

            var startButton = GameObject.Find("StartButton");
            if(startButton == null)
            {
                return;
            }
            var display = startButton.GetComponent<StartButtonDisplay>();
            var buttonOwner = NetworkClient.connection as NetworkConnectionToClient;
            display.TargetHide(buttonOwner);
        }
    }
}