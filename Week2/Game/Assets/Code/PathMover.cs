using UnityEngine;
using System.Collections;

public class PathMover : MonoBehaviour
{
	public EaseType moveEase = EaseType.BackIn;
	public EaseType stopEase = EaseType.CubeOut;
	public float moveDuration = 1;
	public float stopTweenDuration = 0.25f;
	public float stopDuration = 1f;
	public Vector3 stopScale = new Vector3(2f, 1f, 0.5f);
	public Transform[] points;
	public int startIndex;

	void Start()
	{
        if (points.Length > 0)
    		StartCoroutine(MoveOnPath());
	}

	IEnumerator MoveOnPath()
	{
		int index = startIndex;
		transform.localPosition = points[index].position;
		while (true)
		{
			index = (index + 1) % points.Length;
            transform.LookAt(points[index].position);
            yield return StartCoroutine(transform.MoveTo(points[index].position, moveDuration, moveEase));
			if (stopTweenDuration > 0)
				StartCoroutine(transform.ScaleFrom(stopScale, stopTweenDuration, stopEase));
			if (stopDuration > 0)
				yield return StartCoroutine(Auto.Wait(stopDuration));
		}
	}
}
