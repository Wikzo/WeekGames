using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public int PointsToGivePlayer;
    public float Speed;
    public float FireRate = 1f;
    public Projectile ProjectileToShoot;
    public GameObject DestroyedEffect;
    public AudioClip ShootSound;

    private CharacterController2D controller;
    private Vector2 direction;
    private Vector2 startPosition;
    private float canFireIn;
    private float viewDistance = 10f;

    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        direction = new Vector2(-1, 0);
        startPosition = transform.position;
    }

    public void Update()
    {
        controller.SetHorizontalForce(direction.x * Speed);

        // colliding with wall?
        if ((direction.x < 0 && controller.State.IsCollidingLeft) || (direction.x > 0 && controller.State.IsCollidingRight))
        {
            direction = -direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((canFireIn -= Time.deltaTime) > 0)
            return;

        // within range of player?
        var raycast = Physics2D.Raycast(transform.position, direction, viewDistance, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;

        var projectile = (Projectile)Instantiate(ProjectileToShoot, transform.position, transform.rotation);
        projectile.Initialize(gameObject, direction, controller.Velocity);
        canFireIn = FireRate;

        if (ShootSound != null)
            AudioSource.PlayClipAtPoint(ShootSound, transform.position);
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage, GameObject originator)
    {
        if (PointsToGivePlayer != 0)
        {
            var projectile = originator.GetComponent<Projectile>();
            if (projectile != null & projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGivePlayer);
                FloatingText.Show(string.Format("+{0}", PointsToGivePlayer),
                    "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));
            }

        }
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}