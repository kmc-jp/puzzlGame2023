using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NetworkGameplayInitializer : NetworkBehaviour
{
    private bool _initialized = false;

    public PlayerManager Player1 = null;
    public PlayerManager Player2 = null;
    public int LocalPlayerIndex = -1; // 0 = Player 1, 1 = Player 2

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
        foreach (PlayerManager p in players)
        {
            if (isServer)
            {
                if (p.isLocalPlayer)
                {
                    Player1 = p;
                    LocalPlayerIndex = 0;
                }
                else
                {
                    Player2 = p;
                }
            }
            else
            {
                if (p.isLocalPlayer)
                {
                    Player2 = p;
                    LocalPlayerIndex = 1;
                }
                else
                {
                    Player1 = p;
                }
            }
        }
        if (Player1 != null && Player2 != null)
        {
            GameObject.Find("GoalObject_1")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(Player1.GetComponent<NetworkIdentity>().netId);
            GameObject.Find("GoalObject_2")?.GetComponent<GoalLineEntryDetector>()?.SetOwnerNetworkId(Player2.GetComponent<NetworkIdentity>().netId);
            _initialized = true;
        }
    }
}
