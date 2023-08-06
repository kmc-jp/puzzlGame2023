using System;
using UnityEngine;

namespace LobbyScene.RoomBanner {

    // Networking related information such as the server address is initialized in MatchNetworkRoomManager and should not be changed after this.
    // The public properties are tentatively determined by the RoomBannerCreator.
    // RoomBannerBuilder updates room banners' appearance with this changed information.
    // REVIEW:
    // Networking related information is ideally changed in MatchNetworkingRoomManager just once time, but RoomBannerProfile inherits from MonoBehaviour
    // and disallows to use a constructor for initialization. Therefore, networking related information is currently wrapped by properties
    // and asserts an error when setter is called over one time with a flag. Is there any better solution?
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RoomBannerBuilder))]
    public class RoomBannerProfile : MonoBehaviour {
        private bool _was_address_assigned;
        private string _address;

        private bool _was_server_uri_assigned;
        private Uri _server_uri;


        public string Address {
            get { return _address; }
            set {
                if (_was_address_assigned) {
                    Debug.LogError("Address disallows reassignment. Was its first time assignment done by MatchNetworkRoomManager?");
                } else {
                    _was_address_assigned = true;
                    _address = value;
                }
            }
        }

        public Uri ServerUri {
            get { return _server_uri; }
            set {
                if ( _was_server_uri_assigned) {
                    Debug.LogError("ServerUri disallows reassignment. Was its first time assignment done by MatchNetworkRoomManager?");
                } else {
                    _was_server_uri_assigned = true;
                    _server_uri = value;
                }
            }
        }
    }
}