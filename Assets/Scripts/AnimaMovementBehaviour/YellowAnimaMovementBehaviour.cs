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
    [SerializeField] float destroyTime = 3.0f;
    private Rigidbody2D rb;
    private float startTime;



    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplyPhysicsParameters();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > destroyTime)
        {
            Destroy(this.gameObject);
        }
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
