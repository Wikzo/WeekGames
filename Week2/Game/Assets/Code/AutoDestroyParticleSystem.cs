using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        StartCoroutine(DestroyParticle());
    }

    IEnumerator DestroyParticle()
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
