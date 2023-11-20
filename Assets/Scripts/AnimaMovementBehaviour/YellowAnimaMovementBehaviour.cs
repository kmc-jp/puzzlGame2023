using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class YellowAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] float GravityScale = 0.0f;
    [SerializeField] float AngularDrag = 5;
    [SerializeField] float Drag = 5;
    [SerializeField] private PhysicsMaterial2D yellowMat;
    [SerializeField] float massRatio = 0.1f;
    private Rigidbody2D rb;



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
        rb = GetComponent<Rigidbody2D>();
        rb.sharedMaterial = yellowMat;

        //重さの調整
        
        rb.useAutoMass = true;
        rb.useAutoMass = false;
        rb.mass = (rb.mass * massRatio);
        rb.gravityScale = GravityScale;

        //値は適当です
        rb.angularDrag = AngularDrag;
        rb.drag = Drag;
    }
}
