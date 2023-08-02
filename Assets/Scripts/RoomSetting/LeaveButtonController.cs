using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class LeaveButtonController : MonoBehaviour {
#if !UNITY_SERVER
        public void CallStopClientIfClientOnly() {
            if (NetworkClient.active && !NetworkServer.activeHost) {
                NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
                networkRoomManager.StopClient();
            }
        }

        public void CallStopHostIfHost() {
            if (NetworkClient.activeHost) {
                NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
                networkRoomManager.StopHost();
            }
        }
#endif
    }
}