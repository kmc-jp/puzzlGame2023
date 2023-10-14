using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkGameplayInitializer : NetworkBehaviour
{
    private bool _initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeNetwork();
    }

    // Update is called once per frame
    void Update()
    {
        InitializeNetwork();
    }

    private void InitializeNetwork()
    {
        if (_initialized)
        {
            return;
        }

        // Left goal is player 1, Right goal is player 2
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();
        PlayerManager player1 = null;
        PlayerManager player2 = null;
        foreach (PlayerManager p in players)
        {
            if (p.isServer && p.isLocalPlayer)
            {
                player1 = p;
            }
            else
            {
                player2 = p;
            }
        }
        if (player1 != null && player2 != null)
        {
            GameObject.Find("GoalObject_1")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(player1.GetComponent<NetworkIdentity>().netId);
            GameObject.Find("GoalObject_2")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(player2.GetComponent<NetworkIdentity>().netId);
            _initialized = true;
        }
    }
}
