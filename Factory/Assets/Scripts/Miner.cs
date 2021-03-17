using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    public Text mine;
    public Text iron;
    public static int ironCounter;
    public Transform player;
    public float radius;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        mine.enabled = false;
        ironCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        iron.text = "IRON: " + ironCounter;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius)
        {
            mine.enabled = true;

            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;

                if (timer >= 3)
                {
                    ironCounter += 10;
                    timer = 0;
                }
                
            }
        }
        else
        {
            mine.enabled = false;
        }
    }
}
