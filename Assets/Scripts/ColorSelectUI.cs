using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ColorSelectUI : MonoBehaviour
{
    public GameObject ColorSelectButtonPrefab;

    private StageConfig StageConfig = null;
    private bool _initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        NetworkInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        NetworkInitialize();
    }

    void NetworkInitialize()
    {
        if (_initialized)
        {
            return;
        }

        StageConfig = FindObjectOfType<StageConfig>();
        if (StageConfig == null)
        {
            return;
        }

        Sprite[] localAnimaColors;
        if (StageConfig.isServer)
        {
            localAnimaColors = StageConfig.Characters[StageConfig.CharacterIndex[0]].AnimaColorImage;
        }
        else
        {
            localAnimaColors = StageConfig.Characters[StageConfig.CharacterIndex[1]].AnimaColorImage;
        }
        for (int i = 0; i < localAnimaColors.Length; ++i)
        {
            GameObject newButton = Instantiate(ColorSelectButtonPrefab, transform);
            newButton.GetComponent<ColorSelectButton>().ColorIndex = i;
        }

        _initialized = true;
    }
}
