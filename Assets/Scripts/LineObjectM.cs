using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectM : MonoBehaviour
{

    //注意：このスクリプトはプレイヤーが描いた各オブジェクトにアタッチされます
    //このスクリプトがアタッチされたオブジェクトを消去するときはgoalInFragをリセットする必要があります
    //HP機能テスト用にspaceを押すとオブジェクトが左に動きだすようになっています

    //テスト用移動フラグ
    public int moveFrag = 0;

    //自己破壊フラグ
    public int destroyFrag = 0;

    //スクリプト取得用
    public GameObject DrawingCanvas;
    StageManager StageManager;

    //アタッチされたオブジェクトのLinerendere取得用
    public LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        //ゴール判定
        judgeGoal();
        //テスト用のオブジェクト移動機能
        moveObj();
        //描いたオブジェクトが全部画面外に出たらオブジェクトを消去する
       JudgeDestroy();
    }

    //描いたオブジェクトの位置を取得
    Vector3[] getLinePoint()
    {
        //描いたオブジェクトの情報を取得
        lineRenderer = this.GetComponent<LineRenderer>();

        //オブジェクトの位置を格納するための配列
        Vector3[] linePoint = new Vector3[lineRenderer.positionCount];

        //描いたオブジェクトの位置を格納
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            linePoint[i] = lineRenderer.GetPosition(i);
        }

        return linePoint;
    }

    //描いたオブジェクトが全部画面外に出たらオブジェクトを消去する
    void JudgeDestroy()
    {
        destroyFrag = 1;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {

            if (getLinePoint()[i].x > -10.5f)
            {
                destroyFrag = 0;
                break;
            }
        }
        if (destroyFrag == 1)
        {
            //GM.goalInFlag = 0;
            Destroy(this.gameObject);
        }
        destroyFrag = 0;
    }

    //ゴール判定
    void judgeGoal()
    {
        //描いたオブジェクトの位置を取得
        Vector3[] linePoint = getLinePoint();

        //GMのgoalwithを受け取る変数
        double goalWidth;

        //CubeのスクリプトStageMnagerからゴールエリアのサイズを取得
        DrawingCanvas = GameObject.Find("DrawingCanvas");
        StageManager = DrawingCanvas.GetComponent<StageManager>();
        goalWidth = StageManager.goalWidth;

        //GMを取得
        DrawingCanvas = GameObject.Find("DrawingCanvas");
        //GM = DrawingCanvas.GetComponent<GM>();

        //ゴールエリアに描いたオブジェクトの一部が入っていたらゴールフラグを立てる
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            if (linePoint[i].x < -10.5f + (float)goalWidth)
            {
                //GM.goalInFlag = 1;
            }
        }
    }

    //テスト用のオブジェクト移動機能
    void moveObj()
    {
        //描いたオブジェクトの位置を取得
        Vector3[] linePoint = getLinePoint();

        //spaceキーが押されたら描いたオブジェクトを動かす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveFrag = 1;
        }

        //描いたオブジェクトを移動させる
        if (moveFrag == 1)
        {
            //移動させる位置を計算
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                linePoint[i] += new Vector3(-0.01f, 0, 0);
            }

            //移動後の位置に描画
            lineRenderer.SetPositions(linePoint);
        }
    }
}
