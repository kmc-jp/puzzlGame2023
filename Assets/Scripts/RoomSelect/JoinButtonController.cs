using Mirror;
using Mirror.Discovery;
using System;
using UnityEngine;

namespace RoomSelect {

    public class JoinButtonController : MonoBehaviour {

        /// <summary>
        /// RoomFinderによって割り当てられる
        /// </summary>
        public RoomFinder RoomFinder { get; set; }
        /// <summary>
        /// RoomFinderによって割り当てられる
        /// これ使わない
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// RoomFinderが消えるみたいなんでこっち使う
        /// </summary>
        public string networkAddress { get; set; }

        public void Join() {
            //RoomFinder.ConnectToServerAsClient(ServerId);
            // ネットワークアドレスはマネージャーから変更しないといけないらしいのでチート
            NetworkRoomManager.singleton.StartClient();
        }

    }

}