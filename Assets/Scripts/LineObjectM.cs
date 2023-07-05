using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectM : MonoBehaviour
{

    //注意：このスクリプトはプレイヤーが描いた各オブジェクトにアタッチされます
    //このスクリプトがアタッチされたオブジェクトを消去するときはgoalInFragをリセットする必要があります

    
    public int moveFrag = 0;

    public int destroyFrag = 0;

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

        
        //描いたオブジェクトの情報を取得
        GameObject lineObj2;
        lineObj2 = GameObject.Find("Stroke");
        LineRenderer lineRenderer = lineObj2.GetComponent<LineRenderer>();

        //描いたオブジェクトの位置を格納するための変数
        Vector3[] linePoint = new Vector3[lineRenderer.positionCount];

        //描いたオブジェクトの位置を格納
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            linePoint[i] = lineRenderer.GetPosition(i);
        }

        //spaceキーが押されたら描いたオブジェクトを動かす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveFrag = 1;
        }

 
        if(moveFrag == 1)
        {
            //描いたオブジェクトを移動させる
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                //移動させる位置を計算
                linePoint[i] += new Vector3(-0.01f, 0, 0);
            }

            //移動後の位置に描画
            lineRenderer.SetPositions(linePoint);
        }

        //ゴールエリアに描いたオブジェクトの一部が入っていたらゴールフラグを立てる
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            if (linePoint[i].x < -10.5f + (float)goalWidth)
            {
                script.goalInFrag = 1;
            }
        }

        //描いたオブジェクトが全部画面外に出たらオブジェクトを消去する
        destroyFrag = 1;
        for (int i =0; i < lineRenderer.positionCount; i++)
        {
            
            if (linePoint[i].x > -10.5f)
            {
                destroyFrag = 0;
                break;
            } 
        }
        if(destroyFrag == 1)
        {
            script.goalInFrag = 0;
            Destroy(this.gameObject);
        }
        destroyFrag = 0;
    }
}
