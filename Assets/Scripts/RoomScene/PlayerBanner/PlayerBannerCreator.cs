using UnityEngine;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    public class PlayerBannerCreator : MonoBehaviour {
        [SerializeField] private GameObject _bannerPrefab;

        public GameObject Create(string clientAddress) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<PlayerBannerProfile>();
            profile.address = clientAddress;

            return banner;
        }
    }
}