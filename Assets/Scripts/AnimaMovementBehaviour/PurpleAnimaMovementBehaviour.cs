using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PurpleAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] private PhysicsMaterial2D purpleMat;
    [SerializeField] float massRatio = 0.5f;
    private Rigidbody2D rb;

    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sharedMaterial = purpleMat;

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
