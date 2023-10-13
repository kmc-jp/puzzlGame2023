using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaCollisionDetector : ObjectCollisionDetector
{
    void OnCollisionWithGoal(GameObject obj, ContactPoint2D contactPoint)
    {
        if (obj.GetComponent<GoalLineEntryDetector>() != null)
        {
            Object.Destroy(this);
        }
    }

    void OnCollisionWithAnima(GameObject obj, ContactPoint2D contactPoint)
    {
        if (obj.GetComponent<AnimaCollisionDetector>() != null)
        {
            //TODO: Reduce anima health instead of outright destroying
            Object.Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCollisionWithHostile += OnCollisionWithGoal;
        OnCollisionWithHostile += OnCollisionWithAnima;
    }


}
