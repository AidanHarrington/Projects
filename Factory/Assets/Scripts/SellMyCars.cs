using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellMyCars : MonoBehaviour
{

    public Text sellCar;
    public Text noCars;
    public Text gold;
    public static int goldCounter;

    public GameObject[] Cars;

    public Transform player;
    public float radius = 7f;

    // Start is called before the first frame update
    void Start()
    {
        sellCar.enabled = false;
        noCars.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gold.text = "GOLD: " + goldCounter;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius)
        {

            if (CarFactory.carCounter > 0)
            {
                sellCar.enabled = true;
                if (Input.GetKey(KeyCode.E))
                {
                    for (int i = 0; i <= CarFactory.carCounter; i++)
                    {
                        Cars[CarFactory.carCounter - 1].gameObject.SetActive(false);
                        goldCounter += 5;
                        CarFactory.carCounter--;
                    }
                    sellCar.enabled = false;
                }
            }
            else
            {
                noCars.enabled = true;
            }

        }
        else
        {
            noCars.enabled = false;
            sellCar.enabled = false;
        }
    }
}
