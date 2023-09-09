using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAnimaBehaviour : MonoBehaviour
{

    private PhysicsMaterial2D greenMat;
    private Rigidbody2D rb;
    [SerializeField] private float bounciness = 1.0f;
    [SerializeField] private float friction = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {

        greenMat.friction = friction;
        greenMat.bounciness = bounciness;
        this.gameObject.AddComponent<Rigidbody2D>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.sharedMaterial = greenMat;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
