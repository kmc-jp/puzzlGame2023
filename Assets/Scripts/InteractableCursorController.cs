using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableCursorController : MonoBehaviour
{
    public StageInputController InputController;

    CursorInteractable _attachedObject = null;
    int _interactableLayerMask = 0;

    public Vector3 GetCursorInteractionPositionScreen()
    {
        return InputController.GetCursorInteractionPositionScreen();
    }

    public Vector3 GetCursorInteractionPositionWorld()
    {
        return InputController.GetCursorInteractionPositionWorld();
    }

    void Start()
    {
        _interactableLayerMask |= 1;      // Default
        _interactableLayerMask |= 1 << 4; // Water

        InputController.OnCursorInteractionStart += CursorInteractionStart;
        InputController.OnCursorInteractionEnd += CursorInteractionEnd;
    }

    void CursorInteractionStart(Vector3 positionWorld)
    {
        // Search for an object under the cursor to attach
        // Only allow one attached object
        if (_attachedObject == null)
        {
            // Test for an object under the cursor
            RaycastHit2D[] results = Physics2D.RaycastAll(positionWorld, Vector2.zero, 0, _interactableLayerMask);

            foreach (RaycastHit2D i in results)
            {
                _attachedObject = i.transform.gameObject.GetComponent<CursorInteractable>();
                if (_attachedObject)
                {
                    _attachedObject.AttachToCursor(this);
                    break;
                }
            }
        }
    }

    void CursorInteractionEnd(Vector3 positionWorld)
    {
        // On mouse button up, release the attached object
        if (_attachedObject)
        {
            _attachedObject.DetachFromCursor();
            _attachedObject = null;
        }
    }

}
