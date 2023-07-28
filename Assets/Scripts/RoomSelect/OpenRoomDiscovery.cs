using Mirror.Discovery;
using UnityEngine;

namespace RoomSelect {

    // NOTE:
    // This class simply works to call StartDiscovery() in NetworkDiscovery component because required information is only server address at that moment
    // When more information has been required such as the number of players, this class is going to be inherited from NetworkDiscoveryBase
    public class OpenRoomDiscovery : MonoBehaviour {
        [SerializeField] private NetworkDiscovery networkDiscovery;

        void Start() {
            networkDiscovery.StartDiscovery();
        }
    }
}