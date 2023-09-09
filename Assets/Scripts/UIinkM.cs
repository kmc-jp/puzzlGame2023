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
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(sprites.Length > 0);

        //_inkLeftを取得
        double inkLeftRatio = Player.InkLeft / Player.MaxInkAmount;
        int imageIndex = (int)(inkLeftRatio * (sprites.Length - 1));

        image.sprite = sprites[imageIndex];
    }
}
