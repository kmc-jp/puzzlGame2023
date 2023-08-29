using Mirror;
using UnityEngine;

public class BattleNetworkRoomManager : NetworkRoomManager {
    public override void OnRoomStartHost() {
        base.OnRoomStartHost();
    }

    public override void OnRoomStopHost() {
        base.OnRoomStopHost();
    }

    public override void OnRoomStartServer() {
        base.OnRoomStartServer();
    }

    public override void OnRoomStopServer() {
        base.OnRoomStopServer();
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn) {
        base.OnRoomServerConnect(conn);
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn) {
        base.OnRoomServerDisconnect(conn);
    }

    public override void OnRoomServerSceneChanged(string sceneName) {
        OnRoomServerSceneChanged(sceneName);
    }

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn) {
        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer) {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }

    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn) {
        base.OnRoomServerAddPlayer(conn);
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer) {
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    public override void ReadyStatusChanged() {
        base.ReadyStatusChanged();
    }
    
    public override void OnRoomServerPlayersReady() {
        base.OnRoomServerPlayersReady();
    }

    public override void OnRoomServerPlayersNotReady() {
        base.OnRoomServerPlayersNotReady();
    }


    public override void OnRoomClientEnter() {
        base.OnRoomClientEnter();
    }

    public override void OnRoomClientExit() {
        base.OnRoomClientExit();
    }

    public override void OnRoomClientConnect() {
        base.OnRoomClientConnect();
    }

    public override void OnRoomClientDisconnect() {
        base.OnRoomClientDisconnect();
    }

    public override void OnRoomStartClient() {
        base.OnRoomStartClient();
    }

    public override void OnRoomStopClient() {
        base.OnRoomStopClient();
    }

    public override void OnRoomClientSceneChanged() {
        base.OnRoomClientSceneChanged();
    }
}