using Mirror.Discovery;

namespace RoomSelect {

    // NOTE:
    // NetworkDiscoveryXXXExt ideally should not inherit from NetworkDiscovery but NetworkDiscoveryBase
    // because NetworkDiscovery already has a custom request/response and processes.
    // However, required information of the server is only the server address at the moment
    // and the charge of this class is just calling NetworkDiscovery.StartDiscovery so it inherits from NetworkDiscovery.
    public class NetworkDiscoveryRoomSelectExt : NetworkDiscovery {
        public override void Start() {
            base.Start();

#if !UNITY_SERVER
            // NetworkDiscovery does not have the faculty to start discovery automatically.
            StartDiscovery();
#endif
        }
    }
}