using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectButton : MonoBehaviour
{
    public Sprite UnselectedImage;
    public Sprite SelectedP1Image;
    public Sprite SelectedP2Image;
    public Sprite SelectedP1P2Image;

    public int CharacterIndex = 0;
    public StageConfig StageConfig;

    Image _image;
    Sprite[] _selectedImages;

    void OnClick()
    {
        StageConfig.SetPlayerCharacterIndex(CharacterIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        Debug.Assert(_image != null);

        _selectedImages = new Sprite[] { UnselectedImage, SelectedP1Image, SelectedP2Image, SelectedP1P2Image };

        GetComponent<Button>().onClick.AddListener(OnClick);

        Image childImage = transform.Find("CharacterIcon").GetComponent<Image>();
        childImage.sprite = StageConfig.Characters[CharacterIndex].Icon;
    }

    // Update is called once per frame
    void Update()
    {
        int spriteIndex = 0;

        if (StageConfig.GetPlayerCharacterIndex(0) == CharacterIndex)
        {
            spriteIndex += 1;
        }
        if (StageConfig.GetPlayerCharacterIndex(1) == CharacterIndex)
        {
            spriteIndex += 2;
        }

        _image.sprite = _selectedImages[spriteIndex];
    }
}
