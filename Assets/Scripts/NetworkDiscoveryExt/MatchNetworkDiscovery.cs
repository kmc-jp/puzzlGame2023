using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

namespace NetworkDiscoveryExt {

    // NOTE:
    // NetworkDiscoveryBase works correctly only when the components of its inherited classes are exactly the same across apps.
    // Specifically, the GameObject to which the component is attached must have the same GUID. Because of this restriction,
    // it is not possible to place components of different NetworkDiscoveryBase inherited classes in the lobby and room, so they are handled by a common class.
    [DisallowMultipleComponent]
    public class MatchNetworkDiscovery : NetworkDiscoveryBase<MatchServerRequest, MatchServerResponse> {
        public static MatchNetworkDiscovery Singleton { get; private set; }

#if UNITY_EDITOR
        public override void OnValidate() {
            if (Singleton != null) {
                Debug.LogWarning(
                    "MatchNetworkDiscovery is a singleton." +
                    "This component must be removed since there are multiple MatchNetworkDiscovery components in Scenes."
                );
            }
            
            Singleton = this;
        }
#endif

        public override void Start() {
            DontDestroyOnLoad(this);

            StartDiscovery();
        }

        // NOTE:
        // MatchNetworkDiscovery will also support server searches outside the LAN in the future,
        // in which case the distinction between inside and outside the LAN is meaningless to the caller,
        // and the base class StartDiscovery, which searches only inside the LAN, should not be invoked.
        public new void StartDiscovery() {
            base.StartDiscovery();
        }

        // NOTE:
        // MatchNetworkDiscovery will also support server searches outside the LAN in the future,
        // in which case the distinction between inside and outside the LAN is meaningless to the caller,
        // and the base class StopDiscovery, which searches only inside the LAN, should not be invoked.
        public new void StopDiscovery() {
            base.StopDiscovery();
        }

        // NOTE:
        // MatchNetworkDiscovery will also support server searches outside the LAN in the future,
        // in which case the distinction between inside and outside the LAN is meaningless to the caller,
        // and the base class AdvertiseServer, which searches only inside the LAN, should not be invoked.
        [Server]
        public new void AdvertiseServer() {
            base.AdvertiseServer();
        }
        
        // NOTE:
        // ProcessRequest is implemented to be completely identical to the corresponding method defined in NetworkDiscovery.
        // This implementation will change in the future when more requests are required to the server.
        protected override MatchServerResponse ProcessRequest(MatchServerRequest request, IPEndPoint endPoint) {
            try {
                return new MatchServerResponse {
                    serverId = ServerId,
                    uri = transport.ServerUri()
                };
            } catch (NotImplementedException) {
                Debug.LogError($"Transport {transport} does not support network discovery.");
                throw;
            }
        }

        // NOTE:
        // GetRequest is implemented to be completely identical to the corresponding method defined in NetworkDiscovery.
        // This implementation will change in the future when more requests are required to the server.
        protected override MatchServerRequest GetRequest() => new MatchServerRequest();

        // NOTE:
        // ProcessResponse is implemented to be completely identical to the corresponding method defined in NetworkDiscovery.
        // This implementation will change in the future when more requests are required to the server.
        protected override void ProcessResponse(MatchServerResponse response, IPEndPoint endPoint) {
            response.EndPoint = endPoint;

            UriBuilder readUri = new UriBuilder(response.uri) {
                Host = response.EndPoint.Address.ToString()
            };
            response.uri = readUri.Uri;

            OnServerFound.Invoke(response);
        }
    }
}