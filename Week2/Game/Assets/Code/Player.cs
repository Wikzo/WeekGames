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

    public float MaxSpeed = 8f;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;

    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0; // not flipped (scale > 0) = facing right
    }


    public void Update()
    {
        HandleInput();

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        controller.SetHorizontalForce(Mathf.Lerp(controller.Velocity.x, normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
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