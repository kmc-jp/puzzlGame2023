using UnityEngine;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    public class PlayerBannerCreator : MonoBehaviour {
        public static PlayerBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        public GameObject Create() {
            GameObject banner = Instantiate(_bannerPrefab);
            return banner;
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("PlayerBannerCreator is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }

            if (_bannerPrefab == null) {
                Debug.LogError("_bannerPrefab is not assigned.");
            }
        }
    }
}