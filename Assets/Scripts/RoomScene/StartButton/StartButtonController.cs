using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(StartButtonDisplay))]
    public class StartButtonController : NetworkBehaviour {
        public static StartButtonController Singleton { get; private set; }

        [Command]
        public void CmdCallServerChangeScene() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.ServerChangeScene(manager.GameplayScene);
        } 

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("StartButtonController is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}