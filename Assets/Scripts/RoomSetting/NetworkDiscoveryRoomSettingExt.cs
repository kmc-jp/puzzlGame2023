using Mirror;
using Mirror.Discovery;

namespace RoomSetting {

    // NOTE:
    // NetworkDiscoveryXXXExt ideally does not inherit from NetworkDiscovery but NetworkDiscoveryBase
    // because NetworkDiscovery already has a custom request/response and processes.
    // However, required information of the server is only the server address
    // and the charge of this class is just calling NetworkDiscovery.AdvertiseServer so it inherits from NetworkDiscovery.
    public class NetworkDiscoveryRoomSettingExt : NetworkDiscovery {
        public override void Start() {
            base.Start();

            if (NetworkServer.active) {
                AdvertiseServer();
            }
        }
    }
}