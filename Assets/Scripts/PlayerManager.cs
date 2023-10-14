using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    /*
     * アニマ関連
     */
    //インクの最大量(秒)
    [SyncVar]
    [Range(0.0f, 100.0f)]
    public double MaxInkAmount = 2.0;

    //インクの回復測度(秒/秒)
    [Range(0.0f, 100.0f)]
    public double InkRecovery = 0.5;

    //インク残量(秒)
    [SyncVar]
    private double _inkLeft;
    public double InkLeft
    {
        get { return _inkLeft; }
        set { _inkLeft = value; }
    }

    private double _inkUpdateAmount;

    /*
     * その他
     */
    //最大HP
    [SyncVar]
    [Range(1, 10)]
    public int MaxHP = 2;
    //HP計算用
    [SyncVar]
    public int HP;

    //無敵時間
    [Range(0.0f, 100.0f)]
    public float MaxGodMode = 8.0f;

    //無敵時間計算用
    private float _godModeDuration;

    //スクリプト取得用
    public StageInputController InputController;

    void OnAnimaDrawingStart(Vector3 startPositionWorld, int color)
    {
        _inkUpdateAmount = -1;
        CmdAnimaDrawingStart(startPositionWorld, color);
    }

    void OnAnimaDrawingEnd(Vector3 endPositionWorld, bool cancel)
    {
        _inkUpdateAmount = InkRecovery;
        CmdAnimaDrawingEnd(endPositionWorld, cancel);
    }

    [Command]
    void CmdAnimaDrawingStart(Vector3 startPositionWorld, int color)
    {
        _inkUpdateAmount = -1;
    }

    [Command]
    void CmdAnimaDrawingEnd(Vector3 endPositionWorld, bool cancel)
    {
        _inkUpdateAmount = InkRecovery;
    }

    public bool TakeDamage(int amount, bool enableGodMode)
    {
        if (isServer)
        {
            if (_godModeDuration <= 0.0f)
            {
                HP -= amount;

                if (enableGodMode)
                {
                    _godModeDuration = MaxGodMode;
                }
                return true;
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //HP初期化
        HP = MaxHP;

        InkLeft = MaxInkAmount;
        _inkUpdateAmount = InkRecovery;

        if (InputController == null)
        {
            InputController = FindObjectOfType<StageInputController>();
        }

        if (isLocalPlayer)
        {
            InputController.OnAnimaDrawingStart += OnAnimaDrawingStart;
            InputController.OnAnimaDrawingEnd += OnAnimaDrawingEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //無敵時間が終わったか判定
        _godModeDuration -= Time.deltaTime;

        //インク残量を減らす・回復する
        InkLeft += _inkUpdateAmount * Time.deltaTime;
        if (InkLeft < 0.0f)
        {
            InkLeft = 0.0f;
        }
        else if (InkLeft > MaxInkAmount)
        {
            InkLeft = MaxInkAmount;
        }
    }
}
