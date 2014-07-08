using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    public float Speed = 1f;

    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Rotate(Vector3.forward, Speed * Time.deltaTime);
    }
}
