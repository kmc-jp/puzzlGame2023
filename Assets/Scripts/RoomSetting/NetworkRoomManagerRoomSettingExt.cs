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
        }
    }
}