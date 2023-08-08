using Mirror;
using UnityEngine.UI;

namespace RoomSetting {

    // NOTE:
    // XXXButtonDisplay has the responsibility of controlling to show, to hide, or to become interactable.
    // To visualize other clients' state, the control have to be synchronized by RPC.
    public class StartButtonDisplay : NetworkBehaviour {
        public static StartButtonDisplay Singleton { get; private set; }

        [ClientRpc]
        public void RpcChangeInteractableIfHost(bool value) {
            if (NetworkServer.activeHost) {
                Button startButton = GetComponent<Button>();
                startButton.interactable = value;
            }
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Destroy(gameObject);
            }
        }
    }
}