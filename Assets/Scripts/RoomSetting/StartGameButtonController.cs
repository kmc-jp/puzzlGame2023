using Mirror;
using UnityEngine.UI;

namespace RoomSetting {
 
    public class StartGameButtonController : NetworkBehaviour {

        [ClientRpc]
        public void EnablePushStartButtonIfLeader(bool canPush) {
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            foreach (NetworkRoomPlayer player in networkRoomManager.roomSlots) {
                LeaderAuthGranter granter = player.GetComponent<LeaderAuthGranter>();
                if (player.isLocalPlayer && granter.IsLeader) {
                    Button button = GetComponent<Button>();
                    button.interactable = canPush;
                }
            }
        }

        [Command(requiresAuthority = false)]
        public void StartGame() {
            NetworkRoomManager networkRoomManager = NetworkManager.singleton as NetworkRoomManager;
            networkRoomManager.ServerChangeScene(networkRoomManager.GameplayScene);
        }
    }
}