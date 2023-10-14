using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes:
// This will only emit collisions with objects that also have an ObjectCollisionDetector
// Collision handling should _only_ affect this object as the colliding object is responsible
//  for handling this collision for that object itself
// The OnCollision events will only trigger on the server
public class ObjectCollisionDetector : NetworkBehaviour
{
    public delegate void OnCollisionFunc(GameObject obj, Vector2 contactPoint);
    public event OnCollisionFunc OnCollisionWithHostile;
    public event OnCollisionFunc OnCollisionWithFriendly;
    public event OnCollisionFunc OnCollisionWithNeutral;

    [SerializeField]
    protected uint _ownerNetworkId = 0;

    public void SetOwnerNetworkId(uint networkId)
    {
        _ownerNetworkId = networkId;
    }

    protected bool IsHostileObject(GameObject obj)
    {
        // Hostile objects are objects with a valid owner network ID different from the owner
        ObjectCollisionDetector otherCollider = obj.GetComponent<ObjectCollisionDetector>();
        if (otherCollider != null)
        {
            if (_ownerNetworkId == 0)
            {
                return false;
            }

            return otherCollider._ownerNetworkId != _ownerNetworkId;
        }

        return false;
    }

    protected bool IsFriendlyObject(GameObject obj)
    {
        // Friendly objects are objects with a valid owner network ID same as the owner
        ObjectCollisionDetector otherCollider = obj.GetComponent<ObjectCollisionDetector>();
        if (otherCollider != null)
        {
            if (_ownerNetworkId == 0)
            {
                return false;
            }

            return otherCollider._ownerNetworkId == _ownerNetworkId;
        }

        return false;
    }

    protected bool IsNeutralObject(GameObject obj)
    {
        // Neutral objects are objects with an owner network ID of 0
        ObjectCollisionDetector otherCollider = obj.GetComponent<ObjectCollisionDetector>();
        if (otherCollider != null)
        {
            if (_ownerNetworkId == 0)
            {
                return true;
            }

            return otherCollider._ownerNetworkId == 0;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject, collision.GetContact(0).point);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider.gameObject, collider.ClosestPoint(transform.position));
    }

    private void HandleCollision(GameObject otherGameObject, Vector2 contactPoint)
    {
        // Only do collision checks on the server
        if (isServer)
        {
            if (otherGameObject.GetComponent<ObjectCollisionDetector>() != null)
            {
                if (IsHostileObject(otherGameObject))
                {
                    OnCollisionWithHostile?.Invoke(otherGameObject, contactPoint);
                }
                else if (IsFriendlyObject(otherGameObject))
                {
                    OnCollisionWithFriendly?.Invoke(otherGameObject, contactPoint);
                }
                else if (IsNeutralObject(otherGameObject))
                {
                    OnCollisionWithNeutral?.Invoke(otherGameObject, contactPoint);
                }
            }
        }
    }
}
