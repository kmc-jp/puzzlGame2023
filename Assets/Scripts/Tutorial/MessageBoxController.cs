using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour {
    private Image _frameImage;
    private Image _iconImage;
    private TextMeshProUGUI _messageTextMeshProUGUI;
    private Image _arrowImage;

    private void Start() {
        _frameImage = transform.Find("Frame").GetComponent<Image>();
        _iconImage = _frameImage.transform.Find("Icon").GetComponent<Image>();
        _messageTextMeshProUGUI = _frameImage.transform.Find("Message").GetComponent<TextMeshProUGUI>();
        _arrowImage = _frameImage.transform.Find("Arrow").GetComponent<Image>();
    }

    public void ShowMessageBox() {
        _frameImage.enabled = true;
        _iconImage.enabled = true;
        _messageTextMeshProUGUI.enabled = true;
        _arrowImage.enabled = true;

        _messageTextMeshProUGUI.pageToDisplay = 1;

        _arrowImage.transform.localPosition = new Vector3(420f, -168.75f, 0f);
        _arrowImage.transform.DOLocalMoveY(40f, 0.5f).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void HideMessageBox() {
        _frameImage.enabled = false;
        _iconImage.enabled = false;
        _messageTextMeshProUGUI.enabled = false;
        _arrowImage.enabled = false;

        _arrowImage.transform.DOKill();
    }

    public void TurnPageOrHideMessageBox() {
        int currentPageNum = _messageTextMeshProUGUI.pageToDisplay;
        int lastPageNum = _messageTextMeshProUGUI.textInfo.pageCount;
        bool isMessageOnLastPage = currentPageNum == lastPageNum;
        
        if (isMessageOnLastPage) {
            HideMessageBox();
        } else {
            _messageTextMeshProUGUI.pageToDisplay++;
        }
    }
}