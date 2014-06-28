using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCamEffects : MonoBehaviour
{

    bool enabled;
    bool increasing;
    bool decreasing;
    bool countingDown;
    public string NPCsLookingAtPlayer;
    public string PlayerLookingNPCTarget;

    AudioSource audio;

    Blur currentBlur;
    Vignetting currentVign;
    TwirlEffect currentTwirl;
    VortexEffect currentVortex;

    float vignDefaultIntensity;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = 0;
        audio.enabled = false;

        enabled = false;
        increasing = false;
        decreasing = false;
        countingDown = false;

        NPCsLookingAtPlayer = "null_npc";
        PlayerLookingNPCTarget = "null_player";

        currentBlur = GetComponent<Blur>();
        currentVign = GetComponent<Vignetting>();
        currentTwirl = GetComponent<TwirlEffect>();
        currentVortex = GetComponent<VortexEffect>();

        vignDefaultIntensity = currentVign.intensity;

        currentBlur.enabled = false;
        currentVign.enabled = false;
        currentTwirl.enabled = false;
        currentVortex.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.SunGlassesOn)
            enabled = false;
        else
            EnableEffects();

        if (enabled)
        {
            currentBlur.enabled = true;
            currentVign.enabled = true;
            currentVortex.enabled = true;
        }
        else
        {
            currentBlur.enabled = false;
            //currentVign.enabled = false;
            //currentVortex.enabled = false;

            if (audio.volume > 0)
                audio.volume -= Time.deltaTime * 0.2f;

            if (currentVortex.angle > 0)
                currentVortex.angle -= Time.deltaTime * 50;
        }
    }

    public void EnableEffects()
    {
        if (NPCsLookingAtPlayer == PlayerLookingNPCTarget)
        {
            if (!increasing)
            {
                increasing = true;
                StartCoroutine("Increase");
            }

            enabled = true;
            StopCoroutine("CountdownToDisable");
            StartCoroutine("CountdownToDisable");
        }
        else if (!countingDown)
            StartCoroutine("CountdownToDisable");
    }



    IEnumerator CountdownToDisable()
    {
        countingDown = true;
        yield return new WaitForSeconds(0.5f);
        
        countingDown = false;

        enabled = false;
        increasing = false;

        NPCsLookingAtPlayer = "null_npc";
        PlayerLookingNPCTarget = "null_player";

        StartCoroutine("Decrease");
    }

    void ResetEffects()
    {
        currentVign.intensity = vignDefaultIntensity;
    }

    IEnumerator Decrease()
    {
        decreasing = true;

        do
        {
            if (currentVign.intensity > 0)
                currentVign.intensity -= Time.deltaTime * 15;
            else
            {
                decreasing = false;
            }

            if (currentBlur.blurSize > 0)
                currentBlur.blurSize -= Time.deltaTime * 15;

            if (audio.volume > 0)
                audio.volume -= Time.deltaTime * 0.2f;

            if (currentVortex.angle > 0)
                currentVortex.angle -= Time.deltaTime * 50;

            //vortex.angle++;
            yield return null;

        } while (!increasing && decreasing);

    }

    IEnumerator Increase()
    {
        while (increasing)
        {
            audio.enabled = true;

            if (audio.volume < 0.4f)
                audio.volume += Time.deltaTime * 0.1f;

            if (audio.volume > 0.15)
            {
                currentVortex.enabled = true;
                if (currentVortex.angle < 300)
                    currentVortex.angle += Time.deltaTime * 25;
            }

            currentVign.intensity += Time.deltaTime*5;

            if (currentBlur.blurSize < 2.5f)
                currentBlur.blurSize += Time.deltaTime * 2;


            yield return null;
        }
    }

}