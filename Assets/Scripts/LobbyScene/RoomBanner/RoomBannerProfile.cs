using System;
using Mirror;
using UnityEngine;

namespace LobbyScene.RoomBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity))]
    public class RoomBannerProfile : NetworkBehaviour {
        private bool _was_address_assigned;
        private bool _was_uri_assigned;

        [SyncVar(hook = nameof(DisallowAddressReassignment))]
        public string address;
        [SyncVar(hook = nameof(DisallowServerUriReassignment))]
        public Uri uri;

        public void DisallowAddressReassignment(string oldValue, string newValue) {
            if (_was_address_assigned) {
                Debug.Log("address disallows reassignment. It is supposed to be assigned once in RoomBannerCreator.");
            } else {
                _was_address_assigned = true;
                address = newValue;
            }
        }

        public void DisallowServerUriReassignment(Uri oldValue, Uri newValue) {
            if (_was_uri_assigned) {
                Debug.Log("uri disallows reassignment. It is supposed to be assigned once in RoomBannerCreator.");
            } else {
                _was_uri_assigned = true;
                uri = newValue;
            }
        }
    }
}