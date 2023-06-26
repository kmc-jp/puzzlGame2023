using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

namespace RoomSelect {

    public class CustomNetworkDiscovery : NetworkDiscoveryBase<CustomServerRequest, CustomServerResponse> {

        protected override CustomServerResponse ProcessRequest(CustomServerRequest request, IPEndPoint endpoint) {
            // In this case we don't do anything with the request
            // but other discovery implementations might want to use the data
            // in there,  This way the client can ask for
            // specific game mode or something

            try {
                CustomNetworkRoomManager customNetworkRoomManager = GameObject.Find("Custom Network Room Manager").GetComponent<CustomNetworkRoomManager>();

                // this is an example reply message,  return your own
                // to include whatever is relevant for your game
                return new CustomServerResponse {
                    serverId = ServerId,
                    uri = transport.ServerUri(),
                    maxPlayerNum = customNetworkRoomManager.maxConnections,
                    currentPlayerNum = customNetworkRoomManager.roomSlots.Count
                };
            } catch (NotImplementedException) {
                Debug.LogError($"Transport {transport} does not support network discovery");
                throw;
            }
        }

        protected override CustomServerRequest GetRequest() => new CustomServerRequest();

        protected override void ProcessResponse(CustomServerResponse response, IPEndPoint endpoint) {
            // we received a message from the remote endpoint
            response.EndPoint = endpoint;

            // although we got a supposedly valid url, we may not be able to resolve
            // the provided host
            // However we know the real ip address of the server because we just
            // received a packet from it,  so use that as host.
            UriBuilder realUri = new UriBuilder(response.uri) {
                Host = response.EndPoint.Address.ToString()
            };
            response.uri = realUri.Uri;

            OnServerFound.Invoke(response);
        }
    }
}