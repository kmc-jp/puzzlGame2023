using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GM : MonoBehaviour
{
    //線の材質
    [SerializeField] Material lineMaterial;
    //線の色
    [SerializeField] Color lineColor;
    //線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float lineWidth;

    //LineRdenerer型のリスト宣言
    List<LineRenderer> lineRenderers;

    [Range(0.0f, 100.0f)]
    public double MaxInkAmount = 2.0;
    
    [Range(0.0f, 100.0f)]
    public double InkRecovery = 0.5;
    public double inkAmount;
    //インク残量(秒)
    public double inkLeft;
    //インク回復量(秒/秒)
    public double inkRecovery;

    // Start is called before the first frame update
    void Start()
    {
        //Listの初期化
        lineRenderers = new List<LineRenderer>();

        //インク総量の初期化
        inkAmount =2.0;
        //インク残量の初期化
        inkLeft = inkAmount;

        Debug.Log(inkLeft);
        //インク回復量の初期化
        inkRecovery = 0.5;
     }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //lineObjを生成し、初期化する
            _addLineObject();

            Debug.Log(inkLeft);
        }

        if (Input.GetMouseButton(0))
        { 
            //インク残量があれば
            if (inkLeft >0) 
            { 
                //線を描画
                _addPositionDataToLineRendererList(); 

               //インク残量を減らす
               inkLeft -= Time.deltaTime;
            }
           
        }

        //マウスボタンが離されていれば
        if (! Input.GetMouseButton(0) && inkLeft <= 2)
        {
            //インクを回復
            inkLeft += Time.deltaTime * inkRecovery;
        }
    }

    //クリックしたら発動
    void _addLineObject()
    {
        //空のゲームオブジェクト作成
        GameObject lineObj = new GameObject();
        //オブジェクトの名前をStrokeに変更
        lineObj.name = "Stroke";
        //lineObjにLineRendereコンポーネント追加
        lineObj.AddComponent<LineRenderer>();
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
