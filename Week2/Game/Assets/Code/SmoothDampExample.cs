using UnityEngine;
using System.Collections;

// http://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html

public class SmoothDampExample : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.3F;
	public Vector3 velocity = Vector3.zero;
	void Update()
	{
		//Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
		transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
	}
}
