using UnityEngine;
using System.Collections;

public class LaserHit : MonoBehaviour
{
    LineRenderer line;
    public bool LaserEyes = false;

    bool toggleLaserEyes = false;

    float eyeContactTime;
    float eyeContactThreshold = 0f;

    public bool IsNPC = true;


    //public GameObject g;
    // Use this for initialization
    void Start()
    {
        eyeContactTime = 0;

        line = GetComponent<LineRenderer>();
        if (line == null) Debug.Log("ERROR, needs line renderer!");

        line.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!LaserEyes)
            return;

        if (Input.GetKeyDown(KeyCode.L))
            line.enabled = !line.enabled;


        //if (toggleLaserEyes == true)
        if (true)
        {
            // start position
            line.SetPosition(0, transform.position);

            // hitting something?
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out hit, 200f))
            {
                if (hit.collider)
                {
                    //Debug.DrawLine(transform.position, hit.point);
                    line.SetPosition(1, hit.point);

                    //print(hit.transform.name);
                    //g.transform.position = hit.point;

                    if (IsNPC) // npc looking at player
                    {
                        if (hit.transform.tag == "Player")
                        {
                            eyeContactTime += Time.deltaTime;

                            //if (eyeContactTime > eyeContactThreshold)
                            //{
                                GameManager.Instance.PlayerCam.NPCsLookingAtPlayer = gameObject.tag;
                            //}

                        }
                        else
                        {
                            eyeContactTime = 0;

                            /*if (npcAddedToPlayerList)
                            {
                                GameManager.Instance.PlayerCam.NPCsLookingAtPlayer.Remove(gameObject);
                                npcAddedToPlayerList = false;
                            }*/
                        }
                    }
                    else if (!IsNPC) // player looking at npc
                    {
                        string t = hit.transform.tag;
                        if (t == "1" || t == "2" || t == "3" || t == "4" || t == "5" || t == "6" || t == "7" || t == "8" || t == "9" || t == "10" || t == "11")
                        {
                            eyeContactTime += Time.deltaTime;

                            //if (eyeContactTime > eyeContactThreshold)
                            //{
                                GameManager.Instance.PlayerCam.PlayerLookingNPCTarget = hit.transform.tag;
                            //}

                        }
                        else
                        {
                            eyeContactTime = 0;
                            GameManager.Instance.PlayerCam.PlayerLookingNPCTarget = "null_player";
                        }
                    }
                }
            }
            else
                line.SetPosition(1, fwd * 200); // default, not hitting anything
        }
        else
            line.enabled = false;
    }
}
