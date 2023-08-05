using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace RoomScene.PlayerBanner {

    // NOTE:
    // This class does not work for anything at the moment.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerBuilder), typeof(PlayerBannerController))]
    public class PlayerBannerBuilder : MonoBehaviour {
        private PlayerBannerProfile profile;

#if UNITY_EDITOR
        void OnValidate() {
            profile = GetComponent<PlayerBannerProfile>();
        }
#endif

        void Start() {
            // In Start, information related to networking (that is, constatnt information) is only assigned. 
            var address = transform.Find("Address").GetComponent<Text>();
            address.text = profile.Address;
        }
    }
}