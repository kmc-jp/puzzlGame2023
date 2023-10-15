using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OfflineNetworkManager : NetworkManager
{
    private bool _initialized = false;

    // Start is called before the first frame update
    public override void Start()
    {
        StartHost();
        base.Start();

        InitializeNetwork();
    }

    public override void Update()
    {
        InitializeNetwork();

        base.Update();
    }

    private void InitializeNetwork()
    {
        if (_initialized)
        {
            return;
        }

        // Force left goal to be player and right goal to be enemy
        PlayerManager player = FindObjectOfType<PlayerManager>();
        if (player != null) {
            uint localNetId = player.GetComponent<NetworkIdentity>().netId;
            GameObject.Find("GoalObject_1")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(localNetId);
            GameObject.Find("GoalObject_2")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(localNetId + 1);
            _initialized = true;
        }
    }
}
