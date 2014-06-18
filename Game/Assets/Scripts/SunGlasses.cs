using UnityEngine;
using System.Collections;

public class SunGlasses : MonoBehaviour
{
    public Texture2D Black;

    byte alpha = 190;

    bool toggleGlasses = false;
    bool showBlack = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // pick up glasses, only if they are currently visible
            if (!toggleGlasses && gameObject.renderer.isVisible)
            {
                toggleGlasses = !toggleGlasses;
                anim.SetBool("toggle", toggleGlasses);
            }
            else if (toggleGlasses) // put down glasses
            {
                toggleGlasses = !toggleGlasses;
                anim.SetBool("toggle", toggleGlasses);
            }

        }
    }

    void ToggleGlasses()
    {
        showBlack = !showBlack;
    }

    void OnGUI()
    {
        if (showBlack)
        {
            GUI.color = new Color32(255, 255, 255, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Black, ScaleMode.StretchToFill);
        }
    }
}
