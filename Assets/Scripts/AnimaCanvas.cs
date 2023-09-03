using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimaCanvas : MonoBehaviour
{
    [SerializeField] StageInputController InputController;

    [SerializeField] PlayerManager Player;

    //標準の線の材質
    [SerializeField] Material DefaultLineMaterial;

    //標準の線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float DefaultLineWidth;

    [SerializeField] PhysicsMaterial2D DefaultPhysicsMaterial;

    [SerializeField, Range(0.0f, 1.0f)]
    float SimplifyColliderTolerance = 0.1f;

    [SerializeField, Tooltip("Points drawn per second")]
    int DrawFrequency = 48;

    [SerializeField] GameObject[] AnimaColorPrefabs;

    [Flags]
    private enum AnimaCanvasState
    {
        None = 0,
        Drawing = 1
    }

    private AnimaCanvasState _state = AnimaCanvasState.None;

    private GameObject tempDrawingObject = null;
    private LineRenderer tempDrawingLineRenderer = null;
    private int curDrawingColorIndex;
    private float drawFrequencyTimer = 0.0f;
    private float drawFrequencyInverted;
    private float drawTimer;

    public void BeginDraw(Vector3 startPosition, int color)
    {
        // StageInputController color is 1-based
        color -= 1;

        Debug.Assert((_state & AnimaCanvasState.Drawing) != AnimaCanvasState.Drawing);
        Debug.Assert(color < AnimaColorPrefabs.Length);
        Debug.Assert(AnimaColorPrefabs[color].GetComponent<AnimaObject>() != null);

        _state |= AnimaCanvasState.Drawing;

        curDrawingColorIndex = color;

        tempDrawingObject = new GameObject();
        tempDrawingObject.name = "TempStroke";
        tempDrawingObject.transform.parent = transform;
        tempDrawingObject.transform.position = startPosition;
        tempDrawingObject.transform.localScale = Vector3.one;

        tempDrawingLineRenderer = tempDrawingObject.AddComponent<LineRenderer>();
        tempDrawingLineRenderer.useWorldSpace = true;
        tempDrawingLineRenderer.material = DefaultLineMaterial;
        tempDrawingLineRenderer.material.color = AnimaColorPrefabs[curDrawingColorIndex].GetComponent<AnimaObject>().GetColor();
        tempDrawingLineRenderer.startWidth = DefaultLineWidth;
        tempDrawingLineRenderer.endWidth = DefaultLineWidth;
        tempDrawingLineRenderer.positionCount = 0;

        drawFrequencyTimer = 0.0f;
        drawTimer = 0.0f;
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

            drawTimer += Time.deltaTime;
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
        GameObject newLineObj = Instantiate(AnimaColorPrefabs[curDrawingColorIndex]);
        //オブジェクトにLineObjectMをアタッチ
        //newLineObj.GetOrAddComponent<LineObjectM>();
        //オブジェクトの名前をStrokeに変更
        newLineObj.name = "Stroke";

        //TODO: 親のtransformを無視できないため、設定しない
        //lineObjを自身の子要素に設定
        //newLineObj.transform.SetParent(transform);

        newLineObj.transform.localScale = Vector3.one;

        //LineRendererをMesh化して、新しいラインオブジェクトに付く
        //TODO: GetOrAddComponent doesn't work?
        MeshFilter newFilter = newLineObj.GetOrAddComponent<MeshFilter>();
        tempDrawingLineRenderer.BakeMesh(newFilter.mesh);

        MeshRenderer newRenderer = newLineObj.GetOrAddComponent<MeshRenderer>();
        newRenderer.material = DefaultLineMaterial;
        newRenderer.material.color = AnimaColorPrefabs[curDrawingColorIndex].GetComponent<AnimaObject>().GetColor();

        //コライダーを付く
        _createCollider(newLineObj);

        //Rigidbody2Dを付く
        Rigidbody2D newRigidbody = newLineObj.GetOrAddComponent<Rigidbody2D>();
        newRigidbody.sharedMaterial = DefaultPhysicsMaterial;
        //TODO: Calculate mass?

        //AnimaObjectを付く
        AnimaObject animaComponent = newLineObj.GetComponent<AnimaObject>();
        animaComponent.InkUsed = drawTimer;

        //TODO: Allow movement with cursor for now. Remove once anima effects are implemented.
        newLineObj.GetOrAddComponent<CursorInteractable>();
    }

    void _createCollider(GameObject lineObject)
    {
        //Note: This modifies tempDrawingLineRenderer
        tempDrawingLineRenderer.Simplify(SimplifyColliderTolerance);

        // NOTE:
        // BakeMesh takes a snapshot of the mesh that LineRenderer will use for rendering at the time it is called.
        // In this project, since the camera's projection type is orthographic, snapshots are independent of the camera's position whenever they are taken.
        var mesh = new Mesh();
        tempDrawingLineRenderer.BakeMesh(mesh);

        var vertices = new List<Vector3>();
        mesh.GetVertices(vertices);

        var triangles = mesh.GetTriangles(0);

        // NOTE:
        // A path refers to a single closed path that constitutes a polygon.
        // In this implementation, one path is equated with one triangle, and LineRenderer's mesh information is diverted to the collider.

        PolygonCollider2D polygonCollider2d = lineObject.GetOrAddComponent<PolygonCollider2D>();

        // NOTE:
        // pathCount must be explicitly assigned or it will assert an IndexOutOfRange exception.
        polygonCollider2d.pathCount = triangles.Length;

        for (int triangleIndex = 0; triangleIndex < triangles.Length / 3; triangleIndex++)
        {
            var firstPoint = vertices[triangles[triangleIndex * 3]];
            var secondPoint = vertices[triangles[triangleIndex * 3 + 1]];
            var thirdPoint = vertices[triangles[triangleIndex * 3 + 2]];
            var points = new Vector2[] { firstPoint, secondPoint, thirdPoint };

            polygonCollider2d.SetPath(triangleIndex, points);
        }

        Destroy(mesh);
    }

    void _createAnimaColor(GameObject lineObject)
    {
        
    }
}
