using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BlueAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] float Drag = 10.0f;
    [SerializeField] float AngularDrag = 10.0f;
    [SerializeField] PhysicsMaterial2D blueMat;
    [SerializeField] float massRatio = 0.8f;

    public override Color GetColor()
    {
        return Color;
    }

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
        var rb = this.GetComponent<Rigidbody2D>();

        rb.sharedMaterial = blueMat;

        //重さの調整
        rb.useAutoMass = true;
        rb.useAutoMass = false;
        rb.mass = (rb.mass * massRatio);

        //var collider2d = GetComponent<Collider2D>(); //colliderの実装待ち
        //collider2d.density = 1.1f; // 値は適当です。

        rb.angularDrag = AngularDrag;
        rb.drag = Drag;
    }
}
