using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
internal sealed class PlayerCameraScrollController : MonoBehaviour {
    [SerializeField] private Rect _scrollableArea = new Rect(-20f, -5f, 40f, 20f);
    [SerializeField] private float _moveSpeedMaltiply = 0.1f;
    [SerializeField] private Vector2 _ownHalfFocusPoint = new Vector2(-10f, 0f);
    [SerializeField] private Vector2 _oppositionHalfFocusPoint = new Vector2(10f, 0f);
    [SerializeField] private float _focusSpeedMultiply = 1f;

    private Camera _playerCamera;

    private void Awake() {
        _playerCamera = GetComponent<Camera>();
    }

    private void Update() {
        MoveWithinScrollableArea();
        FocusOwnOrOppositionHalf();
    }

    private void MoveWithinScrollableArea() {
        float horizontalDelta = Input.GetAxis("PlayerCameraHorizontal");
        float verticalDelta = Input.GetAxis("PlayerCameraVertical");
        var scrollDelta = new Vector2(horizontalDelta, verticalDelta);
        scrollDelta *= _moveSpeedMaltiply;
        transform.Translate(scrollDelta);

        var cameraLeftCenterPoint = _playerCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f));
        var cameraRightCenterPoint = _playerCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0f));
        var cameraBottomCenterPoint = _playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f));
        var cameraTopCenterPoint = _playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));

        float correctionX = 0f;
        float correctionY = 0f;
        if (cameraLeftCenterPoint.x < _scrollableArea.xMin) {
            correctionX = _scrollableArea.xMin - cameraLeftCenterPoint.x;
        } else if (cameraRightCenterPoint.x > _scrollableArea.xMax) {
            correctionX = _scrollableArea.xMax - cameraRightCenterPoint.x;
        }
        if (cameraBottomCenterPoint.y < _scrollableArea.yMin) {
            correctionY = _scrollableArea.yMin - cameraBottomCenterPoint.y;
        } else if (cameraTopCenterPoint.y > _scrollableArea.yMax) {
            correctionY = _scrollableArea.yMax - cameraTopCenterPoint.y;
        }

        var correctionDelta = new Vector2(correctionX, correctionY);
        transform.Translate(correctionDelta);
    }

    private void FocusOwnOrOppositionHalf() {
        if (Input.GetButton("PlayerCameraFocusOwnHalf")) {
            transform.position = Vector2.MoveTowards(transform.position, _ownHalfFocusPoint, _focusSpeedMultiply);
        }
        if (Input.GetButton("PlayerCameraFocusOppositionHalf")) {
            transform.position = Vector2.MoveTowards(transform.position, _oppositionHalfFocusPoint, _focusSpeedMultiply);
        }
    }
}