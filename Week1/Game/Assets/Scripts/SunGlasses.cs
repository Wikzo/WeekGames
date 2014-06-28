using UnityEngine;
using System.Collections;

public class SunGlasses : MonoBehaviour
{
    public Texture2D Black;

    byte alpha = 190;

    bool toggleGlasses = false;
    bool showBlack = false;

    Animator anim;

    public AudioLowPassFilter LowPassFilter;

    bool playingAnimationNow;

    void Start()
    {
        anim = GetComponent<Animator>();
        LowPassFilter.enabled = false;

        playingAnimationNow = false;

    }

    void Update()
    {
        // only play animation one at a time
        if (playingAnimationNow)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // only play one animation at a time
            //if (anim.GetCurrentAnimatorStateInfo(0).IsName("SunGlassesPutOn"))
              //  return;

            // pick up glasses, only if they are currently visible
            if (!toggleGlasses && gameObject.renderer.isVisible)
            {
                toggleGlasses = !toggleGlasses;
                anim.SetBool("toggle", toggleGlasses);

                GameManager.Instance.SunGlassesOn = true;
            }
            else if (toggleGlasses) // put down glasses
            {
                toggleGlasses = !toggleGlasses;
                anim.SetBool("toggle", toggleGlasses);

                GameManager.Instance.SunGlassesOn = false;
            }

        }
    }

    void PlayingAnimationNow()
    {
        playingAnimationNow = true;
    }

    void StoppingAnimationNow()
    {
        playingAnimationNow = false;
    }

    void ToggleGlasses()
    {
        showBlack = !showBlack;
        LowPassFilter.enabled = !LowPassFilter.enabled;
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
