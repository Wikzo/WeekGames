using UnityEngine;
using System.Collections;

public class CannonShoot : MonoBehaviour
{
    public PathedProjectileSpawner PathedProjectileSpawner;

    public void FireCannon()
    {
        PathedProjectileSpawner.Fire();
    }

}