using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    // follow player
    // margin
    // boundary box

    public Transform Player;
    public Vector2 Margin;
    public Vector2 Smoothing;
    public BoxCollider2D Bounds;

    public bool IsFollowing { get; set; }

    private Vector3 min, max;
    private Camera cam;

    void Start()
    {
        min = Bounds.bounds.min;
        max = Bounds.bounds.max;
        IsFollowing = true;
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (IsFollowing)
        {
            if (Mathf.Abs(x - Player.position.x) > Margin.x)
                x = Mathf.Lerp(x, Player.position.x, Smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - Player.position.y) > Margin.y)
                y = Mathf.Lerp(y, Player.position.y, Smoothing.y * Time.deltaTime);
        }

        var cameraHalfWidth = cam.orthographicSize * cam.aspect;

        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, min.y + cam.orthographicSize, max.y - cam.orthographicSize);

        cam.transform.position = new Vector3(x, y, transform.position.z);
    }
}