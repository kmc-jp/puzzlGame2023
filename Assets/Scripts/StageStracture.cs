using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StageStracture : MonoBehaviour
{
    //衝突してから消えるまでの時間
    public float WaitTime = 0.1f;
    //書いた線の名前
    public string DrawnLineName = "Stroke";

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //触れたものがプレイヤーの書いた物体ならdestroy

        if (collision.gameObject.name == DrawnLineName)//
        {
            Debug.Log("Hit");
            Destroy(gameObject, WaitTime);
        }
    }

}