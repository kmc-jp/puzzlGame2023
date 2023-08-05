using Mirror;
using UnityEngine;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(ReadyButtonController))]
    public class ReadyButtonDisplay : MonoBehaviour {
        public static ReadyButtonDisplay Singleton { get; private set; }

        public void Hide() {
            gameObject.SetActive(false);
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