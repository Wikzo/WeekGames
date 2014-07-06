using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }

    private List<Checkpoint> checkpoints;
    private int currentCheckpointIndex;

    public Checkpoint DebugSpawn;
    public float RespawnTime = 1.5f;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        // gives sorted list of checkpoints, based on position.x
        checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
        currentCheckpointIndex = checkpoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

#if UNITY_EDITOR
        // spawn the player
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(Player);
        else if (currentCheckpointIndex != -1)
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
        // (else: just let player spawn where he is placed in the scene manually)
#else // build
        if (currentCheckpointIndex != -1)
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
#endif

    }

    public void Update()
    {
        // last checkpoint?
        var isAtLastCheckpoint = currentCheckpointIndex + 1 >= checkpoints.Count;
        if (isAtLastCheckpoint)
            return;

        // haven't hit checkpoint yet
        var distanceToNextCheckpoint = checkpoints[currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        // we hit a new checkpoint!

        checkpoints[currentCheckpointIndex].PlayerLeftCheckpoint();
        
        currentCheckpointIndex++;
        checkpoints[currentCheckpointIndex].PlayerHitCheckpoint();
        // TODO: add time bonus
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
        Player.Kill();
        Camera.IsFollowing = false;

        yield return new WaitForSeconds(RespawnTime);

        Camera.IsFollowing = true;

        if (currentCheckpointIndex != -1)
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);

        // TODO: add points system

    }
}
