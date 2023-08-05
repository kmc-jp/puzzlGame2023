using System;
using System.Net;
using Mirror.Discovery;
using UnityEngine;

namespace NetworkDiscoveryExt {

    // NOTE:
    // MatchNetworkDiscovery, MatchServerRequest, and MatchServerResponse are complete clones of NetworkDiscovery, ServerRequest, ServerResponse at the moment
    // and they all do not have unique feature at all. This seems to be verbose but they are there for scalability in case more information is required
    // such as room name, number of participants, game mode, etc.
    [DisallowMultipleComponent]
    public class MatchNetworkDiscovery : NetworkDiscoveryBase<MatchServerRequest, MatchServerResponse> {
        protected override MatchServerResponse ProcessRequest(MatchServerRequest request, IPEndPoint endpoint) {

            try {
                return new MatchServerResponse {
                    serverId = ServerId,
                    uri = transport.ServerUri()
                };
            } catch (NotImplementedException) {
                Debug.LogError($"Transport {transport} does not support network discovery");
                throw;
            }
        }

        protected override MatchServerRequest GetRequest() => new MatchServerRequest();
        
        protected override void ProcessResponse(MatchServerResponse response, IPEndPoint endpoint) {
            response.EndPoint = endpoint;
            
            UriBuilder realUri = new UriBuilder(response.uri) {
                Host = response.EndPoint.Address.ToString()
            };
            response.uri = realUri.Uri;

            OnServerFound.Invoke(response);
        }

    }
}