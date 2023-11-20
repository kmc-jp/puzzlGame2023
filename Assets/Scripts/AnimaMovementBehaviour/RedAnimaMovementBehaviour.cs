using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RedAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;

    //vectorReverse 運動の向きが逆になります
    //power 物体に与える力が強くなります。
    //speed 運動の切り替わりの速度が早くなります
    [SerializeField] private bool vectorReverse = false;
    [SerializeField] private float power = 7.0f;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] PhysicsMaterial2D redMat;
    [SerializeField] float massRatio = 1.0f;
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
        rb = GetComponent<Rigidbody2D>();
        if (vectorReverse) power = -power;
        startTime = Time.time;
        rb.sharedMaterial = redMat;

        //重さの調整
        rb.useAutoMass = true;
        rb.useAutoMass = false;
        rb.mass = (rb.mass * massRatio);
        power = rb.mass * power;
    }

    // Update is called once per frame
    void Update()
    {
        GiveForce();
    }

    void GiveForce()
    {
        if (((Time.time - startTime) * speed) < 1)
        {
            rb.AddForce(new Vector2(power, 0f));
        }
        else if (((Time.time - startTime) * speed) < 3)
        {
            rb.AddForce(new Vector2(-power, 0f));
        }


        if (Time.time - startTime > destroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
