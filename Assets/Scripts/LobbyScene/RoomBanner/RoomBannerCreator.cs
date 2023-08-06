using NetworkDiscoveryExt;
using UnityEngine;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    public class RoomBannerCreator : MonoBehaviour {
        public static RoomBannerCreator Singleton { get; private set; }

        [SerializeField] private GameObject _bannerPrefab;

        // FIXME:
        // RoomBannerCreator is a logic so should not depend on UI design but this class does, for being made as first aid.
        // I was too tired, please help me.
        public void A(MatchServerResponse info) {
            var banner = Create();

            var profile = banner.GetComponent<RoomBannerProfile>();
            profile.Address = info.EndPoint.Address.ToString();
            profile.ServerUri = info.uri;

            banner.transform.SetParent(GameObject.Find("BannersParent").transform);
        }

        public GameObject Create() {
            GameObject banner = Instantiate(_bannerPrefab);
            return banner;
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("RoomBannerCreator is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }

            if (_bannerPrefab == null) {
                Debug.LogError("_bannerPrefab is not assigned.");
            }
        }
    }
}