using Mirror;
using UnityEngine;

namespace RoomScene.PlayerBanner {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkIdentity), typeof(PlayerBannerProfile), typeof(PlayerBannerBuilder))]
    public class PlayerBannerController : NetworkBehaviour {

    }
}