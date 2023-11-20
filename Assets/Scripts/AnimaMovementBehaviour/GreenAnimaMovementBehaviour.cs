using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GreenAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] private PhysicsMaterial2D greenMat;
    [SerializeField] float massRatio = 0.1f;
    [SerializeField] float destroyTime = 1.5f;
    private Rigidbody2D rb;

    private float startTime;

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
}
