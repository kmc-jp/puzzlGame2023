using Mirror;
using UnityEngine;

namespace K.NetworkDiscoveryExt {

    // NOTE:
    // This class should be the only one that can access to MatchNetworkDiscovery in case in the future
    // and so there are some simple wrappers which just calls methods of MatchNetworkDiscovery.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MatchNetworkDiscovery))]
    public class MatchNetworkDiscoveryHandler : MonoBehaviour {
        public static MatchNetworkDiscoveryHandler Singleton { get; private set; }

#if UNITY_EDITOR
        private MatchNetworkDiscovery _discovery;
#endif

        // NOTE:
        // In lobby, Server attribute has no effect because every device is stand-alone, not a server or a client.
        // AdvertiseServer called in client context causes to call Shutdown, which leads to stop broadcasting.
        [Server]
        public void CallAdvertiseServer() {
            _discovery.AdvertiseServer();
        }

        // NOTE:
        // In lobby, Client attribute has no effect because every device is stand-alone, not a server or a client.
        // StartDiscovery called in server context causes to call Shutdown, which leads to stop listening to clients.
        [Client]
        void CallStartDiscovery() {
            _discovery.StartDiscovery();
        }

#if UNITY_EDITOR
        void OnValidate() {
            _discovery = GetComponent<MatchNetworkDiscovery>();
        }
#endif

        void Start() {
            if (Singleton == null) {
                Singleton = this;
            } else {
                Debug.LogWarning("MatchNetworkDiscovery is a singleton. This component is removed since there are multiple components in the scene.");
                Destroy(this);
            }

            // NOTE:
            // NetworkDiscovery will only work if the GameObject which it is attached to is COMPLETELY IDENTICAL both for the server and clients
            // (more precisely, a GameObject cloned by ParrelSync). Therefore, NetworkDiscovery cannot placed on a different GameObject in different scenes.
            DontDestroyOnLoad(gameObject);

            CallStartDiscovery();
        }
    }
}