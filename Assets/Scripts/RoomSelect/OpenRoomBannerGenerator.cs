using UnityEngine;
using UnityEngine.UI;

namespace RoomSelect {
    public class OpenRoomBannerGenerator : MonoBehaviour {
        [SerializeField] private GameObject _openRoomBannerPrefab;
        [SerializeField] private RectTransform _openRoomBannersParent;

        public void AddBanner(OpenRoomBannerInfo info) {
            GameObject newBanner = Instantiate(_openRoomBannerPrefab);
            // _openRoomBannersParent has a VerticalLayout component
            newBanner.transform.SetParent(_openRoomBannersParent.transform);

            // Set banner's info based on info
            Transform serverIp = newBanner.transform.Find("ServerIp");
            Text serverIpText = serverIp.GetComponent<Text>();
            serverIpText.text = info.ServerIp;
        }
        
        public void DeleteAllBanner() {
            // Delete banners if inherent data of server correspond
            foreach (Transform banner in _openRoomBannersParent) {
                Destroy(banner.gameObject);
            }
        }
    }
}