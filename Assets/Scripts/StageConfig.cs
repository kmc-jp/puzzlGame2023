using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[Serializable]
public struct CharacterConfig
{
    public Sprite Icon;
    public Sprite Portrait;
    public GameObject[] AnimaPrefabs;
    public Sprite[] AnimaColorImage;
}

public class StageConfig : NetworkBehaviour
{
    // List of characters available
    public CharacterConfig[] Characters;

    // Selected character of each player
    [SyncVar]
    public readonly SyncList<int> CharacterIndex = new SyncList<int> { 0, 0 };

    public void SetPlayerCharacterIndex(int newCharacterIndex)
    {
        int playerIndex = isServer ? 0 : 1;
        CmdSetPlayerCharacterIndex(playerIndex, newCharacterIndex);
    }

    public int GetPlayerCharacterIndex(int player)
    {
        return CharacterIndex[player];
    }

    [Command(requiresAuthority = false)]
    void CmdSetPlayerCharacterIndex(int playerIndex, int characterIndex)
    {
        CharacterIndex[playerIndex] = characterIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
