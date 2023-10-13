using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLineEntryDetector : ObjectCollisionDetector
{
    void OnCollisionWithAnima(GameObject obj, ContactPoint2D contactPoint)
    {
        //TODO: Inform owning player manager to reduce health and enter invulnerability
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCollisionWithHostile += OnCollisionWithAnima;
    }


}
