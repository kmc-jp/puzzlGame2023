using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAnimaBehaviourMP : MonoBehaviour
{
    //往復の幅
    [SerializeField] private float length = 4.0f;
    //周期の切り替わりの速さ
    [SerializeField] private float speed = 2.0f;
    private Vector3 startPos;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        determinateMovePosition();
    }

    void determinateMovePosition()
    {
        rb2d.MovePosition(
            new Vector2(
                (Mathf.Sin((Time.time) * speed) * length + startPos.x), 
                transform.position.y)
            );
    }
}
