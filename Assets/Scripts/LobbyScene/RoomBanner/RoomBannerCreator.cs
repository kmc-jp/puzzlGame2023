using System;
using UnityEngine;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    public class RoomBannerCreator : MonoBehaviour {
        public static RoomBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        public GameObject Create(string clientAddress, Uri serverUri) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<RoomBannerProfile>();
            profile.address = clientAddress;
            profile.uri = serverUri;

            return banner;
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("RoomBannerCreator is a singleton. This component is removed since there are multiple components in the scene.");
                DestroyImmediate(this);
            }

            if (_bannerPrefab == null) {
                Debug.LogError("_bannerPrefab is not assigned.");
            }
        }
    }
}