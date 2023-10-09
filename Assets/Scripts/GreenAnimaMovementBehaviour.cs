using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GreenAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;

    private PhysicsMaterial2D greenMat;
    private Rigidbody2D rb;
    [SerializeField] private float bounciness = 1.0f;
    [SerializeField] private float friction = 1.0f;

    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Green material not created yet
        //greenMat.friction = friction;
        //greenMat.bounciness = bounciness;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        //rb.sharedMaterial = greenMat;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
