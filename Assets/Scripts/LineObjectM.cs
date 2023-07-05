using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectM : MonoBehaviour
{

    public int moveFrag = 0;

    //スクリプト取得用
    public GameObject DrawingCampas;
    GM script;

    //アタッチされたオブジェクトのLinerendere取得用
    LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GMのgoalwithを受け取る変数
        double goalWidth;

        //CubeのスクリプトGMからゴールエリアのサイズを取得
        DrawingCampas = GameObject.Find("DrawingCanvas");
        script = DrawingCampas.GetComponent<GM>();
        goalWidth = script.goalWidth;

        

        GameObject lineObj2;
        lineObj2 = GameObject.Find("Stroke");
        LineRenderer lineRenderer = lineObj2.GetComponent<LineRenderer>();

        Vector3[] linePoint = new Vector3[lineRenderer.positionCount];

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            linePoint[i] = lineRenderer.GetPosition(i);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveFrag = 1;
        }

        if(moveFrag == 1)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                linePoint[i] += new Vector3(-0.01f, 0, 0);
            }


            lineRenderer.SetPositions(linePoint);
        }

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            if (linePoint[i].x < -10.5f + (float)goalWidth)
            {
                script.goalInFrag = 1;
            }
        }

    }
}
