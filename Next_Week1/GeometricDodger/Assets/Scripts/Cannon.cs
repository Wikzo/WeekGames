using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public GameObject Bullet;

    public float MinShootInterval = 1;
    public float MaxShootInterval = 5;

    private float nextShootTime;
    private Transform player;
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        transform.LookAt(player);

        nextShootTime -= Time.deltaTime;

        if (nextShootTime <= 0)
        {
            Instantiate(Bullet, transform.position, transform.rotation);

            nextShootTime = Random.Range(MinShootInterval, MaxShootInterval);
        }
    }
}
