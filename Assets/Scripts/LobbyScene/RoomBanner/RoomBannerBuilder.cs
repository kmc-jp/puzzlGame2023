using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RoomBannerProfile))]
    public class RoomBannerBuilder : MonoBehaviour {
        private RoomBannerProfile _profile;

        void Start() {
            _profile = GetComponent<RoomBannerProfile>();

            var address = transform.Find("Address").GetComponent<Text>();
            address.text = _profile.address;

            var joinButton = transform.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.AddListener(() => NetworkManager.singleton.StartClient(_profile.uri));

            var parent = GameObject.Find("BannersParent").transform;
            transform.SetParent(parent);
        }
    }
}