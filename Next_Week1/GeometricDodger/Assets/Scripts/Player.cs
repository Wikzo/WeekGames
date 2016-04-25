using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 500f;
    public float ImpulsePower = 200f;

    public Material Normal;
    public Material Glass;

    public SlowMo SlowMotion;
    public LayerMask BulletLayerMask;
    public float BulletRadius = 5;

    Vector3 moveDirection;

    private InputDevice input;
    private Rigidbody rb;
    private Renderer _renderer;
    private TrailRenderer trail;

    private bool boost;

    private bool _isBlinking;
    private float blinkCounter = 0;
    private float blinkRate = 5;
    private float totalBlinkTime = 0;
    private Color RenderColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        RenderColor = _renderer.material.color;
    }

    void Update()
    {
        input = InputManager.ActiveDevice;
        moveDirection = new Vector3(input.LeftStickX, 0, input.LeftStickY);

        CheckIfCloseToBullet();

        if (input.Action1.WasPressed)
        {
            boost = true;
            StartCoroutine(BlinkRespawn(2));
        }
        else
            boost = false;

        if (input.Action1.IsPressed && CheckIfCloseToBullet())
            SlowMotion.BeginSlowMotion();
    }

    private bool CheckIfCloseToBullet()
    {
        if (Physics.CheckSphere(transform.position, BulletRadius, BulletLayerMask))
            return true;
        else
            return false;
    }


    void FixedUpdate()
    {
        if (input == null)
            return;

        rb.AddForce(moveDirection * MoveSpeed);

        if (boost)
        {
            rb.AddForce(moveDirection.normalized * ImpulsePower, ForceMode.Impulse);
        }
    }

    IEnumerator BlinkRespawn(int count)
    {
        _isBlinking = true;

        //trail.enabled = true;

        /*if (_renderer.material.color == RenderColor)
            _renderer.material.color = Color.white;
        else
            _renderer.material.color = RenderColor;*/

        _renderer.material = Glass;

        //_renderer.enabled = !_renderer.enabled;
        yield return new WaitForSeconds(0.2f);

        if (count - 1 > 0)
            StartCoroutine(BlinkRespawn(count - 1));
        else
        {
            //_renderer.material.color = RenderColor;
            trail.enabled = false;
            _isBlinking = false;

            _renderer.material = Normal;

            SlowMotion.EndSlowMotion();
        }
    }
}
