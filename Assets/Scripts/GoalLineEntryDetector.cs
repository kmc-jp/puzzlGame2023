using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalLineEntryDetector : ObjectCollisionDetector
{
    void OnCollisionWithAnima(GameObject obj, Vector2 contactPoint)
    {
        // Inform owning player manager to reduce health and enter invulnerability
        PlayerManager localPlayer = Array.Find(
            FindObjectsByType(typeof(PlayerManager), FindObjectsSortMode.None),
            obj => obj.GetComponent<NetworkIdentity>().netId == _ownerNetworkId
            ) as PlayerManager;
        if (localPlayer != null && localPlayer.TakeDamage(1, true))
        {
            RpcEnableBlinkEffect(localPlayer.MaxGodMode);
        }

        // Allow blinking effect when testing in single player mode
        if (Debug.isDebugBuild && FindObjectsByType(typeof(PlayerManager), FindObjectsSortMode.None).Length == 1)
        {
            RpcEnableBlinkEffect(localPlayer != null ? localPlayer.MaxGodMode : 8.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCollisionWithHostile += OnCollisionWithAnima;
    }

    [ClientRpc]
    void RpcEnableBlinkEffect(float duration)
    {
        GetComponent<BlinkEffect>()?.EnableBlink(duration);
    }


}
