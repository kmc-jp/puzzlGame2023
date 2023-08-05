using Mirror;
using UnityEditor.Build.Player;
using UnityEngine;

namespace K.LobbyScene.RoomBanner {
    
    // 
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RoomBannerBuilder))]
    public class RoomBannerProfile : MonoBehaviour {
        public string Address { get; }
    }
}

namespace K.RoomScene.PlayerBanner {

    // Get-only properties are information that is determined at the client connection stage and these properties are initialized in MatchNetworkRoomManager.
    // The public properties are tentatively determined by the ProfileBannerCreator (or inherited from the previous match)
    // and changed dynamically by the ProfileBannerController. ProfileBannerBuilder updates player banners with this changed information.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerBuilder), typeof(PlayerBannerController))]
    public class PlayerBannerProfile : MonoBehaviour {
        public string Address { get; }
    }
}

using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace K.RoomScene.PlayerBanner {

    // NOTE:
    // This class does not work for anything at the moment.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerProfile), typeof(PlayerBannerController))]
    public class PlayerBannerBuilder : MonoBehaviour {
        private PlayerBannerProfile profile;

#if UNITY_EDITOR
        void OnValidate() {
            profile = GetComponent<PlayerBannerProfile>();
        }
#endif

        void Start() {
            // In Start, information related to networking (that is, constatnt information) is only assigned. 
            var address = transform.Find("Address").GetComponent<Text>();
            address.text = profile.Address;
        }
    }
}

using Mirror;
using UnityEngine;

namespace K.RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    public class PlayerBannerCreator : MonoBehaviour {
        public static PlayerBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        [Server]
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