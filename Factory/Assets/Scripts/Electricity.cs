using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Electricity : MonoBehaviour
{

    public Text startSolar;
    public Text stopSolar;
    public Text electricity;
    public static int electricityCounter;

    private bool isActive;
    public Transform player;
    public float radius = 5f;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        startSolar.enabled = false;
        stopSolar.enabled = false;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        electricity.text = "ELECTRICITY: " + electricityCounter;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius)
        {
            if (isActive == false)
            {
                startSolar.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isActive = true;
                    startSolar.enabled = false;
                    stopSolar.enabled = false;
                }
            }
            else if (isActive == true)
            {
                stopSolar.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isActive = false;
                    timer = 0;
                    startSolar.enabled = false;
                    stopSolar.enabled = false;
                }
            }
        }
        else
        {
            startSolar.enabled = false;
            stopSolar.enabled = false;
        }

        if (isActive == true)
        {
            timer += Time.deltaTime;

            if (timer >= 3)
            {
                electricityCounter += 1;
                timer = 0;
            }
        }

    }
}
