﻿using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour
{

    public Transform Destination;
    public PathedProjectile Projectile;
    public GameObject SpawnEffect;
    public AudioClip SpawnProjectileSound;

    public float Speed;
    public float FireRate;

    private float nextShotInSeconds;

    // Use this for initialization
    void Start()
    {
        nextShotInSeconds = FireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if ((nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(Destination, Speed);

        if (SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);

        if (SpawnProjectileSound != null)
            AudioSource.PlayClipAtPoint(SpawnProjectileSound, transform.position);
    }

    void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
