using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathDefinition : MonoBehaviour
{
    //public Transform[] Points; // old array

    private List<Transform> targetPoints;

    public IEnumerator<Transform> GetPathEnumerator() // ping-pong pattern
    {
        // needs 1 point to go to target
        if (targetPoints.Count < 1 || targetPoints == null)
            yield break; // terminate sequence

        var direction = 1; // forward/backward
        var index = 0;

        while (true)
        {
            yield return targetPoints[index]; // yield = give control back to the caller

            if (targetPoints.Count == 1)
                continue;

            if (index <= 0)
                direction = 1; // forward
            else if (index >= targetPoints.Count - 1)
                direction = -1; // backward

            index = index + direction;
        }
    }

    /*void GetTargetsBasedOnChildren() // array - old
    {
        // doesn't work, since array has to be initialized for FollowPath to work

        int counter = 0;
        foreach (Transform child in transform)
            counter++;

        Points = new Transform[counter];

        int index = 0;

        foreach (Transform child in transform)
            Points[index++] = child;
    }*/

    void GetPointsBasedOnChildObjects()
    {
        //Debug.Log("Getting list of children");

        targetPoints = new List<Transform>();
        foreach (Transform child in transform)
            targetPoints.Add(child);
    }

    void Awake()
    {
        GetPointsBasedOnChildObjects();
    }

    // debug draw in scene view
    public void OnDrawGizmos() // or OnDrawGizmosSelected()
    {
        //if (!Application.isPlaying)
        GetPointsBasedOnChildObjects();

        // needs 2 points to draw line
        /*if (Points.Length < 2 || Points == null)
            return;
        */
        for (var i = 1; i < targetPoints.Count; i++)
        {
            Gizmos.DrawLine(targetPoints[i - 1].position, targetPoints[i].position);
        }
    }
}
