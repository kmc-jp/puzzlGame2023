using UnityEngine;
using UnityEngine.UI;

namespace K.LobbyScene.RoomBanner {

    // NOTE:
    // This class does not work for anything at the moment.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RoomBannerProfile))]
    public class RoomBannerBuilder : MonoBehaviour {
        private RoomBannerProfile profile;

#if UNITY_EDITOR
        void OnValidate() {
            profile = GetComponent<RoomBannerProfile>();
        }
#endif

        void Start() {
            // In Start, information related to networking (that is, constatnt information) is only assigned. 
            var address = transform.Find("Address").GetComponent<Text>();
            address.text = profile.Address;
        }
    }
}