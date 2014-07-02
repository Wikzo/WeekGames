using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour
{
    public float JumpMagnitude = 20f;
    public bool UseBouncyAnimation = true;

    private Vector3 bouncyScaleFrom;
    private Vector3 bouncyScaleTo = new Vector3(1f, 0.8f, 1f);
    private float bouncyDuration = 0.15f;

    void Start()
    {
        bouncyScaleFrom = transform.localScale;
    }


    public void ControllerEnter2D(CharacterController2D controller)
    {
        if (UseBouncyAnimation)
            StartCoroutine(EnableMoverAnimation(controller));
        else
            controller.SetVerticalForce(JumpMagnitude);
    }

    public void ControllerExit2D(CharacterController2D controller)
    {

    }

    IEnumerator EnableMoverAnimation(CharacterController2D controller)
    {
        var mover = gameObject.AddComponent<AutoMover>();
        mover.fromScale = bouncyScaleFrom;
        mover.toScale = bouncyScaleTo;
        mover.duration = bouncyDuration;

        controller.SetVerticalForce(JumpMagnitude);


        yield return new WaitForSeconds(bouncyDuration + 0.1f);
        Destroy(mover);
        transform.localScale = bouncyScaleFrom;

    }
}
