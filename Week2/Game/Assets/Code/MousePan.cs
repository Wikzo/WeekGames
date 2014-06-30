using UnityEngine;
using System.Collections;

public class MousePan : MonoBehaviour
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public float NormalPanSpeed = 0.08f;
    public float ShiftFastPanSpeed = 0.2f;
    public float CtrlSlowPanSpeed = 0.03f;
    

    public MouseButton ButtonToUse = MouseButton.Middle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton((int)ButtonToUse))
            PanMouse();

        // scroll
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (Camera.main.isOrthoGraphic)
                Camera.main.orthographicSize++;// = Mathf.Max(Camera.main.orthographicSize - 1, 1);
            else
                Camera.main.fieldOfView++;

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (Camera.main.isOrthoGraphic)
                Camera.main.orthographicSize--;
            else
                Camera.main.fieldOfView--;

        }
    }

    void PanMouse()
    {
        // drag-pan camera
        Vector2 pos = Input.mousePosition;

        // no smoothing - just binary movement
        //float xPoint = (pos.x < Screen.width / 2 - 2) ? -1 : 1;
        //float yPoint = (pos.y < Screen.height / 2 - 2) ? -1 : 1;

        // linear smoothing
        float xPoint = 0;
        float yPoint = 0;
        float xMove = 0;
        float yMove = 0;

        // get raw value
        xPoint = (pos.x - Screen.width / 2);
        yPoint = (pos.y - Screen.height / 2);

        // clamp
        xPoint = Mathf.Clamp(xPoint, -120f, 120f);
        yPoint = Mathf.Clamp(yPoint, -120f, 120f);

        // smooth lerp fast speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            xMove = Mathf.Lerp(xMove, xPoint, ShiftFastPanSpeed);
            yMove = Mathf.Lerp(yMove, yPoint, ShiftFastPanSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftControl))// smooth lerp slow speed
        {
            xMove = Mathf.Lerp(xMove, xPoint, CtrlSlowPanSpeed);
            yMove = Mathf.Lerp(yMove, yPoint, CtrlSlowPanSpeed);
        }
        else // smooth lerp standard speed
        {
            xMove = Mathf.Lerp(xMove, xPoint, NormalPanSpeed);
            yMove = Mathf.Lerp(yMove, yPoint, 0.08f);
        }

        Vector3 camPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(camPos.x + xMove * Time.deltaTime, camPos.y + yMove * Time.deltaTime, camPos.z);
    }
}
