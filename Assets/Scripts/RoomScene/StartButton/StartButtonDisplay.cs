using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(StartButtonController))]
    public class StartButtonDisplay : NetworkBehaviour {
        // [TargetRpc]
        public void TargetShow(NetworkConnectionToClient roomOwner) {
            var button = gameObject.GetComponent<Button>();
            button.interactable = true;
        }

        // [TargetRpc]
        public void TargetHide(NetworkConnectionToClient roomOwner) {
            var button = gameObject.GetComponent<Button>();
            button.interactable = false;
        }
    }
}