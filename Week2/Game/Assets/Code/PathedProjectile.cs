using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour, ITakeDamage
{
    public GameObject DestroyEffect;
    public int PointsToGivePlayer = 0;
    public AudioClip DestroySound;

    private Transform destination;
    private float speed;

    public void Initialize(Transform destination, float speed)
    {
        this.destination = destination;
        this.speed = speed;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * speed);

        var distanceSquared = (destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > 0.01f * 0.01f)
            return;

        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position);

        Destroy(gameObject);

    }

    public void TakeDamage(int damage, GameObject originator)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        var projectile = originator.GetComponent<Projectile>();
        if (projectile != null && projectile.Owner.GetComponent<Player>() != null && PointsToGivePlayer != 0)
        {
            GameManager.Instance.AddPoints(PointsToGivePlayer);
            FloatingText.Show(string.Format("+{0}", PointsToGivePlayer),
                "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));
        }
    }
}
