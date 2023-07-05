using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectM : MonoBehaviour
{

    public int moveFrag = 0;
    public GameObject circle;

    //Cubeのスクリプト取得用
    public GameObject DrawingCampas;
    GM script;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GMのgoalwithを受け取る変数
        double goalWidth;

        //CubeのスクリプトGMからインク総量を取得
        DrawingCampas = GameObject.Find("DrawingCanvas");
        script = DrawingCampas.GetComponent<GM>();
        goalWidth = script.goalWidth;

        //LineRenderer lineRenderer = circle.GetComponent<LineRenderer>();
        Transform transform = circle.transform;

        //for (int i= 0; i < lineRenderer.positionCount; i++)
        //{
        //    if (false)
        //    {
        //        Debug.Log("detect");
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveFrag = 1;
        }

        if(moveFrag == 1)
        {
            transform.position += new Vector3(-0.01f, 0.0f, 0.0f);
        }

        if (transform.position.x <= -10.5f + (float)goalWidth)
        {
           script.goalInFrag = 1;
        }

    }
}
