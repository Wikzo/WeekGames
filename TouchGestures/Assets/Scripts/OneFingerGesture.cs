using UnityEngine;
using System.Collections;

public class OneFingerGesture : MonoBehaviour
{
    public int ComfortLength = 3;

    private GameObject objectToMove;
    private Vector2 v2_prev;
    private Vector2 v2_current;
    private float deltaTouch;

    void Update()
    {

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                v2_prev = touch.position; // vector goes from (0,0,0) to touch pos
            }

            if (touch.phase == TouchPhase.Ended) // swipes
            {
                v2_current = touch.position; // vector goes from (0,0,0) to touch pos

                deltaTouch = v2_current.magnitude - v2_prev.magnitude; // length of swipe
                if (Mathf.Abs(deltaTouch) > ComfortLength)
                {
                    string swipeMessage = "Swipe from the ";
                    if (deltaTouch > 0)
                    {
                        if (Mathf.Abs(v2_current.x - v2_prev.x) > Mathf.Abs(v2_current.y - v2_prev.y))
                            swipeMessage += "left";
                        else
                            swipeMessage += "bottom";
                    }
                    else
                    {
                        if (Mathf.Abs(v2_current.x - v2_prev.x) > Mathf.Abs(v2_current.y - v2_prev.y))
                            swipeMessage += "right";
                        else
                            swipeMessage += "top";
                    }
                    Debug.Log(swipeMessage);

                }
                else // taps
                {
                    // single tap
                    if (touch.tapCount == 1)
                    {
                        Debug.Log("One finger single tap");

                        RaycastHit hit;
                        Ray ray;

                        ray = Camera.main.ScreenPointToRay(touch.position);

                        // hit object?
                        if (Physics.Raycast(ray, out hit))
                        {
                            var name = hit.transform.name;
                            Debug.Log(name);

                            // stop particles
                            if (objectToMove != null && name != objectToMove.transform.name)
                            {
                                objectToMove.particleSystem.Stop();
                            }

                            // start particles
                            objectToMove = GameObject.Find(name);
                            objectToMove.particleSystem.Play();
                        }
                    }
                    // double tap
                    else if (touch.tapCount == 2)
                    {
                        if (objectToMove == null)
                            return;

                        Debug.Log("One finger double tap");

                        float z = Mathf.Abs(Camera.main.transform.position.z - objectToMove.transform.position.z);
                        Vector3 pos = new Vector3(touch.position.x, touch.position.y, z);

                        objectToMove.transform.position = Camera.main.ScreenToWorldPoint(pos);
                    }
                }
            }
        }
    }
}
