using System;
using System.Net;
using Mirror;

namespace K.NetworkDiscoveryExt {

    // NOTE:
    // NetworkDiscoveryExt, ServerRequestExt, and ServerResponseExt are complete clones of NetworkDiscovery, ServerRequest, ServerResponse at the moment
    // and they all do not have unique feature at all. This seems to be verbose but they are there for scalability in case more information is required
    // such as a room name, a number of participants, a room name or so on.
    public struct MatchServerResponse : NetworkMessage {
        public IPEndPoint EndPoint { get; set; }
        public Uri uri;
        public long serverId;
    }
}