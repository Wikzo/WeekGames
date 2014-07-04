using System.Collections;

public class GameManager
{
    /// <summary>
    /// Singleton class ("bag") to contain data. Not using MonoBehavoir, so doesn't need DontDestroyOnLoad
    /// </summary>

    public static GameManager Instance { get { return null; } }

    public int Points { get; private set; }

    public void Reset()
    {

    }

    public void AddPoints(int points)
    {}
}