using UnityEngine;
using System.Collections;

public class PointStar : MonoBehaviour, IPlayerRespawnListener
{
    public GameObject Effect;
    public int PointsToAdd = 10;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() == null)
            return;

        GameManager.Instance.AddPoints(PointsToAdd);
        Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false); // will be set to active again if player dies/respawns

        FloatingText.Show(string.Format("+{0}", PointsToAdd), "PointStarText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        gameObject.SetActive(true);
    }
}
