using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerProfile))]
    public class PlayerBannerBuilder : MonoBehaviour {
        private PlayerBannerProfile _profile;

        void Start() {
            _profile = GetComponent<PlayerBannerProfile>();

            var address = transform.Find("Address").GetComponent<Text>();
            address.text = _profile.address;

            // TODO:
            // Placement of banners.
        }
    }
}