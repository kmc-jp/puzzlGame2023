using Mirror;
using UnityEngine;
using NetworkRoomManagerExt;

namespace RoomScene.LeaveButton {

    [DisallowMultipleComponent]
    public class LeaveButtonController : MonoBehaviour {
        public static LeaveButtonController Singleton { get; private set; }

        [Client]
        public void CallStopHostOrClient() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            
            if (NetworkClient.activeHost) {
                manager.StopHost();
            } else {
                manager.StartClient();
            }
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("LeaveButtonController is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}