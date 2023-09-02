using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderTestScript : MonoBehaviour
{
    public GameObject testObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Vector3 vec = new Vector3(5,5,0);
            Instantiate(testObject,vec,Quaternion.identity);
        }
    }
}
