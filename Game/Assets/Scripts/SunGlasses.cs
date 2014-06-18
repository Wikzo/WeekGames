using UnityEngine;
using System.Collections;

public class SunGlasses : MonoBehaviour
{
    public Texture2D Black;

    byte alpha = 190;

    bool toggleGlasses = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            toggleGlasses = !toggleGlasses;
            //anim.Play
        }
    }

    void OnGUI()
    {
        if (toggleGlasses)
        {
            GUI.color = new Color32(255, 255, 255, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Black, ScaleMode.StretchToFill);
        }
    }
}
