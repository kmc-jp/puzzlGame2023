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
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning(
                    "StartButtonController is a singleton." +
                    "This component is removed since there are multiple StartButtonController components in Scenes."
                );
                DestroyImmediate(this);
            }
        }
#endif

        [Command]
        public void CmdCallServerChangeScene() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.ServerChangeScene(manager.GameplayScene);
        }
    }
}