using UnityEngine;
using System.Collections;

public class PointStar : MonoBehaviour, IPlayerRespawnListener
{
    public GameObject Effect;
    public int PointsToAdd = 10;

    public AudioClip HitStarSound;
    public Animator Animator;

    public SpriteRenderer Renderer;

    private bool isCollected;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (isCollected)
            return;

        if (collider.GetComponent<Player>() == null)
            return;

        if (HitStarSound != null)
            AudioSource.PlayClipAtPoint(HitStarSound, transform.position); // cannot use audio.PlayOneShot(), since the object gets disabled just after this

        GameManager.Instance.AddPoints(PointsToAdd);
        Instantiate(Effect, transform.position, transform.rotation);

        FloatingText.Show(string.Format("+{0}", PointsToAdd), "PointStarText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2.5f, 50f));

        isCollected = true;

        Animator.SetTrigger("Collect");
    }

    public void FinishAnimationEvent()
    {
        Animator.SetTrigger("Reset");
        Renderer.enabled = false; // will be set to active again if player dies/respawns
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        isCollected = false;
        Renderer.enabled = true;
    }
}
