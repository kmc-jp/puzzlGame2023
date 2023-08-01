using Mirror;
using UnityEngine;

namespace RoomSelect {

    // This class is going to be used to collect room settings input via UI and to reflect them.
    public class RoomCreationHandler : MonoBehaviour {
#if UNITY_SERVER
        public void CallStartServer() {
            NetworkManager.singleton.StartServer();
        }
#else
        public void CallStartHost() {
            NetworkManager.singleton.StartHost();
        }
#endif
    }
}