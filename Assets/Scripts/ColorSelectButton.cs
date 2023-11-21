using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectButton : MonoBehaviour
{
    public Sprite UnselectedBorder;
    public Sprite SelectedBorder;
    public StageInputController InputController;
    public int ColorIndex = 0;

    private StageConfig StageConfig = null;
    private Image BorderImage;
    private Image ButtonImage;
    private bool _initialized = false;

    void OnAnimaColorChange(int newColor)
    {
        if (newColor - 1 == ColorIndex)
        {
            BorderImage.sprite = SelectedBorder;
        }
        else
        {
            BorderImage.sprite = UnselectedBorder;
        }
        if (BorderImage.sprite != null)
        {
            BorderImage.enabled = true;
        }
        else
        {
            BorderImage.enabled = false;
        }
    }

    void OnButtonClick()
    {
        InputController.SetAnimaColor(ColorIndex + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = transform.Find("ColorImage").GetComponent<Image>();
        Debug.Assert(ButtonImage != null);

        BorderImage = GetComponent<Image>();
        Debug.Assert(ButtonImage != null);
        OnAnimaColorChange(1);

        InputController = FindAnyObjectByType<StageInputController>();
        Debug.Assert(InputController != null);
        InputController.OnAnimaColorChange += OnAnimaColorChange;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);

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

        if (StageConfig.isServer)
        {
            ButtonImage.sprite = StageConfig.Characters[StageConfig.CharacterIndex[0]].AnimaColorImage[ColorIndex];
        }
        else
        {
            ButtonImage.sprite = StageConfig.Characters[StageConfig.CharacterIndex[1]].AnimaColorImage[ColorIndex];
        }

        _initialized = true;
    }
}
