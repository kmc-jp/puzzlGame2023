using Mirror;
using UnityEngine;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(StartButtonController))]
    public class StartButtonDisplay : NetworkBehaviour {
        public static StartButtonDisplay Singleton { get; private set; }
        
        [TargetRpc]
        public void TargetShow(NetworkConnectionToClient roomOwner) {
            gameObject.SetActive(true);
        }

        [TargetRpc]
        public void TargetHide(NetworkConnectionToClient roomOwner) {
            gameObject?.SetActive(false);
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("StartButtonDisplay is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}