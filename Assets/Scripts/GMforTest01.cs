using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    //ゴールエリアの幅
    [Range(0.5f, 10.0f)]
    public double goalWidth = 5;

    //ゴールエリア枠線の色
    [SerializeField] Color goalLineColor;

    //侵入不可エリアの幅
    [Range(0.5f, 10.0f)]
    public double impenetrableWidth = 5;

    //侵入不可エリア枠線の色
    [SerializeField] Color impenetrableLineColor;

    //インク残量(秒)
    public double _inkLeft;

    //ゴールエリア侵入フラグ
    public int goalInFrag = 0;

    //HP表示用
    public Text HPtext;
    //最大HP
    [Range(1f,10f)]
    public int MaxHP = 2;
    //HP計算用
    public int HP;

    //勝敗判定用
    public Text winnnertext;

    //無敵時間
    [Range(0.0f, 5.0f)]
    public double maxGodMode;
    //無敵時間計算用
    public double godModeCount;
    //無敵時間フラグ
    public int godModeFrag = 0;

    // Start is called before the first frame update
    void Start()
    {
        //HP初期化
        HP = MaxHP;
        //HP表示初期化
        HPtext.text = "HP:" + HP;

        //Listの初期化
        lineRenderers = new List<LineRenderer>();

        //インク残量の初期化
        _inkLeft = MaxInkAmount;

        setArea();

    }

    // Update is called once per frame
    void Update()
    {
        //マウスポインタがあるスクリーン座標を取得
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            //lineObjを生成し、初期化する
            _addLineObject();


        }

        if (Input.GetMouseButton(0))
        {
            //インク残量があって、侵入不可エリア外なら
            if (_inkLeft > 0 && worldPosition.x >= -10.5+goalWidth+impenetrableWidth) 
            { 
                //線を描画
                _addPositionDataToLineRendererList(); 

               //インク残量を減らす
               _inkLeft -= Time.deltaTime;

            }
           
        }

        //マウスボタンが離されていれば
        if (! Input.GetMouseButton(0) && _inkLeft <= 2)

        {
            //インクを回復
            _inkLeft += Time.deltaTime * InkRecovery;

        }

        if (goalInFrag == 1 && godModeFrag == 0)
        {
            //HP更新
            HP  --;
            //HP表示更新
            HPtext.text = "HP:" + HP;
            //無敵時間開始
            godModeCount = 0;
            godModeFrag = 1;
        }

        //無敵時間の経過時間を計測
        //無敵時間開始時にリセットされる
        godModeCount += Time.deltaTime;

        //無敵時間が終わったか判定
        if(godModeCount >= maxGodMode)
        {
            godModeFrag = 0;
        }

        //勝敗判定
        if(HP <= 0)
        {
            winnnertext.text = "Player Win !";
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

    void polygonLinerenderer(Vector3[] tops,string name,Material material,Color color,float width)
    {
        //空のゲームオブジェクト作成
        GameObject lineObj = new GameObject();
        //オブジェクトの名前を変更
        lineObj.name = name;
        //オブジェクトのレイヤーを指定
        lineObj.layer = 2;
        //lineObjにLineRendereコンポーネント追加
        lineObj.AddComponent<LineRenderer>();
        //lineRendererリストにlineObjを追加
        lineRenderers.Add(lineObj.GetComponent<LineRenderer>());
        //lineObjを自身の子要素に設定
        lineObj.transform.SetParent(transform);

        //lineObj初期化処理
        //線をつなぐ点を0に初期化
        lineRenderers.Last().positionCount = 0;
        //マテリアルを初期化
        lineRenderers.Last().material = material;
        //色の初期化
        lineRenderers.Last().material.color = color;
        //太さの初期化
        lineRenderers.Last().startWidth = width;
        lineRenderers.Last().endWidth = width;

        foreach (var item in tops)
        {
            //lineRenderersに頂点を追加
            lineRenderers.Last().transform.localPosition = item;

            //lineObjの線と線をつなぐ点の数を更新
            lineRenderers.Last().positionCount += 1;

            //LineRendererコンポーネントリストを更新→描画される
            lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1,item);
        }
        
    }

    void setArea()
    {
        //ゴールエリアの描画
        //頂点の設定
        Vector3[] goalTops = new Vector3[] { new Vector3(-10.5f, 5.0f, 0f), new Vector3(-10.5f, -5.0f, 0.0f), new Vector3(-10.5f + (float)goalWidth, -5.0f, 0.0f), new Vector3(-10.5f + (float)goalWidth, 5.0f, 0.0f), new Vector3(-10.5f, 5.0f, 0f) };
        //描画
        polygonLinerenderer(goalTops, "goalLine", lineMaterial, goalLineColor, lineWidth);

        //侵入不可エリアの描画
        //頂点の設定
        Vector3[] impenetrableTops = new Vector3[] { new Vector3(-10.5f + (float)goalWidth + (float)lineWidth, 5.0f, 0f), new Vector3(-10.5f + (float)goalWidth + (float)lineWidth, -5.0f, 0.0f), new Vector3(-10.5f + (float)goalWidth + (float)impenetrableWidth, -5.0f, 0.0f), new Vector3(-10.5f + (float)goalWidth + (float)impenetrableWidth, 5.0f, 0.0f), new Vector3(-10.5f + (float)goalWidth, 5.0f, 0f) };
        //描画
        polygonLinerenderer(impenetrableTops, "inpenetrableLine", lineMaterial, impenetrableLineColor, lineWidth);
    }
}
