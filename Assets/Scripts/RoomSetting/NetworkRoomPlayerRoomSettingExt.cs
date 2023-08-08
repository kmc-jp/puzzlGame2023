using Mirror;

namespace RoomSetting {

    public class NetworkRoomPlayerRoomSettingExt : NetworkRoomPlayer {
        // This property is set by ReadyButtonDisplay.
        public ReadyButtonDisplay LocalReadyButtonDisplay { get; set; }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState) {
            base.ReadyStateChanged(oldReadyState, newReadyState);
            
            // the first time when called NetworkRoomPlayer.CmdChangeReadyState(true)
            if (!oldReadyState && newReadyState) {
                LocalReadyButtonDisplay.RpcHide();
            }
        }
    }
}