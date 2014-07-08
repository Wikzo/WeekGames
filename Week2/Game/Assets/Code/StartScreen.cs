using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public string FirstLevel;

    void Start()
    {
        GameManager.Instance.Reset();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Application.LoadLevel(FirstLevel);
    }
}