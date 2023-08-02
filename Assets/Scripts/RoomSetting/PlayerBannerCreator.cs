using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSetting {

    /// <summary>
    /// Controls banner's position and Ready display depending on whether players own it.
    /// </summary>
    public class PlayerBannerCreator : NetworkBehaviour {
        // This property is set by PlayerBannerGenerator.
        public string ClientAddress { get; set; }

        public override void OnStartClient() {
            base.OnStartClient();

            SetAddress();

            int playerIndex = GetPlayerIndex();
            ChangeDisplayPosition(playerIndex);
        }

        public override void OnStartLocalPlayer() {
            base.OnStartLocalPlayer();

            ShowReady();
        }

        void SetAddress() {
            GameObject address = transform.Find("Address").gameObject;
            Text addressText = address.GetComponent<Text>();
            addressText.text = ClientAddress;
        }

        int GetPlayerIndex() {
            GameObject roomPlayer = NetworkClient.localPlayer.gameObject;
            NetworkRoomPlayer roomPlayerNetworkRoomPlayer = roomPlayer.GetComponent<NetworkRoomPlayer>();
            
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            int playerIndex = networkRoomManager.roomSlots.IndexOf(roomPlayerNetworkRoomPlayer);
            
            return playerIndex;
        }

        // Player banners are potentially located automatically by such as VerticalLayoutGroup
        // so the task of banners' arrangement are devided so that it allows to easily change the implementation of arrangement in actual.
        void ChangeDisplayPosition(int playerIndex) {
            RectTransform rectTransform = transform as RectTransform;

            switch (playerIndex) {
                case 0:
                    // middle-left anchored
                    rectTransform.anchoredPosition = new Vector2(0, 0.5f);
                    rectTransform.localPosition = new Vector3(480, 0);
                    break;
                case 1:
                    // middle-right anchored
                    rectTransform.anchoredPosition = new Vector2(1, 0.5f);
                    rectTransform.localPosition = new Vector3(-480, 0);
                    break;
            }
        }

        // REVIEW:
        // I think that controller scripts have to have the responsibility of only INPUT.
        // Whether ready button is displayed or not does not depends on input so I wrote this method here
        // but in fact there is ReadyButtonController.HideButton, which is called when pushed.
        // Should I move ShowReady to ReadyButtonController or leave it here?
        void ShowReady() {
            GameObject ready = transform.Find("Ready").gameObject;
            ready.SetActive(true);
        }
    }
}