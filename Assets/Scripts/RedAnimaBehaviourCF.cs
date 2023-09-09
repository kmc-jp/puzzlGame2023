using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAnimaBehaviourCF : MonoBehaviour
{

    //
    public float power = 4.0f;
    public float speed = 3.0f;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Mathf.Sin(Time.time * speed)) < 0){
            rb2d.AddForce(new Vector2(power, 0f));
        }
        else
        {
            rb2d.AddForce(new Vector2(-power, 0f));
        }

    }
}
