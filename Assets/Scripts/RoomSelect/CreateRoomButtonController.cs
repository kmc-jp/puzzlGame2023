using Mirror;
using UnityEngine;

namespace RoomSelect {
    
    public class CreateRoomButtonController : MonoBehaviour {
#if !UNITY_SERVER
        public void CallStartHost() {
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            networkRoomManager.StartHost();
        }
#endif
    }
}