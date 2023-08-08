using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSetting {

    // NOTE:
    // XXXButtonDisplay has the responsibility of controlling to show, to hide, or to become interactable.
    // To visualize other clients' state, the control have to be synchronized by RPC.
    public class ReadyButtonDisplay : NetworkBehaviour {
        [ClientRpc]
        public void RpcSetInteractableIfHost(bool value) {
            if (NetworkServer.activeHost) {
                Button readyButton = GetComponent<Button>();
                readyButton.interactable = value;
            }
        }

        // This method is called in NetworkRoomPlayerRoomSettingExt.ReadyStateChanged on the SERVER (because this method has ClientRpc attribute).
        [ClientRpc]
        public void RpcHide() {
            gameObject.SetActive(false);
        }

        void Start() {
            if (authority) {
                GameObject roomPlayer = NetworkClient.localPlayer.gameObject;
                NetworkRoomPlayerRoomSettingExt roomPlayerNetworkRoomPlayerRoomSettingExt = roomPlayer.GetComponent<NetworkRoomPlayerRoomSettingExt>();
                roomPlayerNetworkRoomPlayerRoomSettingExt.LocalReadyButtonDisplay = this;
            }
        }
    }
}