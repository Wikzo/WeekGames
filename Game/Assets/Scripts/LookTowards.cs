using UnityEngine;
using System.Collections;

public class LookTowards : MonoBehaviour
{

    Transform leftEye, rightEye;
    Vector3 leftRelative, rightRelative;
    Quaternion leftRotation, rightRotation;

    int current;


    // Use this for initialization
    void Start()
    {
        //StartCoroutine("LookEyes");

        

        // get eyes
        foreach (Transform child in transform)
        {
            if (child.name == "LeftEye")
                leftEye = child;
            else if (child.name == "RightEye")
                rightEye = child;
        }

        // pick random target
        current = Random.Range(0, GameManager.Instance.transforms.Length);
        float random = Random.Range(1f, 2f);
        StartCoroutine(PickNewSpotToLookAt(random));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            current = Random.Range(0, GameManager.Instance.transforms.Length);


        // left eye
        leftRelative = GameManager.Instance.transforms[current].transform.position - leftEye.position;
        leftRotation = Quaternion.LookRotation(leftRelative);
        //leftEye.rotation = leftRotation;
        leftEye.rotation = Quaternion.Lerp(leftEye.rotation, leftRotation, Time.time * 0.01f);


        // right eye
        rightRelative = GameManager.Instance.transforms[current].transform.position - rightEye.position;
        rightRotation = Quaternion.LookRotation(rightRelative);
        //rightEye.rotation = rightRotation;
        rightEye.rotation = Quaternion.Lerp(rightEye.rotation, rightRotation, Time.time * 0.01f);



        //transform.LookAt(t);
    }

    IEnumerator PickNewSpotToLookAt(float time)
    {
        yield return new WaitForSeconds(time);

        int tries = 0;
        float dot = 0;
        int oldCurrent = current;

        // only pick targets in front of the eyes
        if (dot < 0.5f && tries < 10)
        {
            current = Random.Range(0, GameManager.Instance.transforms.Length);
            
            Vector3 heading = (GameManager.Instance.transforms[current].position - leftEye.position).normalized;
            dot = Vector3.Dot(heading, GameManager.Instance.transforms[current].position);
            


            /*Vector3 eyes = leftEye.TransformDirection(Vector3.forward);
            Vector3 target = (GameManager.Instance.transforms[current].position - leftEye.position).normalized;
                dot = Vector3.Dot(eyes, target);
            
            */
                tries++;


        }

        float random = Random.Range(1f, 2);
        StartCoroutine(PickNewSpotToLookAt(random));
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
