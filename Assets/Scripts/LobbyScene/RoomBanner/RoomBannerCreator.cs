using System;
using UnityEngine;
using NetworkDiscoveryExt;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    public class RoomBannerCreator : MonoBehaviour {
        public static RoomBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        public void ServerFoundCallback(MatchServerResponse info) {
            var clientAddress = info.EndPoint.Address.ToString();
            var serverUri = info.uri;

            Create(clientAddress, serverUri);
        }

        void Start() {
            if (Singleton != null) {
                Debug.LogWarning(
                    "RoomBannerCreator is a singleton." +
                    "This component must be removed since there are multiple RoomBannerCreator components in Scenes."
                );
            }

            Singleton = this;

            if (_bannerPrefab == null) {
                Debug.LogError("_bannerPrefab is not assigned.");
            }
        }

        void Create(string clientAddress, Uri serverUri) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<RoomBannerProfile>();
            profile.address = clientAddress;
            profile.uri = serverUri;
        }
    }
}