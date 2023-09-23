using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class YellowAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] float GravityScale = 0.0f;
    [SerializeField] float AngularDrag = 5;
    [SerializeField] float Drag = 5;

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
        this.gameObject.AddComponent<Rigidbody2D>();
        var rb = this.GetComponent<Rigidbody2D>();

        //重さの調整
        rb.useAutoMass = true;
        rb.gravityScale = GravityScale;

        //値は適当です
        rb.angularDrag = AngularDrag;
        rb.drag = Drag;
    }
}
