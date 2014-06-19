using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCamEffects : MonoBehaviour
{

    bool enabled;
    bool increasing;
    bool decreasing;
    bool countingDown;
    public List<string> NPCsLookingAtPlayer;
    public string PlayerLookingNPCTarget;

    public bool NPCCanSeePlayer;
    public bool PlayerCanSeeNPC;

    Blur currentBlur;
    Vignetting currentVign;
    TwirlEffect currentTwirl;
    VortexEffect currentVortex;

    float vignDefaultIntensity;

    // Use this for initialization
    void Start()
    {
        enabled = false;
        increasing = false;
        decreasing = false;
        countingDown = false;

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
        EnableEffects();

        if (enabled)
        {
            //currentBlur.enabled = true;
            currentVign.enabled = true;
            //currentVortex.enabled = true;
        }
        else
        {
            currentBlur.enabled = false;
            //currentVign.enabled = false;
            currentVortex.enabled = false;
        }
    }

    public void EnableEffects()
    {
        if (PlayerCanSeeNPC)
        {
            foreach (string g in NPCsLookingAtPlayer)
                if (g == PlayerLookingNPCTarget)
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
        }
        else if (!countingDown)
            StartCoroutine("CountdownToDisable");
    }



    IEnumerator CountdownToDisable()
    {
        countingDown = true;
        yield return new WaitForSeconds(5.5f);
        
        countingDown = false;

        enabled = false;
        increasing = false;

        PlayerCanSeeNPC = false;
        NPCCanSeePlayer = false;

        NPCsLookingAtPlayer.Clear();
        NPCsLookingAtPlayer.TrimExcess();

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

            //vortex.angle++;
            yield return null;

        } while (!increasing && decreasing);

    }

    IEnumerator Increase()
    {
        while (increasing)
        {
            currentVign.intensity += Time.deltaTime*5;
            //vortex.angle++;
            yield return null;
        }
    }

}