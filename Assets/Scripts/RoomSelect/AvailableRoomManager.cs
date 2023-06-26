using System.Collections.Generic;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSelect {

    public class AvailableRoomManager : MonoBehaviour {
        [SerializeField] private NetworkDiscovery _networkDiscovery;

        [SerializeField] private RectTransform _roomBannerBoard;
        [SerializeField] private GameObject _roomBannerPrefab;

        private readonly Dictionary<long, CustomServerResponse> _discoveredServers = new Dictionary<long, CustomServerResponse>();

        private readonly List<GameObject> _availableRoomBanners = new List<GameObject>();

        private void InquireAvailableServerInLan() {
            // 現在表示されているバナーを全削除。
            foreach (var toBeDestroyed in _availableRoomBanners) {
                Destroy(toBeDestroyed);
            }
            _availableRoomBanners.Clear();

            // 有効なサーバの情報もリセット。
            _discoveredServers.Clear();
            // LAN内のサーバに新しく問い合わせ。
            _networkDiscovery.StartDiscovery();
        }

        private void UpdateAvailableServerTable(CustomServerResponse info) {
            _discoveredServers[info.serverId] = info;
        }

        private void ShowAvailableRoomBanners() {
            // サーバの情報をもとに入室可能な部屋のバナーを作る。
            foreach (var server in _discoveredServers) {
               
                GameObject newBanner = Instantiate(_roomBannerPrefab);
                // ScrollRectのContentを親に設定してオートレイアウトが効くようにする。
                newBanner.transform.SetParent(_roomBannerBoard);
                newBanner.transform.SetAsLastSibling();
                // サーバのアドレスを表示。
                Text serverUri = GameObject.Find("Server Adress").GetComponent<Text>();
                serverUri.text = server.Value.EndPoint.Address.ToString();
                
                // 部屋人数関連の処理を書く。
                // UIが作れてないので保留。
            }
        }
    }
}