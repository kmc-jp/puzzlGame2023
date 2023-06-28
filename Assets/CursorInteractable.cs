using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CursorInteractable : MonoBehaviour
{
    [Tooltip("How closely the object follows the cursor. 1.0 snaps to the cursor each frame"),
     Range(0.0f, 1.0f)]
    public float CursorTrackingCoefficient = 1.0f;

    [Tooltip("Multiplier applied to object velocity when releasing the object"),
     Range(0.0f, 1000.0f)]
    public float ThrowStrength = 30.0f;

    [Tooltip("How quickly the object accelerates with cursor movement before being thrown. A lower value requires the object to be dragged for longer while a higher value allows the object to be thrown with a quick flick."),
     Range(0.0f, 1.0f)]
    public float TrackingSmoothness = 0.75f;

    // Reference to the objects RigidBody2D component
    // This is also used to determine if the object is currently tracking the cursor or not
    Rigidbody2D m_rigidBody2d = null;
    Vector3 m_anchorPos = Vector3.zero;

    void Start()
    {
    }

    void Update()
    {
        //TODO: hack, calculate which object is under the cursor
        if (Input.GetMouseButtonDown(1))
        {
            OnBeginAnchor(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnEndAnchor();
        }
    }

    void FixedUpdate()
    {
        // Use m_rigidBody2d to determine if the object is currently tracking the cursor
        if (m_rigidBody2d != null)
        {
            Vector3 objectPosWorld = transform.position + m_anchorPos;
            Vector3 cursorPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.MoveTowards(objectPosWorld, cursorPosWorld, (cursorPosWorld - objectPosWorld).magnitude * CursorTrackingCoefficient) - m_anchorPos;

            m_rigidBody2d.velocity = m_rigidBody2d.velocity * (1.0f - TrackingSmoothness) + new Vector2(cursorPosWorld.x - objectPosWorld.x, cursorPosWorld.y - objectPosWorld.y) * TrackingSmoothness;
        }
    }

    void OnBeginAnchor(float cursorX, float cursorY)
    {
        m_rigidBody2d = GetComponent<Rigidbody2D>();

        // Temporarily freeze physics calculations
        m_rigidBody2d.isKinematic = true;

        // Calculate the anchor position from world coordinates
        m_anchorPos = Camera.main.ScreenToWorldPoint(new Vector3(cursorX, cursorY, 0.0f)) - transform.position;

        // Reset the object's velocity
        m_rigidBody2d.velocity.Set(0.0f, 0.0f);
    }

    void OnEndAnchor()
    {
        if (m_rigidBody2d)
        {
            // Adjust the current throwing velocity by the throw strength
            m_rigidBody2d.velocity *= ThrowStrength;

            // Re-enable physics calculations
            m_rigidBody2d.isKinematic = false;

            m_rigidBody2d = null;
        }
    }
}
