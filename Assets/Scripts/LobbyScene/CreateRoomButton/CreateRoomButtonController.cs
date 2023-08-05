using Mirror;
using UnityEngine;
using K.NetworkRoomManagerExt;

namespace K.LobbyScene {

    [DisallowMultipleComponent]
    public class CreateRoomButtonController : MonoBehaviour {
        public void CallStartHost() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StartHost();
        }
    }
}