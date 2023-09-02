using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueAnimaMovementBehaviour : MonoBehaviour
{
    Physics2D Physics2D;
    // Start is called before the first frame update
    void Start()
    {
        _addRigidBody();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    void _addRigidBody()
    {
        this.gameObject.AddComponent<Rigidbody>();
        var rb = this.GetComponent<Rigidbody2D>();

        //重さの調整
        rb.useAutoMass = true;
        float massValue = rb.mass;
        rb.useAutoMass = false;
        rb.mass = massValue;

        rb.angularDrag = 10;     
        rb.simulated = true;
    }
}
