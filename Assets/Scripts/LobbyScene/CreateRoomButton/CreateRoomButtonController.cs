using Mirror;
using UnityEngine;

namespace K.LobbyScene {

    [DisallowMultipleComponent]
    public class CreateRoomButtonController : MonoBehaviour {
        public void CallStartHost() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StartHost();
        }
    }
}