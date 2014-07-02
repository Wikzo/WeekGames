using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    /// <summary>
    /// Handles input and movement for the 2D player
    /// </summary>

    private const float skinWidth = 0.02f;//0.001f; // change this if character gets stuck in geometry (default = 0.02)
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
    public Vector3 PlatformVelocity { get; private set; }

    // fields
    private Vector2 velocity;
    private Transform _transform;
    private Vector3 localScale;
    private BoxCollider2D boxCollider;
    private ControllerParameters2D overrideParameters;
    private Vector3 raycastTopLeft, raycastBottomRight, raycastBottomLeft;
    private float jumpIn;

    private Vector3 activeGlobalPlatformPoint;
    private Vector3 activeLocalPlatformPoint;



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
            HandleMovingPlatforms();
            CalculateRaysOrigins();

            if (deltaMovement.y < 0 && wasGrouned) // moving down while grounded (e.g. a slope hill)
                HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > 0.001f) // moving right/left
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement); // being affected by gravity
        }

        transform.Translate(deltaMovement, Space.World); // apply the movement


        if (Time.deltaTime > 0) // update velocity
            velocity = deltaMovement / Time.deltaTime;

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingDownSlope)
            velocity.y = 0f;

        // get ready for moving platforms
        if (standingOn != null)
        {
            activeGlobalPlatformPoint = transform.position; // player's position (world)
            activeLocalPlatformPoint = standingOn.transform.InverseTransformPoint(transform.position); // player's position in relation to the platform (world-->local)
            print("move" + Time.time);

            //Debug.DrawLine(transform.position, activeGlobalPlatformPoint, Color.red);
            //Debug.DrawLine(transform.position, activeLocalPlatformPoint, Color.green);
        }
    }

    private void HandleMovingPlatforms() // moving platforms
    {
        if (standingOn != null)
        {
            var newGlobalPlatformPoint = standingOn.transform.TransformPoint(activeLocalPlatformPoint); // player's position in relation to world (local-->world)
            var moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;
            print("HandleMovingPlatforms" + Time.time);

            //print("active:" + activeGlobalPlatformPoint + "; new: " + newGlobalPlatformPoint);
            // activeGlobalPlatformPoint and newGlobalPlatformPoint are the same on non-moving platforms (world coordinates)
            // on moving platforms, there will be a difference between the two, since platform is moving in next frame
            // activeGlobalPlatformPoint will be older than newGlobalPlatformPoint --> gives a difference

            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);

            PlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime; // gives the velocity of the moving platform, e.g., 5
        }
        else
            PlatformVelocity = Vector3.zero;

        standingOn = null;
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
        var rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
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
        if (Mathf.RoundToInt(angle) == 90) // cannot move up angle that is 90 degrees
            return false;

        if (angle > Parameters.SlopeLimit)
        {
            deltaMovement.x = 0;
            return true;
        }

        if (deltaMovement.y > 0.7f)
            return true;

        deltaMovement.x += isGoingRight ? -skinWidth : skinWidth;
        deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
        State.IsMovingUpSlope = true;
        State.IsCollidingBelow = true;
        return true;
        
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {
        var center = (raycastBottomLeft.x + raycastBottomRight.x) / 2;
        var direction = -Vector2.up;

        var slopeDistance = slopeLimitTangent * (raycastBottomRight.x - center);
        var slopeRayVector = new Vector2(center, raycastBottomLeft.y);

        Debug.DrawRay(slopeRayVector, direction * slopeDistance, Color.blue);

        var raycastHit = Physics2D.Raycast(slopeRayVector, direction, slopeDistance, PlatformMask);
        if (!raycastHit)
            return;

        var isMovingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign(deltaMovement.x); // Sign() returns -1, 0 or 1 depending on the value's sign
        if (!isMovingDownSlope)
            return;

        var angle = Vector2.Angle(raycastHit.normal, Vector2.up);
        
        if (Mathf.Abs(angle) < 0.0001f) // standing on something perpendicular to player?
            return;

        // we are moving down on a slope!

        State.IsMovingDownSlope = true;
        State.SlopeAngle = angle;
        deltaMovement.y = raycastHit.point.y - slopeRayVector.y;


    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

    }

    public void OnTriggerExit2D(Collider2D collider)
    {

    }


}
