using UnityEngine;
using UnityEngine.UI;

public class HighlighterController : MonoBehaviour {
    private Image _focusImage;
    private Image _fadeImage;

    private void Start() {
        _focusImage = transform.Find("Focus").GetComponent<Image>();
        _fadeImage = _focusImage.transform.Find("Fade").GetComponent<Image>();
    }

    public void ShowHighlighter() {
        _focusImage.enabled = true;
        _fadeImage.enabled = true;
    }

    public void HideHighlighter() {
        _focusImage.enabled = false;
        _fadeImage.enabled = false;
    }
}