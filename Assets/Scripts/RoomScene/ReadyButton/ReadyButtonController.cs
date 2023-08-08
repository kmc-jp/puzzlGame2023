using Mirror;
using UnityEngine;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    public class ReadyButtonController : MonoBehaviour {
        public void CallCmdChangeReadyState(bool readyState) {
            var roomPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayer>();
            roomPlayer.CmdChangeReadyState(readyState);
        }
    }
}