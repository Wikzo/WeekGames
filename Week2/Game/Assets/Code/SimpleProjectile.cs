using UnityEngine;
using System.Collections;

public class SimpleProjectile : Projectile, ITakeDamage
{
    public int Damage;
    public GameObject DestroyedEffect;
    public int PointsToGiveToPlayer;
    public float TimeToLive;
    public AudioClip DestroySound;

    void Update()
    {
        if ((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }

        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }

    private void DestroyProjectile()
    {
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position);

        Destroy(gameObject);

    }

    public void TakeDamage(int damage, GameObject originator)
    {
        if (PointsToGiveToPlayer != 0) // maybe can also give negative points?
        {
            var projectile = originator.GetComponent<Projectile>();

            // player shoots a projectile that hits another projectile (which awards points)
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
                FloatingText.Show(string.Format("+{0}", PointsToGiveToPlayer),
                    "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));
            }
        }

        DestroyProjectile();
    }

    protected override void OnCollideOther(Collider2D coll)
    {
        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D coll, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }
}
