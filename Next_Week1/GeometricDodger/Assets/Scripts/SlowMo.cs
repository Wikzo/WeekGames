using UnityEngine;
using System.Collections;

public class SlowMo : MonoBehaviour
{
    //the factor used to slow down time
    public float slowFactor = 4f;
	public float slowMoPitch = .8f;
	public float normalPitch = 1f;
	
	public AudioClip slowIn;
	public AudioClip slowOut;
	
    //the new time scale
    private float slowTimeScale;
	private float slowFixedDeltaTime;
	private float slowMaximumDeltaTime;
	
	private float normalTimeScale;
	private float normalFixedDeltaTime;
	private float normalMaximumDeltaTime;
	
	private bool hasSlowedDown;

    // Called when this script starts
    void Start()
    {
		normalTimeScale = 1f;
		normalFixedDeltaTime = Time.fixedDeltaTime;
		normalMaximumDeltaTime = Time.maximumDeltaTime;
		
        //calculate the new time scale
        slowTimeScale = Time.timeScale/slowFactor;
		slowFixedDeltaTime = Time.fixedDeltaTime/slowFactor;
		slowMaximumDeltaTime = Time.maximumDeltaTime/slowFactor;
		
		hasSlowedDown = false;
    }

    public void BeginSlowMotion()
    {
        if (!hasSlowedDown)
        {
            GetComponent<AudioSource>().PlayOneShot(slowIn);
            hasSlowedDown = true;
        }
        gameObject.GetComponent<AudioSource>().pitch = slowMoPitch;
        //assign the 'newTimeScale' to the current 'timeScale'
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = slowFixedDeltaTime;
        Time.maximumDeltaTime = slowMaximumDeltaTime;
    }

    public void EndSlowMotion()
    {
        if (hasSlowedDown)
        {
            GetComponent<AudioSource>().PlayOneShot(slowOut);
            hasSlowedDown = false;
        }
        gameObject.GetComponent<AudioSource>().pitch = normalPitch;
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = normalFixedDeltaTime;
        Time.maximumDeltaTime = normalMaximumDeltaTime;
    }
}
