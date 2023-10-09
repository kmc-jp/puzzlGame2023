using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIinkMnager : MonoBehaviour
{
    //画像格納
    public Sprite[] sprites;

    //imge読み込み用
    private Image image;

    //プレーヤーデータ取得用
    public PlayerManager Player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sprites.Length > 0);
        image = GetComponent<Image>();
        _networkInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        double inkLeftRatio = 0.0f;
        if (Player == null)
        {
            _networkInitialize();
        }
        else
        {
            //_inkLeftを取得
            inkLeftRatio = Player.InkLeft / Player.MaxInkAmount;
        }

        int imageIndex = (int)(inkLeftRatio * (sprites.Length - 1));

        image.sprite = sprites[imageIndex];
    }

    private void _networkInitialize()
    {
        if (Player == null)
        {
            Player = System.Array.Find(FindObjectsOfType<PlayerManager>(), player => player.isLocalPlayer);
        }
    }
}
