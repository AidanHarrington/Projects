using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform bulletSpawned;
    private bool shot = false;
    private float currentTime;

    public GameObject player, bullet, enemy, bulletSpawnPoint;
    public float waitTime;

    public float health = 50f;
    public static bool inRange = false,
        isDead = false;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);

        if (inRange)
        {
            if (currentTime == 0)
            {
                shoot();
            }

            if (shot && currentTime < waitTime)
            {
                currentTime += 1 * Time.deltaTime;
            }

            if (currentTime >= waitTime)
            {
                currentTime = 0;
            }
        }
        

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(enemy.gameObject);
        print("Dead");
        isDead = true;
    }

    public void shoot()
    {
        shot = true;
        bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletSpawned.rotation = this.transform.rotation;
    }

}
