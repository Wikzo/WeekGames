using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CeilingTurnOffEditor : MonoBehaviour
{

    public bool TurnOff = true;
    public List<Renderer> Renders;

    #if UNITY_EDITOR
    void Update()
    {
        if (Renders == null || Renders.Count <= 0)
            return;

        if (TurnOff)
            foreach (Renderer r in Renders) r.enabled = false;
        else
            foreach (Renderer r in Renders) r.enabled = true;

    }
    #endif
}