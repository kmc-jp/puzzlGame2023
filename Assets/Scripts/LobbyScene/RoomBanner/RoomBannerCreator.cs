using System;
using UnityEngine;
using NetworkDiscoveryExt;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    public class RoomBannerCreator : MonoBehaviour {
        [SerializeField] private GameObject _bannerPrefab;

        public void ServerFoundCallback(MatchServerResponse info) {
            var clientAddress = info.EndPoint.Address.ToString();
            var serverUri = info.uri;

            Create(clientAddress, serverUri);
        }

        void Create(string clientAddress, Uri serverUri) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<RoomBannerProfile>();
            profile.address = clientAddress;
            profile.uri = serverUri;
        }
    }
}