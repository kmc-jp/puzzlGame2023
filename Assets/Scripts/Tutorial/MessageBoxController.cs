using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour {
    private GameObject _frame;
    private GameObject _message;
    private GameObject _arrow;

    private void Start() {
        _frame = transform.Find("Frame").gameObject;
        _message = _frame.transform.Find("Message").gameObject;
        _arrow = _frame.transform.Find("Arrow").gameObject;

        OpenMessageBox();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MoveToNextPageOrCloseMessageBox();
        }
    }

    public void OpenMessageBox() {
        _frame.SetActive(true);

        var arrowRectTransform = _arrow.GetComponent<RectTransform>();
        arrowRectTransform.DOLocalMoveY(-168.75f + 40f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine); // 540 270 90

        if (IsMessageOnLastPage()) {
            HideArrow();
        }
    }

    public void MoveToNextPage() {
        var messageTextMeshPro = _message.GetComponent<TextMeshProUGUI>();
        messageTextMeshPro.pageToDisplay++;
    }

    public void CloseMessageBox() {
        _arrow.transform.DOKill();
        _frame.SetActive(false);
    }

    public void MoveToNextPageOrCloseMessageBox() {
        if (IsMessageOnLastPage()) {
            CloseMessageBox();
            PlayPageFeedOrMessageBoxClsoeSE();
        } else {
            MoveToNextPage();
            PlayPageFeedOrMessageBoxClsoeSE();

            if (IsMessageOnLastPage()) {
                HideArrow();
            }
        }
    }

    private bool IsMessageOnLastPage() {
        var messageTextMeshProUGUI = _message.GetComponent<TextMeshProUGUI>();

        int currentPageNum = messageTextMeshProUGUI.pageToDisplay;
        int lastPageNum = messageTextMeshProUGUI.textInfo.pageCount;
        bool isMessageOnLastPage = currentPageNum == lastPageNum;

        return isMessageOnLastPage;
    }

    private void PlayPageFeedOrMessageBoxClsoeSE() {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void HideArrow() {
        var arrowImage = _arrow.GetComponent<Image>();
        arrowImage.enabled = false;
    }
}
