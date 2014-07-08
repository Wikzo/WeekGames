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
        StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus));
    }

    private IEnumerator PlayerHitCheckpointCo(int bonus)
    {
        FloatingText.Show("Checkpoint!", "CheckpointText", new CenteredTextPositioner(0.2f));

        yield return new WaitForSeconds(0.75f);
        FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckpointTextBonus", new CenteredTextPositioner(0.25f));
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
