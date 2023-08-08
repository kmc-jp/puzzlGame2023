using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.ReadyButton {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ReadyButtonController))]
    public class ReadyButtonDisplay : MonoBehaviour {
        public void Hide() {
            var button = gameObject.GetComponent<Button>();
            button.interactable = false;
        }
    }
}