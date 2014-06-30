using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    private const float skinWidth = 0.02f;
    private const int totalHorizontalRays = 8;
    private const int totalVerticalRays = 4;

    private static readonly float slopeLimitTangent = Mathf.Tan(75 * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }

    public void Awake()
    {

    }

    public void AddForce(Vector2 force)
    {

    }

    public void SetForce(Vector2 force)
    {

    }

    public void SetHorizontalForce(float x)
    {

    }

    public void SetVerticalForce(float y)
    {

    }

    public void Jump()
    {

    }

    public void LateUpdate()
    {

    }

    private void Move(Vector2 deltaMovement)
    {

    }

    private void HandlePlatforms()
    {

    }

    private void CalculateRaysOrigins()
    {

    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {

    }

    private void MoveVertically(ref Vector2 deltaMovement)
    {

    }

    private void HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {

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
