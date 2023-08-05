using UnityEngine;
using NetworkDiscoveryExt;
using LobbyScene.RoomBanner;

namespace LobbyScene.RoomBannerList {

    [DisallowMultipleComponent]
    public class RoomBannerListUpdate : MonoBehaviour {
        public static RoomBannerListUpdate Singleton { get; private set; }

        public void ServerFoundCallback(MatchServerResponse info) {
            var banner = RoomBannerCreator.Singleton.Create();

            banner.transform.SetParent(transform);
        }

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("RoomBannerListUpdate is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }
        }
    }
}