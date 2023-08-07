using System;
using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace LobbyScene.CreateRoomButton {

    [DisallowMultipleComponent]
    public class CreateRoomButtonController : MonoBehaviour {
        public static CreateRoomButtonController Singleton { get; private set; }

        // NOTE:
        // Experimentally, CallStopHost is currently employed because the prototype is supposed to use LAN network hosted.
        // However, starting with dedicated server requires has to request the server to start and clients should connect to it as client-only.
        // Therefore, this method is going to be deprecated.
        [Obsolete]
        public void CallStartHost() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StartHost();
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning(
                    "CreateRoomButtonController is a singleton." +
                    "This component is removed since there are multiple CreateRoomButtonController components in Scenes."
                );
                Destroy(this);
            }
        }
#endif
    }
}