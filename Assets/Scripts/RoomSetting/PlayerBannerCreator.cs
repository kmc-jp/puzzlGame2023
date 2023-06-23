using Mirror;
using UnityEngine;

namespace RoomSetting {

    public class PlayerBannerCreator : NetworkRoomManager {

        public void CreateBanner() {
            NetworkServer.Spawn(NetworkManager.singleton.spawnPrefabs[0]);
        }

        public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
            base.OnRoomServerConnect(conn);
            CreateBanner();
        }
    }

}