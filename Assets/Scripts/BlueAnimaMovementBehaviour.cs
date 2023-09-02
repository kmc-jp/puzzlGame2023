using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAnimaMovementBehaviour : MonoBehaviour
{

    //アタッチされたオブジェクトのLinerendere取得用
    public LineRenderer lineRenderer;
    List<Vector2> colliderPoints;
    List<Vector3> linePoints;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        colliderPoints = new List<Vector2>();
        linePoints = new List<Vector3>();

        //_addRigidBody();

        //_addEdgeCollider();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void _addEdgeCollider()
    {
        this.gameObject.AddComponent<EdgeCollider2D>();
        var col = this.GetComponent<EdgeCollider2D>();

        //描いたオブジェクトの位置を取得
        Vector3[] vecs = getLinePoint();

        foreach (Vector3 vec in vecs)
        {
            colliderPoints.Add(vec);
        }
        col.SetPoints(colliderPoints);

        col.edgeRadius = 0.25f;
    }

    void _addRigidBody()
    {
        this.gameObject.AddComponent<Rigidbody2D>();
        var rb = this.GetComponent<Rigidbody2D>();
        //質量を100にする
        rb.mass = 100;
        rb.simulated = true;
    }
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
}
