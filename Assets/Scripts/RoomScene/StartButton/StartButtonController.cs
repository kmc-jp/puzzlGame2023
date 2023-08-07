using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity))]
    public class StartButtonController : NetworkBehaviour {
        public static StartButtonController Singleton { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate() {
            if (Singleton != null) {
                Debug.LogWarning(
                    "StartButtonController is a singleton." +
                    "This component must be removed since there are multiple StartButtonController components in Scenes."
                );
            }

            Singleton = this;
        }
#endif

        [Command]
        public void CmdCallServerChangeScene() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.ServerChangeScene(manager.GameplayScene);
        }
    }
}