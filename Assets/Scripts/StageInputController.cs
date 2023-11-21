using Mirror;
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

    public delegate void OnAnimaColorChangeFunc(int newColor);
    public event OnAnimaColorChangeFunc OnAnimaColorChange;

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

        // Debug colors
        DrawingColor4            = 1 << 29,
        DrawingColor5            = 1 << 30,
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

        if (Debug.isDebugBuild)
        {
            if ((_state & ControllerState.DrawingColor4) == ControllerState.DrawingColor4)
            {
                return 4;
            }
            else if ((_state & ControllerState.DrawingColor5) == ControllerState.DrawingColor5)
            {
                return 5;
            }
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

    public void SetAnimaColor(int newAnimaColor)
    {
        _setAnimaColor(newAnimaColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Default color
        _state |= ControllerState.DrawingColor1;

        //TODO: Temporarily allow drawing at all times
        _state |= ControllerState.DrawingEnabled;

        //TODO: Temporarily allow cursor interaction at all times
        _state |= ControllerState.CursorInteractionEnabled;

        _networkInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable input until required network objects are finished loading
        if (Player == null)
        {
            _networkInitialize();
            return;
        }

        /*
         * Anima
         */
        if (_isDrawingAllowed() && Input.GetMouseButtonDown(0))
        {
            if ((_state & ControllerState.Drawing) != ControllerState.Drawing)
            {
                _state |= ControllerState.Drawing;
                OnAnimaDrawingStart?.Invoke(GetAnimaDrawingPositionWorld(), GetAnimaDrawingColor());
            }
        }
        else if (!_isDrawingAllowed() || Input.GetMouseButtonUp(0))
        {
            if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
            {
                OnAnimaDrawingEnd?.Invoke(GetAnimaDrawingPositionWorld(), false);
                _state &= ~ControllerState.Drawing;
            }
        }

        //インク残量が0になったら、drawingを強制終了
        if ((_state & ControllerState.Drawing) == ControllerState.Drawing)
        {
            if (Player.InkLeft <= 0.0f)
            {
                OnAnimaDrawingEnd?.Invoke(GetAnimaDrawingPositionWorld(), false);
                _state &= ~ControllerState.Drawing;
            }
        }

        //アニマの色を切り替え
        if (_isDrawingAllowed())
        {
            if (Input.GetButtonDown("Anima1")) {
                _setAnimaColor(1);
            }
            else if (Input.GetButtonDown("Anima2"))
            {
                _setAnimaColor(2);
            }
            else if (Input.GetButtonDown("Anima3"))
            {
                _setAnimaColor(3);
            }

            // Allow switching up to 5 for debug
            if (Debug.isDebugBuild)
            {
                if (Input.GetButtonDown("Anima4"))
                {
                    _setAnimaColor(4);
                }
                else if (Input.GetButtonDown("Anima5"))
                {
                    _setAnimaColor(5);
                }
            }
        }

        /*
         * Cursor Interaction
         */
        if (_isCursorInteractionAllowed() && Input.GetMouseButtonDown(1))
        {
            _state |= ControllerState.CursorInteraction;
            OnCursorInteractionStart?.Invoke(GetCursorInteractionPositionWorld());
        }
        else if (!_isCursorInteractionAllowed() || Input.GetMouseButtonUp(1))
        {
            OnCursorInteractionEnd?.Invoke(GetCursorInteractionPositionWorld());
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

    private void _setAnimaColor(int color)
    {
        ControllerState allColors =
            ControllerState.DrawingColor1 |
            ControllerState.DrawingColor2 |
            ControllerState.DrawingColor3 |
            ControllerState.DrawingColor4 |
            ControllerState.DrawingColor5;

        _state &= ~allColors;
        switch (color)
        {
            case 1:
                _state |= ControllerState.DrawingColor1;
                break;
            case 2:
                _state |= ControllerState.DrawingColor2;
                break;
            case 3:
                _state |= ControllerState.DrawingColor3;
                break;
            case 4:
                _state |= ControllerState.DrawingColor4;
                break;
            case 5:
                _state |= ControllerState.DrawingColor5;
                break;
            default:
                Debug.LogError(string.Format("Invalid Anima color: {0}", color));
                break;
        }

        OnAnimaColorChange?.Invoke(color);
    }

    private void _networkInitialize()
    {
        if (Player == null)
        {
            var found = FindObjectsOfType<PlayerManager>();
            Player = System.Array.Find(FindObjectsOfType<PlayerManager>(), player => player.isLocalPlayer);
        }
    }
}
