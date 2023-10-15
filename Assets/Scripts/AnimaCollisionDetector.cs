using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaCollisionDetector : ObjectCollisionDetector
{
    void OnCollisionWithGoal(GameObject obj, Vector2 contactPoint)
    {
        if (obj.GetComponent<GoalLineEntryDetector>() != null)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    void OnCollisionWithAnima(GameObject obj, Vector2 contactPoint)
    {
        if (obj.GetComponent<AnimaCollisionDetector>() != null)
        {
            //TODO: Reduce anima health instead of outright destroying
            NetworkServer.Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCollisionWithHostile += OnCollisionWithGoal;
        OnCollisionWithHostile += OnCollisionWithAnima;
    }


}
