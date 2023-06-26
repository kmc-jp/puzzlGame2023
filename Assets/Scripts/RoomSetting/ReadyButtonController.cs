using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class ReadyButtonController : MonoBehaviour {
        public NetworkRoomPlayer LocalPlayer { get; set; }

        public void SendReadyStateToServer() {
            LocalPlayer.CmdChangeReadyState(true);
        }
    }
}