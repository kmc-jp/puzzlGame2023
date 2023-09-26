using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PurpleAnimaMovementBehaviour : AnimaObject
{
    [SerializeField] Color Color;
    [SerializeField] PhysicsMaterial2D Material;

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

    void ApplyPhisicsParameters()
    {
        var rb = this.GetComponent<Rigidbody2D>();

        var collider = this.GetComponent<Collider2D>();
        collider.sharedMaterial = Material;

        Material.friction = 1.0f;
        Material.bounciness = 0.0f;

    }
}
