using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableCursorController : MonoBehaviour
{
    [Tooltip("The mouse button that will correspond to this cursor behavior. 0 = Left, 1 = Right, 2 = Middle"),
     Range(0, 2)]
    public int MouseButtonForInteraction = 1;

    CursorInteractable _attachedObject = null;
    int _interactableLayerMask = 0;

    void Start()
    {
        _interactableLayerMask |= 1;      // Default
        _interactableLayerMask |= 1 << 4; // Water
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(MouseButtonForInteraction))
        {
            // On mouse button down, search for an object under the cursor to attach
            // Only allow one attached object
            if (_attachedObject == null)
            {
                // Test for an object under the cursor
                Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] results = Physics2D.RaycastAll(cameraPos, Vector2.zero, 0, _interactableLayerMask);

                foreach (RaycastHit2D i in results)
                {
                    _attachedObject = i.transform.gameObject.GetComponent<CursorInteractable>();
                    if (_attachedObject)
                    {
                        _attachedObject.AttachToCursor();
                        break;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(MouseButtonForInteraction))
        {
            // On mouse button up, release the attached object
            if (_attachedObject)
            {
                _attachedObject.DetachFromCursor();
                _attachedObject = null;
            }
        }

    }
}
