using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class StageMnager : MonoBehaviour
{

    //標準の線の材質
    [SerializeField] Material lineMaterial;

    //標準の線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float lineWidth;

    //標準の線の色
    [SerializeField] Color lineColor;

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

    //LineRdenerer型のリスト宣言
    List<LineRenderer> lineRenderers;

    // Start is called before the first frame update
    void Start()
    {
        //Listの初期化
        lineRenderers = new List<LineRenderer>();

        setArea();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void polygonLinerenderer(Vector3[] tops, string name, Material material, Color color, float width)
    {
        //新しいオブジェクト作成
        GameObject lineObj = new GameObject();
        lineObj.name = name;
        lineObj.layer = 0;//レイヤーを指定
        lineObj.AddComponent<LineRenderer>();
        lineRenderers.Add(lineObj.GetComponent<LineRenderer>());
        lineObj.transform.SetParent(transform);

        //lineObj初期化処理
        lineRenderers.Last().positionCount = 0;
        lineRenderers.Last().material = material;
        lineRenderers.Last().material.color = color;
        lineRenderers.Last().startWidth = width;
        lineRenderers.Last().endWidth = width;

        foreach (var item in tops)
        {
            //lineRenderersに頂点を追加
            lineRenderers.Last().transform.localPosition = item;

            //lineObjの線と線をつなぐ点の数を更新
            lineRenderers.Last().positionCount += 1;

            //LineRendererコンポーネントリストを更新→描画される
            lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, item);
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
