using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour
{
    public float Speed;
    public LayerMask CollisionMask;

    public GameObject Owner { get; set; }
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }

    public void Initialize (GameObject owner, Vector2 direction, Vector2 initialVelocity)
    {
        transform.right = direction; // make it point in the right direction
        this.Owner = owner;
        this.Direction = direction;
        this.InitialVelocity = initialVelocity;

        OnInitialized();
    }

    protected virtual void OnInitialized() { }

    public virtual void OnTriggerEnter2D(Collider2D coll)
    {

        // http://acc6.its.brooklyn.cuny.edu/~gurwitz/core5/nav2tool.html
        // http://forum.unity3d.com/threads/getting-layer-masks-to-work.100462/
        // http://answers.unity3d.com/questions/8715/how-do-i-use-layermasks.html
        // http://answers.unity3d.com/questions/150690/using-a-bitwise-operator-with-layermask.html
        // Layer #   Binary      Decimal
        // Layer 0 = 0000 0001 = 1
        // Layer 1 = 0000 0010 = 2
        // Layer 2 = 0000 0100 = 4
        // Layer 3 = 0000 1000 = 8
        // Layer 4 = 0001 0000 = 16
        // Layer 5 = 0010 0000 = 32

        // Layer mask example: layer 1 + layer 2 + layer 5
        // Binary mask = 0010 0110

        // Is a specific layer inside the mask?
        // Is layer 5 in the mask?

        // What is 5 in binary?
        // 1 (decimal) in binary is 0000 0001
        // Shift 1 five times: (i << 5) = 0010 0001 (32 in decimal)

        // Is 32 inside the mask?

        // 0010 0110 (layer mask)
        // & (bitwise and) --> returns 1 if both numbers are 1; else returns 0
        // 0010 0001 (individual layer to test for)
        // -------- result:
        // 0010 0000 = NOT ZERO --> layer IS inside the mask

        // second example ------
        // mask: L2+L4 = 0000 0100 + 0001 0000 = 0001 0100 = 20
        // layer to test for: layer 4
        // 0001 0100
        // &
        // 0001 0000
        // ------- results:
        // 0001 0000 --> layer 4 is inside the mask!
        // second example ------

        // if it does NOT match with collision mask
        if ((CollisionMask.value & (1 << coll.gameObject.layer)) == 0)
        {
            OnNotCollideWith(coll);
            return;
        }

        var isOwner = coll.gameObject == Owner;
        if (isOwner)
        {
            OnCollideOwner(coll);
            return;
        }

        var takeDamage = (ITakeDamage)coll.GetComponent(typeof(ITakeDamage)); // cannot use GetComponent<ITakeDamage>(), since it has to an object that inherits from UnityEngine.Object
        if (takeDamage != null)
        {
            OnCollideTakeDamage(coll, takeDamage);
            return;
        }

        OnCollideOther(coll);
    }

    protected virtual void OnNotCollideWith(Collider2D coll) { }
    protected virtual void OnCollideOwner(Collider2D coll) { }
    protected virtual void OnCollideTakeDamage(Collider2D coll, ITakeDamage takeDamage) { }
    protected virtual void OnCollideOther(Collider2D coll) { }

}