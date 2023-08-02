using Mirror;

namespace RoomSetting {

    /// <summary>
    /// Controls StartButton at the bottom-right so that it is showed only on the host and becomes interactable when the opponent has been joined the room
    /// </summary>
    public class StartButtonController : NetworkBehaviour {
        // NOTE:
        // P2P player-hosted is supposed in ShowIfHost and HideIfHost
        // Note that it is required to tell a specific player to show StartButton by TargetRPC if we adopt dedicated server

        // This is called from NetworkRoomManager.OnServerPlayersReady
        // It is called from the server and executed on the client when the number of players who join the game has reached min number to start.
        // Prefix target shows that this method is executed only on a specific client by called on the server
        [TargetRpc]
        void TargetSetActiveIfHost(NetworkConnectionToClient playerHosted, bool showOrNot) {
            gameObject.SetActive(showOrNot);
        }

        void CallServerChangeScene() {
            // NetworkRoomManager.ServerChangeScene automatically synchronizes all clients to load a game scene.
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            networkRoomManager.ServerChangeScene(networkRoomManager.GameplayScene);
        }
    }
}