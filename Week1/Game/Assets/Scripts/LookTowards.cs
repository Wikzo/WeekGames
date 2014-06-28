using UnityEngine;
using System.Collections;

public class LookTowards : MonoBehaviour
{

    Transform leftEye, rightEye;
    Vector3 leftRelative, rightRelative;
    Quaternion leftRotation, rightRotation;

    float lerpSpeed = 0.01f;
    float MinLerpSpeed = 0.001f;
    float MaxLerpSpeed = 0.005f;

    bool hasFoundTarget;
    int current;
    Vector3 defaultForwardDir;
    Quaternion defaultRotation;

    public bool IsFrontNPC = false;


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

        defaultForwardDir = leftEye.TransformDirection(Vector3.forward);
        defaultRotation = leftEye.rotation;

        if (IsFrontNPC)
            StartCoroutine(FrontNPC());
        else
        // pick random target
        StartCoroutine(PickNewSpotToLookAt(Random.Range(0f, 0.5f)));
    }

    IEnumerator FrontNPC()
    {
        // so he doesnt start staring at player from the beginning

        hasFoundTarget = true;

        lerpSpeed = MaxLerpSpeed;

        // left eye
        leftRelative = GameManager.Instance.Outside.transform.position - leftEye.position;
        leftRotation = Quaternion.LookRotation(leftRelative);

        // right eye
        rightRelative = GameManager.Instance.Outside.transform.position - rightEye.position;
        rightRotation = Quaternion.LookRotation(rightRelative);

        // set rotations directly (no lerp)
        leftEye.rotation = leftRotation;
        rightEye.rotation = rightRotation;


        yield return new WaitForSeconds(1);
        StartCoroutine(PickNewSpotToLookAt(Random.Range(0f, 0.5f)));

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFoundTarget)
            return;

        // left eye
        leftEye.rotation = Quaternion.Lerp(leftEye.rotation, leftRotation, Time.time * lerpSpeed);


        // right eye
        rightEye.rotation = Quaternion.Lerp(rightEye.rotation, rightRotation, Time.time * lerpSpeed);


        //transform.LookAt(t);
    }

    IEnumerator PickNewSpotToLookAt(float time)
    {
        yield return new WaitForSeconds(time);

        int tries = 0;
        float dot = 0;
        bool hasFoundOne = false;

        // only pick targets in front of the eyes
        if (!!hasFoundOne || tries < 10)
        {
            int randomIndex = Random.Range(0, GameManager.Instance.transforms.Length);

            //Vector3 heading = (GameManager.Instance.transforms[randomIndex].position - leftEye.position).normalized;
            //dot = Vector3.Dot(heading, GameManager.Instance.transforms[randomIndex].position);

            //Vector3 eyes = leftEye.TransformDirection(Vector3.forward);
            Vector3 dir = (GameManager.Instance.transforms[randomIndex].position - leftEye.position).normalized;
            dot = Vector3.Dot(dir, defaultForwardDir);

            tries++;

            // target is within field of vision
            if (dot > 0.3f)
            {
                current = randomIndex;
                //print("found " + GameManager.Instance.transforms[randomIndex].name);
                hasFoundOne = true;

                lerpSpeed = Random.Range(MinLerpSpeed, MaxLerpSpeed);

                // left eye
                leftRelative = GameManager.Instance.transforms[current].transform.position - leftEye.position;
                leftRotation = Quaternion.LookRotation(leftRelative);


                // right eye
                rightRelative = GameManager.Instance.transforms[current].transform.position - rightEye.position;
                rightRotation = Quaternion.LookRotation(rightRelative);

                hasFoundTarget = true;


            }
            else // target is behind (go to default)
            {
                //hasFoundTarget = false; // no new target = don't move

                //leftRotation = defaultRotation;
                //rightRotation = defaultRotation;
            }

        }

        float random = Random.Range(1f, 7f);
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
