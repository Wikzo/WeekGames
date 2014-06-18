using UnityEngine;
using System.Collections;

public class LookTowards : MonoBehaviour
{
    Transform t;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine("LookEyes");

        t = GameManager.Instance.LeftEye;
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 relativePos = Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;*/

        transform.LookAt(t);
    }

    /*IEnumerator LookEyes()
    {
        while (true)
        {
            transform.LookAt(Target.transform);
            yield return new WaitForSeconds(0.1f);
        }
    }*/
}
