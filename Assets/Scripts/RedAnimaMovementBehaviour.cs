using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RedAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;

    //vectorReverse 運動の向きが逆になります
    //power 物体に与える力が強くなります。
    //speed 運動の切り替わりの速度が早くなります
    [SerializeField] private bool vectorReverse = false;
    [SerializeField] private float power = 4.0f;
    [SerializeField] private float speed = 3.0f;
    private Rigidbody2D _rb;
    private float _startTime;

    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (vectorReverse) power = -power;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        GiveForce();
    }

    void GiveForce()
    {
        if (((Time.time - _startTime) * speed) < 1)
        {
            _rb.AddForce(new Vector2(power, 0f));
        }
        else if (((Time.time - _startTime) * speed) < 3)
        {
            _rb.AddForce(new Vector2(-power, 0f));
        }
    }
}
