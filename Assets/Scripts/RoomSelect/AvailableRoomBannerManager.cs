using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSelect {

    public class AvailableRoomBannerManager : MonoBehaviour {
        [SerializeField] private GameObject _roomBannerPrefab;
        [SerializeField] private RectTransform _roomBannerBoard;

        public void DeleteAllBanners() {
            foreach (Transform roomBanner in _roomBannerBoard) {
                Destroy(roomBanner.gameObject);
            }
        }

        public void CreateBanner(ServerResponse info) {
            GameObject roomBanner = Instantiate(_roomBannerPrefab);
            roomBanner.transform.SetParent(_roomBannerBoard);
            roomBanner.transform.SetAsLastSibling();
            Text serverAddress = roomBanner.transform.Find("ServerAddress").GetComponent<Text>();
            serverAddress.text = info.EndPoint.Address.ToString();
            JoinButtonController joinButton = roomBanner.transform.Find("JoinButton").GetComponent<JoinButtonController>();
            joinButton.ServerUri = info.uri;
        }
    }
}