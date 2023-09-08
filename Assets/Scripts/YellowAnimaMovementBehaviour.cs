using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class YellowAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;

    public override Color GetColor()
    {
        return Color;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
