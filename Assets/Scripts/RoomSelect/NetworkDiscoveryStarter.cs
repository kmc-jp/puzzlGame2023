#if !UNITY_SERVER
using Mirror.Discovery;
#endif
using UnityEngine;

namespace RoomSelect {

    public class NetworkDiscoveryStarter : MonoBehaviour {
#if !UNITY_SERVER
        void Start() {
            CallStartDiscovery();
        }

        // NetworkDiscovery has no faculty to start discovery automatically.
        void CallStartDiscovery() {
            NetworkDiscovery networkDiscovery = GetComponent<NetworkDiscovery>();
            networkDiscovery.StartDiscovery();
        }
#endif
    }
}