using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class LeaderAuthGranter : NetworkBehaviour {
        [SyncVar]
        public bool IsLeader;
    }
}