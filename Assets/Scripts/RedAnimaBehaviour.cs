using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAnimaBehaviour : MonoBehaviour
{

    //vectorReverse 運動の向きが逆になります
    //power 物体に与える力が強くなります。
    //speed 運動の切り替わりの速度が早くなります
    [SerializeField] private bool vectorReverse = false;
    [SerializeField] private float power = 4.0f;
    [SerializeField] private float speed = 3.0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (vectorReverse) power = -power;
    }

    // Update is called once per frame
    void Update()
    {
        GiveForce();
    }

    void GiveForce()
    {
        if ((Time.time * speed) < 1)
        {
            rb.AddForce(new Vector2(power, 0f));
        }
        else if((Time.time * speed) < 3)
        {
            rb.AddForce(new Vector2(-power, 0f));
        }
    }
}
