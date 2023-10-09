using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OfflineNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    void Start()
    {
        StartHost();

        base.Start();
    }
}
