using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GreenAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] private PhysicsMaterial2D greenMat;
    [SerializeField] float massRatio = 0.1f;
    private Rigidbody2D rb;

    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Green material not created yet
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.sharedMaterial = greenMat;
        //重さの調整
        rb.useAutoMass = true;
        rb.useAutoMass = false;
        rb.mass = (rb.mass * massRatio);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
