using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CharacterConfig
{
    public Sprite Icon;
    public Sprite Portrait;
    public GameObject[] AnimaPrefabs;
}

public class StageConfig : MonoBehaviour
{
    // List of characters available
    public CharacterConfig[] Characters;

    // Selected character of each player
    public int[] CharacterIndex = new int[2];

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
