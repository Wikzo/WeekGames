using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathDefinition : MonoBehaviour
{
    public Transform[] Points;

    public IEnumerator<Transform> GetPathsEnumerator()
    {
        throw new NotImplementedException();
    }

    // debug draw in scene view
    public void OnDrawGizmos() // or OnDrawGizmosSelected()
    {
        if (Points.Length < 2 || Points == null)
            return;

        for (var i = 1; i < Points.Length; i++)
        {
            Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
        }
    }
}
