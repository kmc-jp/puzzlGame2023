using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInputController : MonoBehaviour
{
    public PlayerManager Player;

    /*
     * アニマ関連
     */
    public delegate void OnAnimaDrawingStartFunc(Vector3 startPosition);
    public event OnAnimaDrawingStartFunc OnAnimaDrawingStart;

    public delegate void OnAnimaDrawingEndFunc(Vector3 endPosition, bool cancel);
    public event OnAnimaDrawingEndFunc OnAnimaDrawingEnd;

    /*
     * ステート管理
     */
    [Flags]
    private enum ControllerState
    {
        None = 0,
        Drawing = 1,
        PreventDrawing = 2,
        DrawingColor1 = 4,
        DrawingColor2 = 8,
        DrawingColor3 = 16,
        CursorInteraction = 32
    }

    private ControllerState _state = ControllerState.None;

    public Vector3 GetAnimaDrawingPositionWorld()
    {
        //マウスポインタがあるスクリーン座標を取得
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);

        //スクリーン座標をワールド座標に変換
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return worldPosition;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Update based on possible player inputs
        if (Input.GetMouseButtonDown(0))
        {
            if ((_state & ControllerState.Drawing) != ControllerState.Drawing)
            {
                OnAnimaDrawingStart(GetAnimaDrawingPositionWorld());
                _state |= ControllerState.Drawing;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
            {
                _state &= ~ControllerState.Drawing;
                OnAnimaDrawingEnd(GetAnimaDrawingPositionWorld(), false);
            }
        }

        //インク残量を減らす・回復する
        //TODO: Move this logic into a proper player state management class
        if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
        {
            Player.InkLeft -= Time.deltaTime;
            if (Player.InkLeft <= 0.0f)
            {
                Player.InkLeft = 0.0f;
                _state &= ~ControllerState.Drawing;
                OnAnimaDrawingEnd(GetAnimaDrawingPositionWorld(), false);
            }
        }
        else
        {
            if (Player.InkLeft < Player.MaxInkAmount)
            {
                Player.InkLeft += Time.deltaTime * Player.InkRecovery;
            }
        }

        //TODO: Move cursor interactable controller to this class
    }
}
