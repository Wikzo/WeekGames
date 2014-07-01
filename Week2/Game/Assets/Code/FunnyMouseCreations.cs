using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunnyMouseCreations : MonoBehaviour
{

    enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    private bool hasCreated;
    private GameObject g;
    private float z;
    private List<GameObject> cubes = new List<GameObject>();
    private int layer;
    private bool holdingShift;

    public Shader SelfIlum;
    public Shader ParticlesAdditive;
    public int MaxNumberOfCubes = 50;

    void Start()
    {
        layer = gameObject.layer;
    }


    void MouseInput(MouseButton button)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (button == MouseButton.Left) // move
        {
            if (!hasCreated || holdingShift) // if (!hasCreated)
            {
                if (cubes.Count >= MaxNumberOfCubes)
                {
                    var c = cubes[0];
                    cubes.RemoveAt(0);
                    Destroy(c);
                }

                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                // add stuff to cube
                // -----------------------------------
                g = (GameObject)Instantiate(cube, pos, Quaternion.identity);
                //g.transform.position += new Vector3(0, 0, g.transform.localScale.z / 2);
                g.renderer.material = new Material(SelfIlum);
                g.renderer.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                g.layer = layer;
                DestroyImmediate(g.GetComponent<BoxCollider>());
                g.AddComponent<BoxCollider2D>();
                g.AddComponent<Rigidbody2D>();
                
                // trail
                var trail = g.AddComponent<TrailRenderer>();
                trail.material = new Material(ParticlesAdditive);
                trail.startWidth = 0.2f;
                trail.endWidth = 0.8f;
                trail.time = 2f;
                // -----------------------------------

                Destroy(cube);
                hasCreated = true;
                cubes.Add(g);
            }
            else
                g.transform.position = pos;
        }
        else if (button == MouseButton.Right) // rotate
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
        else if (button == MouseButton.Middle)
        {
            // calculate midpoint
            float x = 0;
            float y = 0;

            foreach (GameObject c in cubes)
            {
                x += c.transform.position.x;
                y += c.transform.position.y;
            }
            Vector2 MidPoint = new Vector2(x / cubes.Count, y / cubes.Count);
            Vector2 distance = pos - MidPoint;

            // move all cubes
            foreach (GameObject c in cubes)
                c.transform.position = new Vector3(c.transform.position.x + distance.x, c.transform.position.y + distance.y, c.transform.position.z);

        }
    }

    public void Update()
    {
        // place/move
        if (Input.GetMouseButton(0))
            MouseInput(MouseButton.Left);
        else if (Input.GetMouseButtonUp(0))
            hasCreated = false;

        // burst mode
        if (Input.GetKey(KeyCode.LeftShift))
            holdingShift = true;
        else
            holdingShift = false;

        // rotate/scale
        if (Input.GetMouseButton(1))
            MouseInput(MouseButton.Right);
        else if (Input.GetMouseButtonUp(1))
            z = g.transform.rotation.z;

        // move all
        if (Input.GetMouseButton(2))
            MouseInput(MouseButton.Middle);

    }


}
