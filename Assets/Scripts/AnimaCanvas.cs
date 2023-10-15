using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimaCanvas : NetworkBehaviour
{
    [SerializeField] StageInputController InputController;


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

    private GameObject _tempDrawingObject = null;
    private LineRenderer _tempDrawingLineRenderer = null;
    private int _curDrawingColorIndex;
    private float _drawFrequencyTimer = 0.0f;
    private float _drawFrequencyInverted;
    private float _drawTimer;
    private PlayerManager _localPlayer = null;

    public void BeginDraw(Vector3 startPosition, int color)
    {
        // StageInputController color is 1-based
        color -= 1;

        Debug.Assert((_state & AnimaCanvasState.Drawing) != AnimaCanvasState.Drawing);
        Debug.Assert(color < AnimaColorPrefabs.Length);
        Debug.Assert(AnimaColorPrefabs[color].GetComponent<AnimaObject>() != null);

        _state |= AnimaCanvasState.Drawing;

        _curDrawingColorIndex = color;

        _tempDrawingObject = new GameObject();
        _tempDrawingObject.name = "TempStroke";
        _tempDrawingObject.transform.parent = transform;
        _tempDrawingObject.transform.position = startPosition;
        _tempDrawingObject.transform.localScale = Vector3.one;

        _tempDrawingLineRenderer = _tempDrawingObject.AddComponent<LineRenderer>();
        _tempDrawingLineRenderer.useWorldSpace = true;
        _tempDrawingLineRenderer.material = DefaultLineMaterial;
        _tempDrawingLineRenderer.material.color = AnimaColorPrefabs[_curDrawingColorIndex].GetComponent<AnimaObject>().GetColor();
        _tempDrawingLineRenderer.startWidth = DefaultLineWidth;
        _tempDrawingLineRenderer.endWidth = DefaultLineWidth;
        _tempDrawingLineRenderer.positionCount = 0;

        _drawFrequencyTimer = 0.0f;
        _drawTimer = 0.0f;
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
            if (_localPlayer && AnimaColorPrefabs[_curDrawingColorIndex].GetComponent<NetworkIdentity>())
            {
                Vector3[] positions = new Vector3[_tempDrawingLineRenderer.positionCount];
                _tempDrawingLineRenderer.GetPositions(positions);
                CmdSpawnNetworkedAnima(
                    _localPlayer.GetComponent<NetworkIdentity>().netId,
                    AnimaColorPrefabs[_curDrawingColorIndex].GetComponent<NetworkIdentity>().assetId,
                    positions);
            }
            else
            {
                //空のゲームオブジェクト作成
                GameObject newObj = Instantiate(AnimaColorPrefabs[_curDrawingColorIndex]);
                _createAnimaObject(newObj, _tempDrawingLineRenderer);
            }
        }

        GameObject.Destroy(_tempDrawingObject);
        _tempDrawingLineRenderer = null;
        _tempDrawingObject = null;
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnNetworkedAnima(uint ownerNetworkId, uint animaTemplatePrefabId, Vector3[] points)
    {
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            // Find the owning player
            NetworkIdentity ownerIdentity;
            if (!NetworkServer.spawned.TryGetValue(ownerNetworkId, out ownerIdentity)) {
                Debug.LogErrorFormat("[SpawnNetworkedAnima] Unable to find player for ID {0}", ownerNetworkId);
                return;
            }

            // Find the prefab
            GameObject prefab = networkManager.spawnPrefabs.Find(prefab => prefab.GetComponent<NetworkIdentity>()?.assetId == animaTemplatePrefabId);
            if (!prefab)
            {
                Debug.LogErrorFormat("[SpawnNetworkedAnima] Unable to find prefab for ID {0}", animaTemplatePrefabId);
                return;
            }

            // Instantiate the object on the server
            GameObject newObject = Instantiate(prefab);
            Debug.Assert(newObject);

            // Spawn the new object on all clients
            NetworkServer.Spawn(newObject, ownerIdentity.gameObject);
            newObject.GetComponent<AnimaCollisionDetector>().SetOwnerNetworkId(ownerNetworkId);

            // Initialize the object on all clients
            RpcInitializeNetworkedAnima(newObject, points);
        }
    }

    [ClientRpc]
    void RpcInitializeNetworkedAnima(GameObject obj, Vector3[] points)
    {
        LineRenderer lineRenderer = obj.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = DefaultLineWidth;
        lineRenderer.endWidth = DefaultLineWidth;
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        _createAnimaObject(obj, lineRenderer);

        Destroy(lineRenderer);
    }

    // Start is called before the first frame update
    void Start()
    {
        InputController.OnAnimaDrawingStart += BeginDraw;
        InputController.OnAnimaDrawingEnd += EndDraw;

        _drawFrequencyInverted = 1.0f / DrawFrequency;

        _networkInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Disallow drawing until networked objects are initialized
        if (_localPlayer == null)
        {
            _networkInitialize();
            return;
        }

        // Drawing
        if ((_state & AnimaCanvasState.Drawing) == AnimaCanvasState.Drawing)
        {
            if (_drawFrequencyTimer <= 0.0f)
            {
                //マウスポインタのワールド座標を取得
                Vector3 mousePosition = InputController.GetAnimaDrawingPositionWorld();

                //ポインターはカンバスの中に入っているかを確認
                if (_positionInBounds(mousePosition))
                {
                    //線を描画
                    _addPositionDataToLineRenderer(mousePosition);
                }
                _drawFrequencyTimer = _drawFrequencyInverted;
            }
            else
            {
                _drawFrequencyTimer -= Time.deltaTime;
            }

            _drawTimer += Time.deltaTime;
        }
    }

    void _addPositionDataToLineRenderer(Vector3 worldPosition)
    {
        worldPosition.z = 0.0f;

        //lineRendererの線と線をつなぐ点の数を更新
        _tempDrawingLineRenderer.positionCount += 1;

        //lineRendererを更新
        _tempDrawingLineRenderer.SetPosition(_tempDrawingLineRenderer.positionCount - 1, worldPosition);

        //あとから描いた線が上に来るように調整
        _tempDrawingLineRenderer.sortingOrder = 1;
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

    GameObject _createAnimaObject(GameObject newLineObj, LineRenderer lineRenderer)
    {
        if (lineRenderer.positionCount == 0)
        {
            return null;
        }

        //オブジェクトにLineObjectMをアタッチ
        //newLineObj.GetOrAddComponent<LineObjectM>();
        //オブジェクトの名前をStrokeに変更
        newLineObj.name = "Stroke";
        newLineObj.transform.localScale = Vector3.one;

        //LineRendererをMesh化して、新しいラインオブジェクトに付く
        //TODO: GetOrAddComponent doesn't work?
        MeshFilter newFilter = newLineObj.GetOrAddComponent<MeshFilter>();
        lineRenderer.BakeMesh(newFilter.mesh);

        MeshRenderer newRenderer = newLineObj.GetOrAddComponent<MeshRenderer>();
        newRenderer.material = DefaultLineMaterial;
        newRenderer.material.color = newLineObj.GetComponent<AnimaObject>().GetColor();

        //コライダーを付く
        _createCollider(lineRenderer, newLineObj);

        //Rigidbody2Dを付く
        Rigidbody2D newRigidbody = newLineObj.GetOrAddComponent<Rigidbody2D>();
        newRigidbody.sharedMaterial = DefaultPhysicsMaterial;
        //TODO: Calculate mass?

        //AnimaObjectを付く
        AnimaObject animaComponent = newLineObj.GetComponent<AnimaObject>();
        animaComponent.InkUsed = _drawTimer;

        //TODO: Allow movement with cursor for now. Remove once anima effects are implemented.
        newLineObj.GetOrAddComponent<CursorInteractable>();

        return newLineObj;
    }

    void _createCollider(LineRenderer lineRenderer, GameObject lineObject)
    {
        //Note: This modifies tempDrawingLineRenderer
        lineRenderer.Simplify(SimplifyColliderTolerance);

        // NOTE:
        // BakeMesh takes a snapshot of the mesh that LineRenderer will use for rendering at the time it is called.
        // In this project, since the camera's projection type is orthographic, snapshots are independent of the camera's position whenever they are taken.
        var mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);

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

    private void _networkInitialize()
    {
        if (_localPlayer == null)
        {
            var found = FindObjectsOfType<PlayerManager>();
            _localPlayer = System.Array.Find(FindObjectsOfType<PlayerManager>(), player => player.isLocalPlayer);
        }
    }
}
