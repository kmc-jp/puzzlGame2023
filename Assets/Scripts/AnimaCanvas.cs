using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
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

    [SerializeField, Range(0.0f, 1.0f)]
    float SimplifyColliderTolerance = 0.1f;

    [SerializeField, Tooltip("Points drawn per second")]
    int DrawFrequency = 48;

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
    private float drawFrequencyTimer = 0.0f;
    private float drawFrequencyInverted;

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
        tempDrawingObject.transform.localScale = Vector3.one;

        tempDrawingLineRenderer = tempDrawingObject.AddComponent<LineRenderer>();
        tempDrawingLineRenderer.useWorldSpace = true;
        tempDrawingLineRenderer.material = DefaultLineMaterial;
        tempDrawingLineRenderer.material.color = curDrawingColor;
        tempDrawingLineRenderer.startWidth = DefaultLineWidth;
        tempDrawingLineRenderer.endWidth = DefaultLineWidth;
        tempDrawingLineRenderer.positionCount = 0;

        drawFrequencyTimer = 0.0f;
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

        drawFrequencyInverted = 1.0f / DrawFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        // Drawing
        if ((_state & AnimaCanvasState.Drawing) == AnimaCanvasState.Drawing)
        {
            if (drawFrequencyTimer <= 0.0f)
            {
                //マウスポインタのワールド座標を取得
                Vector3 mousePosition = InputController.GetAnimaDrawingPositionWorld();

                //ポインターはカンバスの中に入っているかを確認
                if (_positionInBounds(mousePosition))
                {
                    //線を描画
                    _addPositionDataToLineRenderer(mousePosition);
                }
                drawFrequencyTimer = drawFrequencyInverted;
            }
            else
            {
                drawFrequencyTimer -= Time.deltaTime;
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
        if (tempDrawingLineRenderer.positionCount == 0)
        {
            return;
        }

        //TODO: Build from a prefab?

        //空のゲームオブジェクト作成
        GameObject newLineObj = new GameObject();
        //オブジェクトにLineObjectMをアタッチ
        //newLineObj.AddComponent<LineObjectM>();
        //オブジェクトの名前をStrokeに変更
        newLineObj.name = "Stroke";

        //TODO: 親のtransformを無視できないため、設定しない
        //lineObjを自身の子要素に設定
        //newLineObj.transform.SetParent(transform);

        newLineObj.transform.localScale = Vector3.one;

        //LineRendererをMesh化して、新しいラインオブジェクトに付く
        MeshFilter newFilter = newLineObj.AddComponent<MeshFilter>();
        tempDrawingLineRenderer.BakeMesh(newFilter.mesh);

        MeshRenderer newRenderer = newLineObj.AddComponent<MeshRenderer>();
        newRenderer.material = DefaultLineMaterial;
        newRenderer.material.color = curDrawingColor;

        //コライダーを付く
        _createCollider(newLineObj);

        //Rigidbody2Dを付く
        Rigidbody2D newRigidbody = newLineObj.AddComponent<Rigidbody2D>();
        newRigidbody.sharedMaterial = DefaultPhysicsMaterial;
        //TODO: Calculate mass

        //TODO: Attach AnimaObject based on color
        //AnimaObjectを付く
        //AnimaObject newAnima = newLineObj.AddComponent<AnimaObject>();

        //TODO: Allow movement with cursor for now. Remove once anima effects are implemented.
        newLineObj.AddComponent<CursorInteractable>();
    }

    void _createCollider(GameObject lineObject)
    {
        //Note: This modifies tempDrawingLineRenderer
        tempDrawingLineRenderer.Simplify(SimplifyColliderTolerance);
        Mesh colliderMesh = new Mesh();

        tempDrawingLineRenderer.BakeMesh(colliderMesh);
        PolygonCollider2D newCollider = lineObject.AddComponent<PolygonCollider2D>();
        List<Vector3> vertices = new List<Vector3>();
        colliderMesh.GetVertices(vertices);
        int[] triangles = colliderMesh.GetTriangles(0);
        newCollider.pathCount = triangles.Length;

        for (int i = 0; i <  triangles.Length / 3; i++)
        {
            Vector2[] points = new Vector2[] {
                vertices[triangles[i * 3]],
                vertices[triangles[i * 3 + 1]],
                vertices[triangles[i * 3 + 2]]
            };
            newCollider.SetPath(i, points);
        }

        Destroy(colliderMesh);
    }
}
