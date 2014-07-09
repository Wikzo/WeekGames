using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, ITakeDamage
{
    /// <summary>
    /// Driver for the player controls - uses CharacterController2D for actual input handling
    /// </summary>

    private bool isFacingRight;
    private CharacterController2D controller;
    private float normalizedHorizontalSpeed; // -1 = left, 1 = right
    private float rotationWhenDeadZ = 57f;

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public int MaxHealth = 100;
    public GameObject DamageEffect;

    public AudioClip PlayerShootSound;
    public AudioClip PlayerHitSound;
    public AudioClip PlayerGetHealthSound;
    private AudioSource audioSource;

    public Animator Animator;

    public Projectile ProjectileToShoot;
    public GameObject FireProjectileEffect;
    public float FireRate;
    public Transform ProjectileFireLocation;

    public bool IsDead { get; private set; }
    public int Health { get; set; }

    private float canFireIn;

    public void Awake() // important to set "controller" before Start() in Checkpoint.cs
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0; // not flipped (scale > 0) = facing right

        Health = MaxHealth;

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Update()
    {
        if (canFireIn > -1) // only needs to be zero or greater
            canFireIn -= Time.deltaTime;

        if (!IsDead)
            HandleInput();

        // calculate and apply horizontal movement
        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        var horizontalForce = IsDead ? 0 : Mathf.Lerp(controller.Velocity.x, normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor);
        controller.SetHorizontalForce(horizontalForce);

        Animator.SetBool("IsGrounded", controller.State.IsGrounded);
        Animator.SetBool("IsDead", IsDead);
        Animator.SetFloat("Speed", Mathf.Abs(controller.Velocity.x) / MaxSpeed); // between 0 and 1

    }

    public void Kill()
    {
        controller.HandleCollisions = false;
        collider2D.enabled = false;
        IsDead = true;
        Health = 0;

        // tilt the player slightly
        var rotation = isFacingRight ? rotationWhenDeadZ : -rotationWhenDeadZ;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, rotation)); 

        controller.SetForce(new Vector2(0, 12)); // prevent horizontal force
    }

    public void RespawnAt(Transform spawnpoint)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0)); // reset rotation

        if (!isFacingRight)
            Flip();

        controller.HandleCollisions = true;
        collider2D.enabled = true;
        IsDead = false;
        Health = MaxHealth;

        transform.position = spawnpoint.position;
    }

    public void TakeDamage(int damage, GameObject originator)
    {
        audioSource.PlayOneShot(PlayerHitSound);

        FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

        Instantiate(DamageEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    private void HandleInput()
    {
#if UNITY_ANDROID
        // touch input (test)
        if (Input.touchCount > 0)
        {
            var touchCount = Input.touchCount;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
                FireProjectile();

            if (touchCount == 1)
            {
                var touchPos = Input.GetTouch(0).position;

                if (touchPos.x > Screen.width / 2)
                    normalizedHorizontalSpeed = 1;
                else
                    normalizedHorizontalSpeed = -1;
            }
            else if (touchCount == 2 && controller.CanJump)
                controller.Jump();
        }
#endif

        if (Input.GetKey(KeyCode.D))
        {
            normalizedHorizontalSpeed = 1;

            if (!isFacingRight)
                Flip();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            normalizedHorizontalSpeed = -1;

            if (isFacingRight)
                Flip();
        }
        else
            normalizedHorizontalSpeed = 0;

        if (controller.CanJump && (Input.GetKeyDown(KeyCode.W)))// || Input.GetKeyDown(KeyCode.Space))) //|| Input.GetMouseButtonDown(1)))
            controller.Jump();

        /*if (Input.GetMouseButtonDown(0))
            FireProjectile();*/

        if (Input.GetKeyDown(KeyCode.Space))
            FireProjectile();
    }

    private void FireProjectile()
    {
        if (canFireIn > 0)
            return;

        if (FireProjectileEffect != null)
        {
            var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.parent = transform;
        }

        var direction = isFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(ProjectileToShoot, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, controller.Velocity);

        canFireIn = FireRate;

        audioSource.PlayOneShot(PlayerShootSound);
        Animator.SetTrigger("Fire");
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        isFacingRight = transform.localScale.x > 0;
    }

    public void GiveHealth(int health, GameObject gameObject)
    {
        audioSource.PlayOneShot(PlayerGetHealthSound);

        FloatingText.Show(string.Format("+{0} HP", health),
            "PlayerGotHealthText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));

        Health = Mathf.Min(Health + health, MaxHealth);
    }

    public void FinishLevel()
    {
        enabled = false;
        controller.enabled = false;
        collider2D.enabled = false;
    }
}