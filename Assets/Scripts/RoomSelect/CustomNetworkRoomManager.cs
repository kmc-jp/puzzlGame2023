using Mirror;
using RoomSetting;
using UnityEngine;

namespace RoomSelect {
    
    public class CustomNetworkRoomManager : NetworkRoomManager {
        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn) {
            GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject);
            if (roomSlots.Count == 0) {
                LeaderAuthGranter initialLeader = roomPlayer.GetComponent<LeaderAuthGranter>();
                initialLeader.IsLeader = true;
            };
            return roomPlayer;
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) {
            base.OnRoomServerDisconnect(conn);
            
            foreach (NetworkRoomPlayer remainPlayers in roomSlots) {
                LeaderAuthGranter granter = remainPlayers.GetComponent<LeaderAuthGranter>();
                if (granter.IsLeader) {
                    return;
                }
            }

            if (roomSlots.Count > 0) {
                LeaderAuthGranter newLeader = roomSlots[0].GetComponent<LeaderAuthGranter>();
                newLeader.IsLeader = true;
            }
        }

        public override void OnRoomClientEnter() {
            base.OnRoomClientEnter();
            
            GameObject readyButton = GameObject.Find("ReadyButton");
            ReadyButtonController controller = readyButton.GetComponent<ReadyButtonController>();
            
            foreach (NetworkRoomPlayer player in roomSlots) {
                if (player.isLocalPlayer) {
                    controller.LocalPlayer = player;
                }
            }
        }

        public override void OnRoomServerPlayersReady() {
            GameObject startGameButton = GameObject.Find("StartGameButton");
            StartGameButtonController controller = startGameButton.GetComponent<StartGameButtonController>();
            controller.EnablePushStartButtonIfLeader(true);
        }

        public override void OnRoomServerPlayersNotReady() {
            base.OnRoomServerPlayersNotReady();
            GameObject startGameButton = GameObject.Find("StartGameButton");
            if (startGameButton == null) {
                return;
            }
            StartGameButtonController controller = startGameButton.GetComponent<StartGameButtonController>();
            controller.EnablePushStartButtonIfLeader(false);
        }
    }
}