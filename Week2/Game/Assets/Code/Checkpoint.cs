using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> listeners;

    public void Awake()
    {
        listeners = new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckpoint()
    {
    }

    private IEnumerator PlayerHitCheckpointCo()
    {
        yield break;
    }

    public void PlayerLeftCheckpoint() { }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);

        foreach (var listener in listeners)
            listener.OnPlayerRespawnInThisCheckpoint(this, player);
    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
        listeners.Add(listener);
    }

}
