using UnityEngine;
using System.Collections;
using System;

[Serializable] // make variables show up in Unity Inspector
public class ControllerParameters2D
{
    public enum JumpBehaviour
    {
        CanJumpOnGround,
        CanJumpAnywhere,
        CannotJump
    }

    public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);
    
    [Range(0, 90)] // Inspector slider
    public float SlopeLimit = 30f;

    public float Gravity = -25f;

    public JumpBehaviour JumpRestrictions;

    public float JumpFrequency = 0.25f; // how often can repeatedly press the jump button
}