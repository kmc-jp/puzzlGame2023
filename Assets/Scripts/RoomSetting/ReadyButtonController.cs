using System;
using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class ReadyButtonController : MonoBehaviour {
        // NOTE:
        // This field is referred by CustomRoomNetworkManager, which will be unnecessary in the future.
        // However, because CustomRoomNetworkManager is in the charge of #27 (Here is #32), deletion of it and LocalPlayer are pending.
        [Obsolete]
        public NetworkRoomPlayer LocalPlayer;

        // This wrapper ensures that CallCmdChangeReadyState is called once per connection. 
        public void SendReadyToServer() {
            CallCmdChangeReadyState(true);
            HideButton();
        }

        void CallCmdChangeReadyState(bool readyState) {
            GameObject roomPlayer = NetworkClient.localPlayer.gameObject;
            NetworkRoomPlayer roomPlayerNetworkRoomPlayer = roomPlayer.GetComponent<NetworkRoomPlayer>();
            roomPlayerNetworkRoomPlayer.CmdChangeReadyState(readyState);
        }

        void HideButton() {
            gameObject.SetActive(false);
        }
    }
}