using System;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSelect {

    public class RoomBannerListManager : MonoBehaviour {
#if !UNITY_SERVER
        [SerializeField] private GameObject _roomBannerPrefab;
        private Transform _roomBannersParent;

        public void UpdateBanner(ServerResponse info) {
            string address = info.EndPoint.Address.ToString();
            Uri uri = info.uri;
            DeleteBannerifExist(address);
            CreateBanner(address, uri);
        }

        void Start() {
            _roomBannersParent = transform.Find("RoomBannersParent");
        }

        void CreateBanner(string dedicatedServerOrHostAddress, Uri uriOfDedicatedServerOrHost) {
            GameObject roomBanner = Instantiate(_roomBannerPrefab);
            roomBanner.transform.SetParent(_roomBannersParent);

            GameObject address = roomBanner.transform.Find("Address").gameObject;
            Text addressText = address.GetComponent<Text>();
            addressText.text = dedicatedServerOrHostAddress;

            GameObject join = roomBanner.transform.Find("Join").gameObject;
            Button joinButton = join.GetComponent<Button>();
            JoinButtonController joinJoinButtonController = joinButton.GetComponent<JoinButtonController>();
            joinButton.onClick.AddListener(() => joinJoinButtonController.CallStartClient(uriOfDedicatedServerOrHost));
        }

        void DeleteBannerifExist(string dedicatedServerOrHostAddress) {
            for (int i = 0; i <  _roomBannersParent.childCount; i++) {
                GameObject roomBanner = _roomBannersParent.GetChild(i).gameObject;

                GameObject address = roomBanner.transform.Find("Address").gameObject;
                Text addressText = address.GetComponent<Text>();

                if (addressText.text == dedicatedServerOrHostAddress) {
                    Destroy(roomBanner);
                    return;
                }
            }
        }
#endif
    }
}