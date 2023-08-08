using System;
using System.Collections.Generic;
using UnityEngine;
using NetworkDiscoveryExt;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    public class RoomBannerCreator : MonoBehaviour {
        [SerializeField] private GameObject _bannerPrefab;

        private Dictionary<long, MatchServerResponse> discoveredServers;

        public void ServerFoundCallback(MatchServerResponse info) {
            if (!discoveredServers.ContainsKey(info.serverId)) {
                discoveredServers[info.serverId] = info;
                
                var clientAddress = info.EndPoint.Address.ToString();
                var serverUri = info.uri;

                Create(clientAddress, serverUri);
            }
        }

        void Start() {
            discoveredServers = new Dictionary<long, MatchServerResponse>();
        }

        void Create(string clientAddress, Uri serverUri) {
            GameObject banner = Instantiate(_bannerPrefab);

            var profile = banner.GetComponent<RoomBannerProfile>();
            profile.address = clientAddress;
            profile.uri = serverUri;
        }
    }
}