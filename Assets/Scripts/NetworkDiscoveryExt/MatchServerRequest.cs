using Mirror;

namespace NetworkDiscoveryExt {

    // NOTE:
    // MatchServerRequest and MatchServerResponse are currently both complete clones of ServerRequest and ServerResponse, respectively.
    // This may seem redundant but is intended to be changed easily when required more information of room such as room name,
    // maximum number of players, game mode, etc.
    public struct MatchServerRequest : NetworkMessage { }
}