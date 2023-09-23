using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class CursorInteractable : MonoBehaviour
{
    [Tooltip("How closely the object follows the cursor. 1.0 snaps to the cursor each frame"),
     Range(0.0f, 1.0f)]
    public float CursorTrackingCoefficient = 1.0f;

    [Tooltip("Multiplier applied to object velocity when releasing the object"),
     Range(0.0f, 100.0f)]
    public float ThrowStrength = 1.0f;

    [Tooltip("How quickly the object accelerates with cursor movement before being thrown. A lower value requires the object to be dragged for longer while a higher value allows the object to be thrown with a quick flick. Maximum value is based on fixed timestep."),
     Range(0.0f, 50.0f)]
    public float TrackingSmoothness = 20.0f;

    // Reference to the controller managing cursor interactions
    InteractableCursorController _interactionController;

    // Reference to the objects RigidBody2D component
    // This is also used to determine if the object is currently tracking the cursor or not
    Rigidbody2D _rigidBody2d = null;
    Vector3 _anchorPos = Vector3.zero;

    // Fixed values based on project settings
    float _fixedFps;
    float _effectiveTrackingSmoothness;

    Vector2 _previousPos;
    Vector2 _throwVelocity = Vector2.zero;
    float _throwSpeed = 0.0f;

    public void AttachToCursor(InteractableCursorController controller)
    {
        _interactionController = controller;
        OnBeginAnchor();
    }

    public void DetachFromCursor()
    {
        OnEndAnchor();
    }

    void Start()
    {
        _fixedFps = 1.0f / Time.fixedDeltaTime;
        _effectiveTrackingSmoothness = TrackingSmoothness * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        // Use m_rigidBody2d to determine if the object is currently tracking the cursor
        if (_rigidBody2d != null)
        {
            Vector3 objectPosWorld = transform.position + _anchorPos;
            Vector3 cursorPosWorld = _interactionController.GetCursorInteractionPositionWorld();
            Vector2 previousTravelDir = (Vector2)objectPosWorld - _previousPos;
            transform.position = objectPosWorld * (1.0f - CursorTrackingCoefficient) + cursorPosWorld * CursorTrackingCoefficient - _anchorPos;

            float previousMoveDistance = previousTravelDir.magnitude;
            _throwVelocity = _throwVelocity * (1.0f - _effectiveTrackingSmoothness) + previousTravelDir * _effectiveTrackingSmoothness;
            _throwSpeed = _throwSpeed * (1.0f - _effectiveTrackingSmoothness) + previousMoveDistance * _effectiveTrackingSmoothness;
            if (Mathf.Approximately(_throwSpeed, 0.0f))
            {
                _throwSpeed = 0.0f;
            }

            _previousPos.Set(objectPosWorld.x, objectPosWorld.y);
        }
    }

    void OnBeginAnchor()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();

        // Temporarily freeze physics calculations
        _rigidBody2d.isKinematic = true;

        // Calculate the anchor position from world coordinates
        Vector3 interactionPosScreen = _interactionController.GetCursorInteractionPositionScreen();
        _anchorPos = Camera.main.ScreenToWorldPoint(new Vector3(interactionPosScreen.x, interactionPosScreen.y, 0.0f)) - transform.position;

        // Reset the object's velocity
        _rigidBody2d.velocity = Vector2.zero;
        _rigidBody2d.angularVelocity = 0.0f;
        _throwVelocity = Vector2.zero;
        _throwSpeed = 0.0f;
        _previousPos = transform.position + _anchorPos;
    }

    void OnEndAnchor()
    {
        if (_rigidBody2d)
        {
            // Adjust the current throwing velocity by the throw strength
            _rigidBody2d.velocity = _throwVelocity.normalized * _throwSpeed * _fixedFps * ThrowStrength;

            // Re-enable physics calculations
            _rigidBody2d.isKinematic = false;

            _rigidBody2d = null;
        }
    }
}
