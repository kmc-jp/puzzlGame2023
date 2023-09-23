using Mirror;
using UnityEngine;
using RoomScene.RoomPlayer;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity))]
    public class StartButtonController : NetworkBehaviour {
        public void CallServerChangeScene() {
            var helper = NetworkClient.localPlayer.GetComponent<RoomPlayerHelper>();
            helper.CallServerChangeScene();
        }
    }
}