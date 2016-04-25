using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.2f;
    public float ShakeAmountX = 0.2f;
    public float ShakeAmountY = 0.2f;

    private float _shakeTimer;

    public bool StartShake = false;

    private Transform _transform;
    private Vector3 _originalPosition;

    public void StartShaking()
    {
        if (!StartShake)
            StartShake = true;
    }

    void Start()
    {
        _transform = this.transform;
        _originalPosition = _transform.position;
        _shakeTimer = 0f;
    }

    public void Update()
    {
        if (!StartShake)
            _originalPosition = transform.position;

        if (StartShake && _shakeTimer <= ShakeDuration)
        {

            Vector3 shakePosition = new Vector3(_originalPosition.x + Random.insideUnitCircle.x * ShakeAmountX,
                _originalPosition.y + Random.insideUnitCircle.y * ShakeAmountY,
                _originalPosition.z);

            _transform.position = shakePosition;

            _shakeTimer += Time.deltaTime;

        }
        else
        {
            _transform.position = _originalPosition;
            _shakeTimer = 0f;
            StartShake = false;
        }
    }
}
