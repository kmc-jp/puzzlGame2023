using Mirror;
using NetworkRoomManagerExt;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIHpManager : MonoBehaviour
{
    public PlayerManager Player;
    public PlayerManager OtherPlayer;
    public NetworkGameplayInitializer NetworkGameplayInitializer;

    private Text _hpText;
    [SerializeField]
    private Image WinEffect;
    [SerializeField]
    private Image LoseEffect;

    private bool _gameEnded = false;
    private bool _initialized = false;

    void Start()
    {
        _hpText = GetComponent<Text>();
    }

    void Update()
    {
        int hp = 0;
        if (Player == null || OtherPlayer == null)
        {
            _networkInitialize();
        }
        else
        {
            hp = Player.HP;
        }

        if (!_initialized) { return; }
        if (_gameEnded) return;

        _hpText.text = "HP:" + hp;

        // 勝利または敗北条件のチェック
        if (Player != null && OtherPlayer != null)
        {
            if (Player.HP <= 0)
            {
                LoseEffect.enabled = true;
                StartCoroutine(EndGame());
                _gameEnded = true;
            }
            else if (OtherPlayer.HP <= 0)
            {
                WinEffect.enabled = true;
                StartCoroutine(EndGame());
                _gameEnded = true;
            }
        }
    }
    private void _networkInitialize()
    {
        if (_initialized) return;

        if (NetworkGameplayInitializer.Player1 != null && NetworkGameplayInitializer.Player2 != null)
        {
            Debug.Log("NetworkGameplayInitializer.Player1 != null && NetworkGameplayInitializer.Player2 != null");
            if(NetworkGameplayInitializer.LocalPlayerIndex == 0)
            {
                Player = NetworkGameplayInitializer.Player1;
                OtherPlayer = NetworkGameplayInitializer.Player2;
            }
            else
            {
                Player = NetworkGameplayInitializer.Player2;
                OtherPlayer = NetworkGameplayInitializer.Player1;
            }
            _initialized = true;
        }
    }



    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        if (NetworkGameplayInitializer.LocalPlayerIndex == 0)
        {
            NetworkManager.singleton.StopHost();
        }
    }
}
