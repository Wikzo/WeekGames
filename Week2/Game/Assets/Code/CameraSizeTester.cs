using UnityEngine;
using System.Collections;

public class CameraSizeTester : MonoBehaviour
{
    public Camera cam;
    private Transform t;
    // Use this for initialization
    void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // make box fill whole camera view
        // --------------------------------
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float height = cam.orthographicSize * 2f;
        float width = height * aspectRatio;

        //print(aspectRatio == cam.aspect + float.Epsilon);

        //t.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 2);
        //t.localScale = new Vector3(width, height, t.localScale.z);

        // --------------------------------



        // lock the camera inside boundary box
        // --------------------------------

        float minX = -2;
        float maxX = 120;
        float posX = transform.position.x;
        float input = 0;

        input = Input.GetAxis("Horizontal");

        posX += input;

        posX = Mathf.Clamp(posX, minX + width, maxX - width);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        // --------------------------------


    }
}