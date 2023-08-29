using System.Net;
using System;
using Mirror.Discovery;
using UnityEngine;

public class BattleNetworkDiscovery : NetworkDiscoveryBase<BattleServerRequest, BattleServerResponse> {
    protected override BattleServerResponse ProcessRequest(BattleServerRequest request, IPEndPoint endpoint) {
        try {
            return new BattleServerResponse {
                serverId = ServerId,
                uri = transport.ServerUri()
            };
        } catch (NotImplementedException) {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }
    
    protected override BattleServerRequest GetRequest() => new BattleServerRequest();

    protected override void ProcessResponse(BattleServerResponse response, IPEndPoint endpoint) {
        response.EndPoint = endpoint;

        UriBuilder realUri = new UriBuilder(response.uri) {
            Host = response.EndPoint.Address.ToString()
        };
        response.uri = realUri.Uri;

        OnServerFound.Invoke(response);
    }
}