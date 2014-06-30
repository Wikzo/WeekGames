using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunnyMouseCreations : MonoBehaviour
{

    bool hasCreated;
    GameObject g;
    float z;
    List<GameObject> cubes = new List<GameObject>();


    void MouseInput(bool leftButton)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (leftButton) // move
        {
            if (!hasCreated) // if (!hasCreated)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g = (GameObject)Instantiate(cube, pos, Quaternion.identity);
                g.renderer.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                Destroy(cube);
                hasCreated = true;
                cubes.Add(g);
            }
            else
                g.transform.position = pos;
        }
        else if (!leftButton) // rotate
        {

            //g.transform.RotateAround(Vector3.left, distance);
            //z += distance;

            foreach (GameObject c in cubes)
            {
                var distanceY = (pos.y - c.transform.position.y);
                var distanceX = (pos.x - c.transform.position.x);

                c.transform.rotation = Quaternion.Euler(new Vector3(0, 0, distanceY * 5));
                c.transform.localScale = new Vector3(distanceX + 5, c.transform.localScale.y, c.transform.localScale.z);
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
            MouseInput(true);
        else if (Input.GetMouseButtonUp(0))
            hasCreated = false;

        if (Input.GetMouseButton(1))
            MouseInput(false);
        else if (Input.GetMouseButtonUp(1))
            z = g.transform.rotation.z;
    }


}
