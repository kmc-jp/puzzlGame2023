using UnityEngine;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    public class PlayerBannerCreator : MonoBehaviour {
        public static PlayerBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        public GameObject Create(string clientAddress) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<PlayerBannerProfile>();
            profile.address = clientAddress;

            return banner;
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (Singleton != null) {
                Debug.LogWarning(
                    "PlayerBannerCreator is a singleton." +
                    "This component must be removed since there are multiple PlayerBannerCreator components in Scenes."
                );
            }

            Singleton = this;

            if (_bannerPrefab == null) {
                Debug.LogError("_bannerPrefab is not assigned.");
            }
        }
#endif
    }
}