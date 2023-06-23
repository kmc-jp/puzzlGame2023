using System;
using System.Collections.Generic;
using System.Reflection;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace RoomSelect {

    public class RoomFinder : MonoBehaviour {

        [SerializeField] private GameObject _roomBannerPrefab;
        [SerializeField] private Transform _roomBanners;

        // ボタンからは引数がUriのメソッドを呼べないのでserverIdで紐づける
        private Dictionary<long, Uri> _serverIdUriDict = new();

        public void AddToRecruitingServer(ServerResponse info) {
            if (!_serverIdUriDict.ContainsKey(info.serverId)) {
                _serverIdUriDict.Add(info.serverId, info.uri);
            }
        }

        /// <summary>
        /// サーバの絶対URIを含むルームバナーを作る
        /// このメソッドはOnServerFoundイベントで呼ばれ、OnServerFoundで呼ばれるメソッドはサーバの情報を引数に持つ
        /// </summary>
        public void CreateNewRoomBanner(ServerResponse info) {
            GameObject roomBanner = Instantiate(_roomBannerPrefab);
            // スクロール可能なようにScrollRectのContentの最後尾に追加する
            roomBanner.transform.SetParent(_roomBanners);
            roomBanner.transform.SetAsLastSibling();
            // サーバURIを書き込む
            Transform serverUri = roomBanner.transform.Find("Server URI");
            Text serverUriText = serverUri.GetComponent<Text>();
            serverUriText.text = info.uri.AbsoluteUri;
            JoinButtonController joinButtonController = roomBanner.GetComponent<JoinButtonController>();
            //joinButtonController.ServerId = info.serverId;
            joinButtonController.networkAddress = info.uri.Host;
        }

        /// <summary>
        /// Uriを使ってサーバに接続する
        /// </summary>
        /// <param name="serverId"></param>
        public void ConnectToServerAsClient(long serverId) {
            NetworkRoomManager.singleton.StartClient(_serverIdUriDict[serverId]);
        }

        public void Test() {
            Debug.Log("Test");
        }
    }

}