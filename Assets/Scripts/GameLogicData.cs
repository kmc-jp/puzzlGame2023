using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameLogicData : MonoBehaviour
{
    [Tooltip("Object's maximum health")]
    public float MaxHealth;

    [Tooltip("Multiplier applied to this object's damage when colliding with another object")]
    public float DamageMultiplier = 1.0f;

    [Tooltip("Player object that owns this object. If not set, defaults to the parent object")]
    public GameObject Owner = null;

    GameLogicManager _stageGameManager = null;
    float _curHealth;

    public float CalculateDamage(GameObject otherObject)
    {
        // No damage if no game logic data or owned by the same player
        GameLogicData otherData = otherObject.GetComponent<GameLogicData>();
        if (otherData != null && otherData.Owner != Owner)
        {
            float totalDamage = 0.0f;

            //TODO:
            // Calculate base damage using the object's mass and damage multiplier

            // Modify damage by the object's velocity relative to the object it is colliding with

            return totalDamage;
        }
        return 0.0f;
    }

    public void TakeDamage(float damageAmount, GameObject damageDealer)
    {
        _curHealth -= damageAmount;

        if (_curHealth < 0)
        {
            _stageGameManager.DestroyStageObject(gameObject);
        }
    }

    // Called when the object should be destroyed
    // If this returns true, the object will be destroyed upon returning from the function
    // If this returns false, the object will not be destroyed and this function will be called again on the next frame
    public bool OnDeath()
    {
        UnityEngine.Debug.Log(string.Format("'{0}' OnDeath", name));
        return true;
    }

    void Start()
    {
        GameObject[] gameManager = GameObject.FindGameObjectsWithTag("GameController");
        UnityEngine.Debug.AssertFormat(gameManager.Length > 0, "GameLogicManager does not exist for stage");

        _stageGameManager = gameManager[0].GetComponent<GameLogicManager>();
        _curHealth = MaxHealth;

        // If no Owner explicitly set, default to the parent object
        if (Owner == null)
        {
            Owner = transform.parent.gameObject;
        }
    }

}