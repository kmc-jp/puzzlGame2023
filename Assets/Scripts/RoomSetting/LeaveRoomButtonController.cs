using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class LeaveRoomButtonController : MonoBehaviour {
        public void DisconnectServerAndBackToRoomSelectScene() {
            NetworkManager.singleton.StopServer();
            NetworkManager.singleton.StopClient();
        }
    }
}