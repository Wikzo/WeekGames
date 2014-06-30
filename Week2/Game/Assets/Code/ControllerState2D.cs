using UnityEngine;
using System.Collections;

public class ControllerState2D
{
    // colliders
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool IsGrounded { get; set; }

    // slope
    public bool IsMovingDownSlope { get; set; }
    public bool IsUpDownSlope { get; set; }
    public float SlopeAngle { get; set; }

    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }

    public void Reset()
    {
        // sets all booleans to false
        IsMovingDownSlope = IsMovingDownSlope = IsCollidingRight = IsCollidingLeft = IsCollidingAbove = IsCollidingBelow = false;

        SlopeAngle = 0f;
    }

    public override string ToString()
    {
        return string.Format("(Controller: r:{0}, l:{1}, a:{2}, b:{3}, down-slope:{4}, up-slope:{5}, angle:{6})",
            IsCollidingRight,
            IsCollidingLeft,
            IsCollidingAbove,
            IsCollidingBelow,
            IsMovingDownSlope,
            IsUpDownSlope,
            SlopeAngle);
    }

}