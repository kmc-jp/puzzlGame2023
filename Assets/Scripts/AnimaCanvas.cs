using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimaCanvas : MonoBehaviour
{
    [SerializeField] StageInputController InputController;

    //標準の線の材質
    [SerializeField] Material DefaultLineMaterial;

    //標準の線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float DefaultLineWidth;

    [SerializeField] PhysicsMaterial2D DefaultPhysicsMaterial;

    //プレイヤーが描画する線の色
    [SerializeField] Color DefaultLineColor;

    [SerializeField] float SimplifyColliderTolerance = 50.0f;

    public Color[] LineColors;

    public PlayerManager Player;


    [Flags]
    private enum AnimaCanvasState
    {
        None = 0,
        Drawing = 1
    }

    private AnimaCanvasState _state = AnimaCanvasState.None;

    private GameObject tempDrawingObject = null;
    private LineRenderer tempDrawingLineRenderer = null;
    private Color curDrawingColor = Color.black;

    public void BeginDraw(Vector3 startPosition, int color)
    {
        Debug.Assert((_state & AnimaCanvasState.Drawing) != AnimaCanvasState.Drawing);

        _state |= AnimaCanvasState.Drawing;

        curDrawingColor = DefaultLineColor;
        if (LineColors.Length > color)
        {
            curDrawingColor = LineColors[color];
        }

        tempDrawingObject = new GameObject();
        tempDrawingObject.name = "TempStroke";
        tempDrawingObject.transform.parent = transform;
        tempDrawingObject.transform.position = startPosition;

        tempDrawingLineRenderer = tempDrawingObject.AddComponent<LineRenderer>();
        tempDrawingLineRenderer.useWorldSpace = true;
        tempDrawingLineRenderer.material = DefaultLineMaterial;
        tempDrawingLineRenderer.material.color = curDrawingColor;
        tempDrawingLineRenderer.startWidth = DefaultLineWidth;
        tempDrawingLineRenderer.endWidth = DefaultLineWidth;
    }

    public void EndDraw(Vector3 endPosition, bool cancel)
    {
        if ((_state & AnimaCanvasState.Drawing) != AnimaCanvasState.Drawing)
        {
            return;
        }

        _state &= ~AnimaCanvasState.Drawing;

        if (!cancel)
        {
            // Create the Anima object from the LineRenderer
            _createAnimaObject();
        }

        GameObject.Destroy(tempDrawingObject);
        tempDrawingLineRenderer = null;
        tempDrawingObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnAnimaDrawingStart += BeginDraw;
        InputController.OnAnimaDrawingEnd += EndDraw;
    }

    // Update is called once per frame
    void Update()
    {
        // Drawing
        if ((_state & AnimaCanvasState.Drawing) == AnimaCanvasState.Drawing)
        {
            //マウスポインタのワールド座標を取得
            Vector3 mousePosition = InputController.GetAnimaDrawingPositionWorld();

            //TODO: Verify the point is within the bounds of the canvas
            if (_positionInBounds(mousePosition))
            {
                //線を描画
                _addPositionDataToLineRenderer(mousePosition);
            }
        }
    }

    void _addPositionDataToLineRenderer(Vector3 worldPosition)
    {
        worldPosition.z = 0.0f;

        //lineRendererの線と線をつなぐ点の数を更新
        tempDrawingLineRenderer.positionCount += 1;

        //lineRendererを更新
        tempDrawingLineRenderer.SetPosition(tempDrawingLineRenderer.positionCount - 1, worldPosition);

        //あとから描いた線が上に来るように調整
        tempDrawingLineRenderer.sortingOrder = 1;
    }

    bool _positionInBounds(Vector3 worldPosition)
    {
        // Layer: Canvas = 1 << 8
        RaycastHit2D[] results = Physics2D.RaycastAll(worldPosition, Vector2.zero, 0, 1 << 8);
        foreach (RaycastHit2D i in results)
        {
            AnimaCanvas foundCanvas = i.transform.gameObject.GetComponent<AnimaCanvas>();
            if (foundCanvas && ReferenceEquals(foundCanvas, this))
            {
                return true;
            }
        }
        return false;
    }

    void _createAnimaObject()
    {
        //TODO: Build from a prefab?

        //空のゲームオブジェクト作成
        GameObject newLineObj = new GameObject();
        //オブジェクトにLineObjectMをアタッチ
        //newLineObj.AddComponent<LineObjectM>();
        //オブジェクトの名前をStrokeに変更
        newLineObj.name = "Stroke";
        //lineObjを自身の子要素に設定
        newLineObj.transform.SetParent(transform);

        //LineRendererをMesh化して、新しいラインオブジェクトに付く
        MeshFilter newFilter = newLineObj.AddComponent<MeshFilter>();
        tempDrawingLineRenderer.BakeMesh(newFilter.mesh);

        MeshRenderer newRenderer = newLineObj.AddComponent<MeshRenderer>();
        newRenderer.material = DefaultLineMaterial;
        newRenderer.material.color = curDrawingColor;

        //コライダーを付く
        tempDrawingLineRenderer.Simplify(SimplifyColliderTolerance);
        Mesh colliderMesh = new Mesh();
        tempDrawingLineRenderer.BakeMesh(colliderMesh);
        MeshCollider newCollider = newLineObj.AddComponent<MeshCollider>();
        newCollider.sharedMesh = colliderMesh;

        //Rigidbody2Dを付く
        Rigidbody2D newRigidbody = newLineObj.AddComponent<Rigidbody2D>();
        newRigidbody.sharedMaterial = DefaultPhysicsMaterial;
    }
}
