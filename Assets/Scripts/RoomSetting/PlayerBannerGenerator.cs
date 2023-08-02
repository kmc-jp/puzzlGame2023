using UnityEngine;

namespace RoomSetting {

    // This class has only the responsibility of instantiation of player banners and does not have of build a banner.
    // Building banners is executed in PlayerBannerCreator because a banner should be dealt with in different processes
    // so PlayerBannerCreator has to inherit from NetworkBehaviour.
    public class PlayerBannerGenerator : MonoBehaviour {
        public static PlayerBannerGenerator Singleton { get; private set; }

        [SerializeField] private GameObject _playerBannerPrefab;

        public GameObject Generate(string clientAddress) {
            GameObject playerBanner = Instantiate(_playerBannerPrefab);
            
            // There is no way to get the client address easily in PlayerBannerCreator, hence passing the address via property.
            // However, building banners should be performed completely in PlayerBannerCreator and so this class does not care about how to build them.
            PlayerBannerCreator playerBannerPlayerBannerCreator = playerBanner.GetComponent<PlayerBannerCreator>();
            playerBannerPlayerBannerCreator.ClientAddress = clientAddress;

            return playerBanner;
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Destroy(gameObject);
            }
        }
    }
}