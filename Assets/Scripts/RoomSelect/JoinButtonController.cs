using System;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

namespace RoomSelect {

    public class JoinButtonController : MonoBehaviour {
        private NetworkDiscovery _networkDiscovery;
        public Uri ServerUri;

        private void Start() {
            _networkDiscovery = GameObject.Find("NetworkManager").GetComponent<NetworkDiscovery>();
        }

        public void StopAvailableServerInquiring() {
            _networkDiscovery.StopDiscovery();
        }

        public void JoinServer() {
            NetworkManager.singleton.StartClient(ServerUri);
        }
    }
}