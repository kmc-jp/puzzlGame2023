using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.StartButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(StartButtonController))]
    public class StartButtonDisplay : NetworkBehaviour {
        public static StartButtonDisplay Singleton { get; private set; }

#if UNITY_EDITOR
        protected override void OnValidate() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning(
                    "StartButtonDisplay is a singleton." +
                    "This component is removed since there are multiple StartButtonDisplay components in Scenes."
                );
                DestroyImmediate(this);
            }
        }
#endif
        
        [TargetRpc]
        public void TargetShow(NetworkConnectionToClient roomOwner) {
            var button = gameObject.GetComponent<Button>();
            button.interactable = true;
        }

        [TargetRpc]
        public void TargetHide(NetworkConnectionToClient roomOwner) {
            var button = gameObject.GetComponent<Button>();
            button.interactable = false;
        }
    }
}