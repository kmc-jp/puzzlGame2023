using Mirror;
using UnityEngine;
using K.NetworkRoomManagerExt;

namespace K.LobbyScene {

    [DisallowMultipleComponent]
    public class CreateRoomButtonController : MonoBehaviour {
        public static CreateRoomButtonController Singleton { get; private set; }

        public void CallStartHost() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StartHost();
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("CreateRoomButtonController is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}