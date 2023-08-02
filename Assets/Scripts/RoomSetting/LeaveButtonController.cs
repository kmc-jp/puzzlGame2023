using Mirror;
using UnityEngine;

namespace RoomSetting {

    // This class leads virtually to delete the player's banner and disconnect when a player leave a room
    public class LeaveButtonController : MonoBehaviour {
        /// <summary>
        /// Disconnect from the host (P2P player-hosted) via NetworkManager.StopClient
        /// This must be called on the last of series of leaving processes because NetworkManager.StopClient causes the transition to RoomSelect
        /// </summary>
        void DisconnectFromServer() {
            // StopClient fires NetworkRoomManager.OnRoomServerDisconnect, which asks the other clients to delete the banner of leaving player throught RPC
            NetworkManager.singleton.StopClient();
        }
    }
}