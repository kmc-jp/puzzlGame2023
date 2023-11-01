using UnityEngine;

[DisallowMultipleComponent]
public class TutorialUIManager : MonoBehaviour {
    private static TutorialUIManager _singleton;
    
    public void Awake() {
        if (_singleton != null) {
            Debug.LogAssertion("TutorialUIManager must be the singleton.");
            Destroy(this);
        }

        _singleton = this;
    }
}