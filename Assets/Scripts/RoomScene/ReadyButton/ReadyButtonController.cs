using Mirror;
using UnityEngine;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    public class ReadyButtonController : MonoBehaviour {
        public static ReadyButtonController Singleton { get; private set; }

        public void CallCmdChangeReadyState(bool readyState) {
            var roomPlayer = NetworkClient.localPlayer.GetComponent<NetworkRoomPlayer>();
            roomPlayer.CmdChangeReadyState(readyState);
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning(
                    "ReadyButtonController is a singleton." +
                    "This component is removed since there are multiple ReadyButtonController components in Scenes."
                );
                Destroy(this);
            }
        }
#endif
    }
}