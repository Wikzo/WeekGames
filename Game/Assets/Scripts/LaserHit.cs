using UnityEngine;
using System.Collections;

public class LaserHit : MonoBehaviour
{
    LineRenderer line;
    public bool LaserEyes = false;

    bool toggleLaserEyes = false;

    //public GameObject g;
    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (line == null) Debug.Log("ERROR, needs line renderer!");

        if (!LaserEyes)
            line.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!LaserEyes)
            return;

        if (Input.GetKeyDown(KeyCode.L))
            toggleLaserEyes = !toggleLaserEyes;

        if (toggleLaserEyes == true)
        {
            line.enabled = true;

            // start position
            line.SetPosition(0, transform.position);

            // hitting something?
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out hit, 200f))
            {
                if (hit.collider)
                {
                    //Debug.DrawLine(transform.position, hit.point);
                    line.SetPosition(1, hit.point);

                    //print(hit.transform.name);
                    //g.transform.position = hit.point;
                }
            }
            else
                line.SetPosition(1, fwd * 200); // default, not hitting anything
        }
        else
            line.enabled = false;
    }
}
