using System.Collections;

public class GameManager
{
    /// <summary>
    /// Singleton class ("bag") to contain data. Not using MonoBehavoir, so doesn't need DontDestroyOnLoad
    /// </summary>

    private static GameManager instance;
    public static GameManager Instance { get { return instance ?? (instance = new GameManager()); } }

    private GameManager() {} // constructor makes it so ONLY the GameManager class can instantiate GameManager objects

    public int Points { get; private set; }

    public void Reset()
    {
        Points = 0;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    internal void ResetPointsTo(int savedPoints)
    {
        Points = savedPoints;
    }
}