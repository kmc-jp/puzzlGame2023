using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{

    LinkedList<GameObject> _managedObjects;
    LinkedList<GameObject> _destroyedObjects;

    public void AddStageObject(GameObject newObject)
    {
        UnityEngine.Debug.AssertFormat(_managedObjects.Find(newObject) == null, "GameLogicManager already contains object '{0}'", newObject.name);
        _managedObjects.AddLast(newObject);
    }

    public void DestroyStageObject(GameObject destroyedObject)
    {
        LinkedListNode<GameObject> destroyedObjectNode = _managedObjects.Find(destroyedObject);

        if (destroyedObjectNode != null)
        {
            // Mark object as destroyed and move it to the destroyed objects list
            _managedObjects.Remove(destroyedObjectNode);
            _destroyedObjects.AddLast(destroyedObjectNode);
        }
    }

    void Start()
    {
        _managedObjects = new LinkedList<GameObject>();
        _destroyedObjects = new LinkedList<GameObject>();
    }

    void Update()
    {
        // Remove all objects in the destroy list
        LinkedListNode<GameObject> it = _destroyedObjects.First;
        while (it != null)
        {
            LinkedListNode<GameObject> next = it.Next;

            // Before final destruction, call the object's OnDeath
            // If OnDeath returns false, do not destroy the object yet
            if (it.Value.GetComponent<GameLogicData>().OnDeath())
            {
                Object.Destroy(it.Value);
                _destroyedObjects.Remove(it);
            }

            it = next;
        }
    }
}
