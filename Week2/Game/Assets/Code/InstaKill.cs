using UnityEngine;
using System.Collections;

public class InstaKill : MonoBehaviour
{
    public void OnTriggerEnter2D (Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player == null)
            return;

        LevelManager.Instance.KillPlayer();
    }
}
