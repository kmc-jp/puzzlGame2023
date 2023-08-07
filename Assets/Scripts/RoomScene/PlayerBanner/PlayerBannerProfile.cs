using Mirror;
using UnityEngine;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity))]
    public class PlayerBannerProfile : NetworkBehaviour {
        private bool _was_address_assigned;

        [SyncVar(hook = nameof(DisallowAddressReassignment))]
        public string address;

        private void DisallowAddressReassignment(string oldValue, string newValue) {
            if (_was_address_assigned) {
                Debug.LogError("address disallows reassignment. It is supposed to be assigned once in MatchNetworkManager.");
            } else {
                _was_address_assigned = true;
                address = newValue;
            }
        }
    }
}