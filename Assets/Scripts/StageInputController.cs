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
    public delegate void OnAnimaDrawingStartFunc(Vector3 startPositionWorld);
    public event OnAnimaDrawingStartFunc OnAnimaDrawingStart;

    public delegate void OnAnimaDrawingEndFunc(Vector3 endPositionWorld, bool cancel);
    public event OnAnimaDrawingEndFunc OnAnimaDrawingEnd;

    /*
     * CursorInteractable関連
     */
    public delegate void OnCursorInteractionStartFunc(Vector3 startPositionWorld);
    public event OnCursorInteractionStartFunc OnCursorInteractionStart;

    public delegate void OnCursorInteractionEndFunc(Vector3 endPositionWorld);
    public event OnCursorInteractionEndFunc OnCursorInteractionEnd;

    /*
     * ステート管理
     */
    [Flags]
    private enum ControllerState
    {
        None = 0,
        Drawing                  = 1 << 0,
        DrawingEnabled           = 1 << 1,
        DrawingColor1            = 1 << 2,
        DrawingColor2            = 1 << 3,
        DrawingColor3            = 1 << 4,
        CursorInteraction        = 1 << 5,
        CursorInteractionEnabled = 1 << 6,
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

    public Vector3 GetCursorInteractionPositionScreen()
    {
        return Input.mousePosition;
    }

    public Vector3 GetCursorInteractionPositionWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Temporarily allow drawing at all times
        _state |= ControllerState.DrawingEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Update based on possible player inputs
        /*
         * Anima
         */
        if (_isDrawingAllowed() && Input.GetMouseButtonDown(0))
        {
            if ((_state & ControllerState.Drawing) != ControllerState.Drawing)
            {
                _state |= ControllerState.Drawing;
                OnAnimaDrawingStart(GetAnimaDrawingPositionWorld());
            }
        }
        else if (!_isDrawingAllowed() || Input.GetMouseButtonUp(0))
        {
            if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
            {
                OnAnimaDrawingEnd(GetAnimaDrawingPositionWorld(), false);
                _state &= ~ControllerState.Drawing;
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

        /*
         * Cursor Interaction
         */
        if (_isCursorInteractionAllowed() && Input.GetMouseButtonDown(1))
        {
            _state |= ControllerState.CursorInteraction;
            OnCursorInteractionStart(GetCursorInteractionPositionWorld());
        }
        else if (!_isCursorInteractionAllowed() || Input.GetMouseButtonUp(1))
        {
            OnCursorInteractionEnd(GetCursorInteractionPositionWorld());
            _state &= ~ControllerState.CursorInteraction;
        }
    }

    private bool _isDrawingAllowed()
    {
        return (_state & ControllerState.DrawingEnabled) == ControllerState.DrawingEnabled;
    }

    private bool _isCursorInteractionAllowed()
    {
        return (_state & ControllerState.CursorInteractionEnabled) == ControllerState.CursorInteractionEnabled;
    }
}
