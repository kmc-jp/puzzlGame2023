using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIHpManager : MonoBehaviour
{
    //プレーヤーデータ取得用
    public PlayerManager Player;

    private Text _hpText;

    // Start is called before the first frame update
    void Start()
    {
        _hpText = GetComponent<Text>();
        _networkInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        int hp = 0;
        if (Player == null)
        {
            _networkInitialize();
        }
        else
        {
            hp = Player.HP;
        }

        _hpText.text = "HP:" + hp;
    }

    private void _networkInitialize()
    {
        if (Player == null)
        {
            Player = System.Array.Find(FindObjectsOfType<PlayerManager>(), player => player.isLocalPlayer);
        }
    }
}
