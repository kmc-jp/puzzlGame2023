using UnityEngine;
using UnityEngine.UI;
using Mirror.Discovery;

namespace RoomSelect {

    public class OpenRoomBannersManager : MonoBehaviour {
        [SerializeField] private GameObject _openRoomBannerPrefab;
        [SerializeField] private RectTransform _openRoomBannersParent;

        public void UpdateBanner(ServerResponse info) {
            string serverIp = info.EndPoint.Address.ToString();
            DeleteIfExist(serverIp);
            CreateNew(serverIp);
        }

        /// <summary>
        /// Creates a banner with serverIp and binds a connection to to the server to JoinButton
        /// </summary>
        /// <param name="serverIp">serverIp to be showed on the banner</param>
        void CreateNew(string serverIp) {
            GameObject openRoomBannerGameObject = Instantiate(_openRoomBannerPrefab);

            GameObject serverIpGameObject = openRoomBannerGameObject.transform.Find("ServerIp").gameObject;
            Text serverIpText = serverIpGameObject.GetComponent<Text>();
            serverIpText.text = serverIp;

            openRoomBannerGameObject.transform.SetParent(_openRoomBannersParent);
        }

        /// <summary>
        /// Searches _openRoomBannersParent and destroy a banner which has the same serverIp if any
        /// </summary>
        /// <param name="serverIp">serverIp of the banner to be deleted</param>
        void DeleteIfExist(string serverIp) {
            for (int i = 0; i < _openRoomBannersParent.childCount; i++) {
                GameObject openRoomBannerGameObject = _openRoomBannersParent.GetChild(i).gameObject;

                GameObject serverIpGameObject = openRoomBannerGameObject.transform.Find("ServerIp").gameObject;
                Text serverIpText = serverIpGameObject.GetComponent<Text>();
                
                if (serverIpText.text == serverIp) {
                    Destroy(openRoomBannerGameObject);
                    break;
                }
            }
        }
    }
}