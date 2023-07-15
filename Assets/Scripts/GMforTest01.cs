using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    //標準の線の材質
    [SerializeField] Material lineMaterial;
   
    //標準の線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float lineWidth;

    //プレイヤーが描画する線の色
    [SerializeField] Color lineColor;

    //LineRdenerer型のリスト宣言
    List<LineRenderer> lineRenderers;

    //描画中フラグ
    [Tooltip("描画中フラグ")]
    public int drawFlag;

    //インクの最大量(秒)
    [Range(0.0f, 100.0f)]
    public double MaxInkAmount = 2.0;
    
    //インクの回復測度(秒/秒)
    [Range(0.0f, 100.0f)]
    public double InkRecovery = 0.5;
   
    //インク残量(秒)
    public double _inkLeft;

    //ゴールエリア侵入フラグ
    public int goalInFlag = 0;

    //スクリプト取得用
    public GameObject DrawingCampas;
    StageMnager StageMnager;

    // Start is called before the first frame update
    void Start()
    {
        //Listの初期化
        lineRenderers = new List<LineRenderer>();

        //インク残量の初期化
        _inkLeft = MaxInkAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスポインタがあるスクリーン座標を取得
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //CubeのスクリプトStageMnagerからゴールエリアのサイズを取得
        DrawingCampas = GameObject.Find("DrawingCanvas");
        StageMnager = DrawingCampas.GetComponent<StageMnager>();
        //GMのgoalwithを受け取る変数
        double goalWidth;
        goalWidth = StageMnager.goalWidth;
        double impenetrableWidth;
        impenetrableWidth = StageMnager.impenetrableWidth;

        //マウスがクリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            //lineObjを生成し、初期化する
            _addLineObject();

            //描画中フラグを立てる
            drawFlag = 1;
        }

        //マウスボタンが離されていれば
        if (!Input.GetMouseButton(0))
        {
            drawFlag = 0;
        }

        //侵入不可内なら
        if (worldPosition.x <= -10.5 + goalWidth + impenetrableWidth)
        {
            drawFlag = 0;
        }

        //マウスがクリックされているとき
        if (Input.GetMouseButton(0))
        {
            //インク残量があって、描画中なら
            if (_inkLeft > 0 && drawFlag == 1)
            {
                //線を描画
                _addPositionDataToLineRendererList();

                //インク残量を減らす
                _inkLeft -= Time.deltaTime;

            }

        }



        //マウスボタンが離されていて、インクが最大より少なければ
        if (!Input.GetMouseButton(0) && _inkLeft <= MaxInkAmount)

        {
            //インクを回復
            _inkLeft += Time.deltaTime * InkRecovery;

        }

    }

    //クリックしたら発動
    void _addLineObject()
    {
        //空のゲームオブジェクト作成
        GameObject lineObj = new GameObject();
        //オブジェクトにLineObjectMをアタッチ
        lineObj.AddComponent<LineObjectM>();
        //オブジェクトの名前をStrokeに変更
        lineObj.name = "Stroke";
        //lineObjにLineRendereコンポーネント追加
        lineObj.AddComponent<LineRenderer>();
        //lineObjのLineRendererコンポーネントを取得
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        //useWorldSpaceをtrueに
        lineRenderer.useWorldSpace = true;
        //lineRendererリストにlineObjを追加
        lineRenderers.Add(lineObj.GetComponent<LineRenderer>());
        //lineObjを自身の子要素に設定
        lineObj.transform.SetParent(transform);
        //lineObj初期化処理
        _initRenderers();
    }

    //lineObj初期化処理
    void _initRenderers()
    {
        //線をつなぐ点を0に初期化
        lineRenderers.Last().positionCount = 0;
        //マテリアルを初期化
        lineRenderers.Last().material = lineMaterial;
        //色の初期化
        lineRenderers.Last().material.color = lineColor;
        //太さの初期化
        lineRenderers.Last().startWidth = lineWidth;
        lineRenderers.Last().endWidth = lineWidth;
    }

    void _addPositionDataToLineRendererList()
    {
        //マウスポインタがあるスクリーン座標を取得
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //ワールド座標をローカル座標に変換
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition.x, worldPosition.y, -1.0f);

        //lineRenderersの最後のlineObjのローカルポジションを上記のローカルポジションに設定
        lineRenderers.Last().transform.localPosition = localPosition;

        //lineObjの線と線をつなぐ点の数を更新
        lineRenderers.Last().positionCount += 1;

        //LineRendererコンポーネントリストを更新
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, worldPosition);

        //あとから描いた線が上に来るように調整
        lineRenderers.Last().sortingOrder = lineRenderers.Count;
    }
}
