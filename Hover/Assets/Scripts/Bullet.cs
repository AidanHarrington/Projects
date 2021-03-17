using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject triggeringEnemy, player;

    public float speed, maxDistance, damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        maxDistance += 1 * Time.deltaTime;

        if(maxDistance >= 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.tag == "Bullet")
        {
            if (other.tag == "Player")
            {
                player = other.gameObject;
                player.GetComponent<PlayerController>().health -= 20;
                
            }
        }
        else if (this.tag == "PlayerBullet")
        {
            if (other.tag == "Enemy")
            {
                triggeringEnemy = other.gameObject;
                triggeringEnemy.GetComponentInChildren<Enemy>().health -= damage;
                
            }
        }
    }
}
