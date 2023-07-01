using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIinkMnager : MonoBehaviour
{
    //画像格納
    public Sprite sprite;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
       image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            image.sprite = sprite;
        }
    }
}
