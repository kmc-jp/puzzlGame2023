using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAnimaBehaviourHM : MonoBehaviour
{
    public float firstImpact = -10f;
    //単振動 定数
    public float k = 4.0f;

    private Vector2 startPos;
    private Vector2 nowPos;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(new Vector2 (firstImpact,0f) ,ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        nowPos = transform.position;
        Vector2 force = new Vector2 ((startPos.x - nowPos.x) * k , 0f);

        rb2d.AddForce(force);

    }
}
