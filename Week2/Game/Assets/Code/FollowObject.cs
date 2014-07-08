using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour
{
    public Transform Target;
    public Vector2 Offset;
    public GameObject GUIFrame;

    private Transform myTransform;

    private Vector3 min, max;
    private Vector2 frameHalfSize;

    // Use this for initialization
    void Start()
    {
        myTransform = gameObject.transform;

        CameraController c = GameObject.FindObjectOfType<CameraController>();
        min = c.Min;
        max = c.Max;

        frameHalfSize = GUIFrame.transform.localScale / 2;
        frameHalfSize += new Vector2(0.6f, 0.6f); // small offset
    }

    // Update is called once per frame
    void Update()
    {
        var x = Target.transform.position.x + Offset.x;
        var y = Target.transform.position.y + Offset.y;

        x = Mathf.Clamp(x, min.x + frameHalfSize.x, max.x - frameHalfSize.x);
        y = Mathf.Clamp(y, min.y - 20, max.y - frameHalfSize.y); // it's okay for health bar to go under the ground (when falling down/dying)

        myTransform.position = new Vector3(x, y, 0);
    }
}
