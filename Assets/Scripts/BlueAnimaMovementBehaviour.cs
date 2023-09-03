using UnityEngine;

public class BlueAnimaMovementBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ApplyPhysicsParameters();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    void ApplyPhysicsParameters()
    {
        this.gameObject.AddComponent<Rigidbody2D>();
        var rb = this.GetComponent<Rigidbody2D>();

        //重さの調整
        rb.useAutoMass = true;

        //var collider2d = GetComponent<Collider2D>(); //colliderの実装待ち
        //collider2d.density = 1.1f; // 値は適当です。


        rb.angularDrag = 10;     
        rb.drag = 10;
    }
}
