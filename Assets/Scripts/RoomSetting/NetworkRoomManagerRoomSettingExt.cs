using Mirror;
using UnityEngine;

namespace RoomSetting {
 
    public class NetworkRoomManagerRoomSettingExt : NetworkRoomManager {
        public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
            base.OnRoomServerConnect(conn);

            GameObject playerBanner = PlayerBannerGenerator.Singleton.Generate(conn.address);
            
            // The second parameter of NetworkServer.Spawn is to signify the owner of spawned networked objects.
            // conn is the connection to the client got connected to the server.
            NetworkServer.Spawn(playerBanner, conn);

            // NOTE:
            // ReadyButtonDisplay.RpcSetInteractableIfHost is of course RPC.
            // Thought I don't know whether RPCs must be called after NetworkServer.Spawn has completed,
            // I wrote these processes after the spawn just in case.
            GameObject ready = playerBanner.transform.Find("Ready").gameObject;
            ReadyButtonDisplay readyReadyButtonDisplay = ready.GetComponent<ReadyButtonDisplay>();
            readyReadyButtonDisplay.RpcSetInteractableIfHost(true);
        }

        // OnRoomServerPlayersReady is called when players which are ready reach the minimum number to play.
        public override void OnRoomServerPlayersReady() {
            // The default implementation of OnRoomServerPlayerReady is call immediately NetworkRoomManager.ServerChangeScene,
            // which also triggers all clients to change scene.
            // base.OnRoomServerPlayersReady should be ignored because it is required to be called when the start button is pushed.
            
            // base.OnRoomServerPlayersReady();

            StartButtonDisplay.Singleton.RpcChangeInteractableIfHost(true);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();

            StartButtonDisplay.Singleton.RpcChangeInteractableIfHost(false);
        }
    }
}