using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectPortrait : MonoBehaviour
{
    public int PlayerIndex;
    public StageConfig StageConfig;

    Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        Debug.Assert(_image != null);
    }

    // Update is called once per frame
    void Update()
    {
        _image.sprite = StageConfig.Characters[StageConfig.GetPlayerCharacterIndex(PlayerIndex)].Portrait;
    }
}
