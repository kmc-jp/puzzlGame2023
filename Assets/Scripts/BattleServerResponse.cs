using System;
using System.Net;
using Mirror;

public struct BattleServerResponse : NetworkMessage {
    public IPEndPoint EndPoint { get; set; }

    public Uri uri;

    public long serverId;
}