using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private bool isFacingRight;
    private CharacterController2D controller;
    private float normalizedHorizontalSpeed; // -1 = left, 1 = right

    public float MaxSpeed;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;

    public void Start()
    {
        controller = GetComponent<CharacterController2D>();
        isFacingRight = transform.localScale.x > 0; // not flipped (scale > 0) = facing right
    }

    public void Update()
    {
        //HandleInput();

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;
        controller.SetHorizontalForce(Mathf.Lerp(controller.Velocity.x, normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
    }

    private void HandleInput()
    {
        throw new System.NotImplementedException();
    }
}
