using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StageInputController : MonoBehaviour
{
    public PlayerManager Player;

    /*
     * アニマ関連
     */
    public delegate void OnAnimaDrawingStartFunc(Vector3 startPositionWorld, int color);
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

    public int GetAnimaDrawingColor()
    {
        //TODO: Revise when the color system is developed
        if ((_state & ControllerState.DrawingColor1) == ControllerState.DrawingColor1)
        {
            return 1;
        }
        else if ((_state & ControllerState.DrawingColor2) == ControllerState.DrawingColor2)
        {
            return 2;
        }
        else if ((_state & ControllerState.DrawingColor3) == ControllerState.DrawingColor3)
        {
            return 3;
        }
        return 0;
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

        //TODO: Temporarily allow cursor interaction at all times
        _state |= ControllerState.CursorInteractionEnabled;
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
                OnAnimaDrawingStart(GetAnimaDrawingPositionWorld(), GetAnimaDrawingColor());
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

        //インク残量が0になったら、drawingを強制終了
        if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
        {
            if (Player.InkLeft <= 0.0f)
            {
                OnAnimaDrawingEnd(GetAnimaDrawingPositionWorld(), false);
                _state &= ~ControllerState.Drawing;
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