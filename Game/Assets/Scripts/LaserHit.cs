using UnityEngine;
using System.Collections;

public class LaserHit : MonoBehaviour
{
    LineRenderer line;
    public bool LaserEyes = false;

    bool toggleLaserEyes = false;

    float eyeContactTime;
    float eyeContactThreshold = 0.2f;
    bool npcAddedToPlayerList;

    public bool IsNPC = true;

    //public GameObject g;
    // Use this for initialization
    void Start()
    {
        eyeContactTime = 0;
        npcAddedToPlayerList = false;

        line = GetComponent<LineRenderer>();
        if (line == null) Debug.Log("ERROR, needs line renderer!");

        if (!LaserEyes)
            line.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!LaserEyes)
            return;

        if (Input.GetKeyDown(KeyCode.L))
            toggleLaserEyes = !toggleLaserEyes;

        if (toggleLaserEyes == true)
        {
            line.enabled = true;

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

                            print("npc looking at player");

                            if (eyeContactTime > eyeContactThreshold)
                            {
                                GameManager.Instance.PlayerCam.NPCsLookingAtPlayer.Add(gameObject.tag);
                                npcAddedToPlayerList = true;
                                GameManager.Instance.PlayerCam.NPCCanSeePlayer = true;
                            }

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
                        if (hit.transform.tag == "NPC")
                        {
                            eyeContactTime += Time.deltaTime;

                            print("player looking at npc");

                            if (eyeContactTime > eyeContactThreshold)
                            {
                                GameManager.Instance.PlayerCam.PlayerCanSeeNPC = true;
                                GameManager.Instance.PlayerCam.PlayerLookingNPCTarget = hit.transform.tag;
                            }

                        }
                        else
                        {
                            GameManager.Instance.PlayerCam.PlayerCanSeeNPC = false;
                            eyeContactTime = 0;
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
