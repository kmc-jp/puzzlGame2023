using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerProfile))]
    public class PlayerBannerBuilder : MonoBehaviour {
        private PlayerBannerProfile profile;

        void Start() {
            profile = GetComponent<PlayerBannerProfile>();

            var address = transform.Find("Address").GetComponent<Text>();
            address.text = profile.address;
        }
    }
}