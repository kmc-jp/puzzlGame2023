using Mirror;
using NetworkRoomManagerExt;

namespace RoomScene.RoomPlayer {
 
    public class RoomPlayerHelper : NetworkRoomPlayer {
        [Command]
        void CmdCallServerChangeScene() {
            var manager = NetworkManager.singleton as MatchNetworkRoomManager;
            manager.ServerChangeScene(manager.GameplayScene);
        }

        public void CallServerChangeScene() {
            CmdCallServerChangeScene();
        }
    }
}