using System;
using Mirror;
using UnityEngine;

namespace RoomSelect {

    public class JoinButtonController : MonoBehaviour {
#if !UNITY_SERVER
        public void CallStartClient(Uri uriOfDedicatedServerOrHost) {
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            networkRoomManager.StartClient(uriOfDedicatedServerOrHost);
        }
#endif
    }
}