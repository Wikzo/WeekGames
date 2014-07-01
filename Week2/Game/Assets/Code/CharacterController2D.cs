﻿using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    /// <summary>
    /// Handles input and movement for the 2D player
    /// </summary>

    private const float skinWidth = 0.001f; // change this if character gets stuck in geometry (default = 0.02)
    private const int totalHorizontalRays = 8;
    private const int totalVerticalRays = 4;

    private static readonly float slopeLimitTangent = Mathf.Tan(75 * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;
    public ControllerState2D State { get; private set; }

    // properties
    public Vector2 Velocity { get { return velocity; } } // value type
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return overrideParameters ?? DefaultParameters; } } // returns overrideParameters, but if null returns DefaultParameters - Null coalescing (http://www.dotnetperls.com/null-coalescing)
    public GameObject standingOn { get; private set; }
    public bool CanJump
    { 
        get 
        {
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpAnywhere) // can jump anywhere
                return jumpIn <= 0;
            
            if (Parameters.JumpRestrictions == ControllerParameters2D.JumpBehaviour.CanJumpOnGround) // can jump from ground
                return State.IsGrounded;
            else
                return false; // cannot jump
        } 
    }

    // fields
    private Vector2 velocity;
    private Transform _transform;
    private Vector3 localScale;
    private BoxCollider2D boxCollider;
    private ControllerParameters2D overrideParameters;
    private Vector3 raycastTopLeft, raycastBottomRight, raycastBottomLeft;
    private float jumpIn;


    private float verticalDistanceBetweenRays, horizontalDistanceBetweenRays;


    public void Awake()
    {
        HandleCollisions = true;

        State = new ControllerState2D();

        // alisasing components out - for performance
        _transform = gameObject.transform;
        localScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        // calculate box colliders (their scale changes when gameObject scale changes)
        // internally, box collider's size is calculated by multiplying with the transform's local scale

        var colliderWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2*skinWidth);
        horizontalDistanceBetweenRays = colliderWidth / (totalVerticalRays - 1);

        var colliderHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * skinWidth);
        verticalDistanceBetweenRays = colliderHeight / (totalHorizontalRays - 1);
    }

    public void AddForce(Vector2 force)
    {
        velocity = force;
    }

    public void SetForce(Vector2 force)
    {
        velocity += force;
    }

    public void SetHorizontalForce(float x)
    {
        velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        velocity.y = y;
    }

    public void Jump()
    {
        // TODO: moving platform support
        AddForce(new Vector2(0, Parameters.JumpMagnitude));
        jumpIn = Parameters.JumpFrequency;
    }

    public void LateUpdate()
    {
        jumpIn -= Time.deltaTime;

        velocity.y += Parameters.Gravity * Time.deltaTime; // gravity
        Move(velocity * Time.deltaTime);
    }
    
    /// <summary>
    /// Moves the player
    /// </summary>
    /// <param name="deltaMovement"></param>
    private void Move(Vector2 deltaMovement)
    {
        var wasGrouned = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRaysOrigins();

            if (deltaMovement.y < 0 && wasGrouned) // moving down while grounded (e.g. a slope hill)
                HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > 0.001f) // moving right/left
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement); // being affected by gravity
        }

        transform.Translate(deltaMovement, Space.World); // apply the movement

        //TODO: additional moving platform code

        if (Time.deltaTime > 0) // update velocity
            velocity = deltaMovement / Time.deltaTime;

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingDownSlope)
            velocity.y = 0f;
    }

    private void HandlePlatforms()
    {
        
    }

    private void CalculateRaysOrigins()
    {
        var sizeOfBoxCollider = new Vector2(boxCollider.size.x * Mathf.Abs(localScale.x), boxCollider.size.y * Mathf.Abs(localScale.y)) / 2;
        var centerOfBoxCollider = new Vector2(boxCollider.center.x * localScale.x, boxCollider.center.y * localScale.y);

        raycastTopLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + skinWidth, centerOfBoxCollider.y + sizeOfBoxCollider.y - skinWidth);
        raycastBottomRight = transform.position + new Vector3(centerOfBoxCollider.x + sizeOfBoxCollider.x - skinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + skinWidth);
        raycastBottomLeft = transform.position + new Vector3(centerOfBoxCollider.x - sizeOfBoxCollider.x + skinWidth, centerOfBoxCollider.y - sizeOfBoxCollider.y + skinWidth);
    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? raycastBottomRight : raycastBottomLeft;

        for (var i = 0; i < totalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            // we hit something

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(raycastHit.normal, Vector2.up), isGoingRight))
                break; // slopes?

            deltaMovement.x = raycastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            // colliding right/left?
            if (isGoingRight)
            {
                deltaMovement.x -= skinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += skinWidth;
                State.IsCollidingLeft = true;
            }

            // stuck inside geometry?
            if (rayDistance < skinWidth + 0.0001f) // overshoot?
                break;
        }
    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y + skinWidth);
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? raycastTopLeft : raycastBottomLeft;

        rayOrigin.x += deltaMovement.x; // already calculated from MoveHorizontally

        var standingOnDistance = float.MaxValue;

        for (var i = 0; i < totalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * horizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!raycastHit)
                continue;

            if (!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
                if (verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    standingOn = raycastHit.collider.gameObject;
                }
            }

            deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            // colliding above/below?
            if (isGoingUp)
            {
                deltaMovement.y -= skinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += skinWidth;
                State.IsCollidingBelow = true;
                State.IsGrounded = true; // FIX: IsGrounded put in manually
            }

            // going down a slope?
            if (!isGoingUp && deltaMovement.y > 0.0001f)
                State.IsMovingDownSlope = true;

            // stuck inside geometry?
            if (rayDistance < skinWidth + 0.0001f)
                break;
        
        }
    }

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        return false;
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

    }

    public void OnTriggerExit2D(Collider2D collider)
    {

    }


}