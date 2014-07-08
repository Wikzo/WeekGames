using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour
{
    public string LevelName;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<Player>() == null)
            return;

        LevelManager.Instance.GotoNextLevel(LevelName);
    }

}
