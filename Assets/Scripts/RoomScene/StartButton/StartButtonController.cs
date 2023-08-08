using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity))]
    public class StartButtonController : NetworkBehaviour {
        [Command]
        public void CmdCallServerChangeScene() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.ServerChangeScene(manager.GameplayScene);
        }
    }
}