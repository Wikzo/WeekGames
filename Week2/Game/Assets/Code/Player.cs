using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
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
    public GameObject TakeDamageEffect;

    public bool IsDead { get; private set; }
    public int Health { get; set; }

    public void Awake() // important to set "controller" before Start() in Checkpoint.cs
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0; // not flipped (scale > 0) = facing right

        Health = MaxHealth;
    }

    public void Update()
    {
        if (!IsDead)
            HandleInput();


        // calculate and apply horizontal movement
        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        var horizontalForce = IsDead ? 0 : Mathf.Lerp(controller.Velocity.x, normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor);
        controller.SetHorizontalForce(horizontalForce);
        
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

    public void TakeDamage(int damage)
    {
        Instantiate(TakeDamageEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    private void HandleInput()
    {
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

        if (controller.CanJump && Input.GetKeyDown(KeyCode.Space))
            controller.Jump();
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        isFacingRight = transform.localScale.x > 0;
    }

}