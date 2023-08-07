using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReadyButtonController))]
    public class ReadyButtonDisplay : MonoBehaviour {
        public static ReadyButtonDisplay Singleton { get; private set; }

        public void Hide() {
            var button = gameObject.GetComponent<Button>();
            button.interactable = false;
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning(
                    "ReadyButtonDisplay is a singleton." +
                    "This component is removed since there are multiple ReadyButtonDisplay components in Scenes."
                );
                Destroy(this);
            }
        }
#endif
    }
}