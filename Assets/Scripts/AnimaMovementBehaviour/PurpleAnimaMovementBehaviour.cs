using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PurpleAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] private PhysicsMaterial2D purpleMat;
    [SerializeField] float massRatio = 0.5f;
    [SerializeField] float destroyTime = 5.0f;
    private Rigidbody2D rb;

    private float startTime;

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
