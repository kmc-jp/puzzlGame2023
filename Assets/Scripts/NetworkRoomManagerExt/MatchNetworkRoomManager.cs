using Mirror;
using UnityEngine;

namespace K.NetworkRoomManagerExt {

    /// <summary>
    /// Manages network processing related to lobby, room, and gameplay.
    /// </summary>
    // REVIEW:
    // The network managing GameObject which this component attaches to supposes to manage the network over three scenes, 
    // lobby, room, and gameplay, but it is not the only case necessary for network management. Therefore, I named this class MatchNetworkRoomManager 
    // with a focus on the usage of network management, but it does not seem to be the best best choice. Is there any better naming?
    [DisallowMultipleComponent]
    public class MatchNetworkRoomManager : NetworkRoomManager {

    }
}