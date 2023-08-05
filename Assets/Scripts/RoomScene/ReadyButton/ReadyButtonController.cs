using Mirror;
using UnityEngine;
using K.NetworkRoomManagerExt;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReadyButtonDisplay))]
    public class ReadyButtonController : MonoBehaviour {
        public static ReadyButtonController Singleton { get; private set; }

        [Client]
        public void CallCmdChangeReadyState(bool readyState) {
            var roomPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayer>();
            roomPlayer.CmdChangeReadyState(readyState);
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("ReadyButtonDisplay is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}