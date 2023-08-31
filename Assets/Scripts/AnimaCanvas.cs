using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaCanvas : MonoBehaviour
{
    [SerializeField] StageInputController InputController;

    //標準の線の材質
    [SerializeField] Material DefaultLineMaterial;

    //標準の線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField] float DefaultLineWidth;

    //プレイヤーが描画する線の色
    [SerializeField] Color DefaultLineColor;

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

    public void BeginDraw(Vector3 startPosition)
    {
        Debug.Assert((_state & AnimaCanvasState.Drawing) != AnimaCanvasState.Drawing);

        _state |= AnimaCanvasState.Drawing;

        tempDrawingObject = new GameObject();
        tempDrawingObject.name = "TempStroke";
        tempDrawingObject.transform.parent = transform;
        tempDrawingObject.transform.position = startPosition;

        tempDrawingLineRenderer = tempDrawingObject.AddComponent<LineRenderer>();
        tempDrawingLineRenderer.useWorldSpace = true;
        tempDrawingLineRenderer.material = DefaultLineMaterial;
        tempDrawingLineRenderer.material.color = DefaultLineColor;
        tempDrawingLineRenderer.startWidth = DefaultLineWidth;
        tempDrawingLineRenderer.endWidth = DefaultLineWidth;
    }

    public void EndDraw(Vector3 endPosition, bool cancel)
    {
        Debug.Assert((_state & AnimaCanvasState.Drawing) == AnimaCanvasState.Drawing);

        _state &= ~AnimaCanvasState.Drawing;

        if (!cancel)
        {
            // Create the Anima object from the LineRenderer
            //TODO:
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

            //線を描画
            _addPositionDataToLineRenderer(mousePosition);
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
}
