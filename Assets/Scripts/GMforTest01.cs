using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    GameObject lineObj;

    //LineRdenerer型のリスト宣言
    List<LineRenderer> lineRenderers;

    GameObject colliderContainer;

    // Start is called before the first frame update
    void Start()
    {
        //Listの初期化
        lineRenderers = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //lineObjを生成し、初期化する
            _addLineObject();
        }

        if (Input.GetMouseButton(0)) {
            Debug.Log("on");
            _addPositionDataToLineRendererList();
        }

        if (Input.GetMouseButtonUp(0)) {
            _completeLineObject();
        }
    }

    //クリックしたら発動
    void _addLineObject()
    {
        //空のゲームオブジェクト作成
        lineObj = new GameObject();
        //オブジェクトの名前をStrokeに変更
        lineObj.name = "Stroke";
        //lineObjにLineRendereコンポーネント追加
        lineObj.AddComponent<LineRenderer>();
        //lineRendererリストにlineObjを追加
        lineRenderers.Add(lineObj.GetComponent<LineRenderer>());
        //lineObjを自身の子要素に設定
        lineObj.transform.SetParent(transform);

        //colliderContainerを初期化
        colliderContainer = new GameObject();
        colliderContainer.name = "ColliderContainer";

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

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
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

    void _completeLineObject() {
        _createMeshCollider();
    }

    void _createMeshCollider() { 
        colliderContainer.AddComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        lineRenderers.Last().BakeMesh(mesh);
        colliderContainer.GetComponent<MeshCollider>().sharedMesh = mesh;
        colliderContainer.transform.SetParent(lineRenderers.Last().transform);
    }
}