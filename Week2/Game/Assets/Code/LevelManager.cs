using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.Now - started; } }
    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Math.Max(0, secondDifference) * BonusSecondsMultiplier;
        }
    }

    private List<Checkpoint> checkpoints;
    private int currentCheckpointIndex;
    private DateTime started;
    private int savedPoints; // used to cache number of points, in case the player dies unexpectedly

    public Checkpoint DebugSpawn;
    public float RespawnTime = 1.5f;
    public int BonusCutoffSeconds; // threshold - max time player has to go from checkpoint A to checkpoint B to receive bonus
    public int BonusSecondsMultiplier; // amount of seconds left * multiplier

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

        started = DateTime.Now;

        // loop backwards to find what stars belong to what checkpoints
        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>(); // using Linq
        foreach (var listener in listeners)
        {
            for (var i = checkpoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - checkpoints[i].transform.position.x;

                if (distance < 0) // looking at a star behind (left to) a checkpoint (not what we want)
                    continue;

                checkpoints[i].AssignObjectToCheckpoint(listener);
                break;
            }
        }

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
        GameManager.Instance.AddPoints(CurrentTimeBonus);
        savedPoints = GameManager.Instance.Points;
        started = DateTime.Now;
        
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
        started = DateTime.Now;
        GameManager.Instance.ResetPointsTo(savedPoints);
    }
}
