using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
    //プレイヤーナンバー
    public string playerNumber;

    //HP表示用
    public Text HPtext;
    //最大HP
    [Range(1f, 10f)]
    public int MaxHP = 2;
    //HP計算用
    public int HP;

    //無敵時間
    [Range(0.0f, 5.0f)]
    public double maxGodMode;
    //無敵時間計算用
    public double godModeCount;
    //無敵時間フラグ
    public int godModeFlag = 0;

    //スクリプト取得用
    public GameObject DrawingCampas;
    GM GM;

    // Start is called before the first frame update
    void Start()
    {
        //HP初期化
        HP = MaxHP;
        //HP表示初期化
        HPtext.text = "HP:" + HP;

        //プレイヤー番号初期化
        playerNumber = "Player1";

        //プレイヤー名のタグを追加
       
    }

    // Update is called once per frame
    void Update()
    {
        //他スクリプトの情報を受け取る変数
        double goalInFlag;

        //他スクリプトから情報を取得
        DrawingCampas = GameObject.Find("DrawingCanvas");
        GM = DrawingCampas.GetComponent<GM>();
        goalInFlag = GM.goalInFlag;

        //オブジェクトがゴールエリアにあって無敵時間外なら
        if (goalInFlag == 1 && godModeFlag == 0)
        {
            //HP更新
            HP--;
            //HP表示更新
            HPtext.text = "HP:" + HP;
            //無敵時間開始
            godModeCount = 0;
            godModeFlag = 1;
        }

        //無敵時間の経過時間を計測
        //無敵時間開始時にリセットされる
        godModeCount += Time.deltaTime;

        //無敵時間が終わったか判定
        if (godModeCount >= maxGodMode)
        {
            godModeFlag = 0;
        }
    }
}
