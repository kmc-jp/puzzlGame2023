using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace RoomScene.LeaveButton {

    [DisallowMultipleComponent]
    public class LeaveButtonController : MonoBehaviour {
        public void CallStopClient() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StopClient();
        }

        // NOTE:
        // Experimentally, CallStopHost is currently employed because the prototype is supposed to use LAN network hosted.
        // However, after dedicated server has been applied, clients will not need to stop the server in client context.
        public void CallStopHost() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.StopHost();
        }
    }
}