using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFactory : MonoBehaviour
{
    public Text createCar;
    public Text Insufficient;
    public Text car;
    public Text fullPark;
    public Text inProcess;
    public static int carCounter;
    public GameObject[] Cars;

    private bool isActive;
    public Transform player;
    public float radius;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        createCar.enabled = false;
        Insufficient.enabled = false;
        fullPark.enabled = false;
        inProcess.enabled = false;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        car.text = "CARS: " + carCounter + "/6";

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius)
        {

            if (Electricity.electricityCounter >= 2 && Miner.ironCounter >= 10 && WoodPicker.woodCounter >= 5 && carCounter < 6)
            {
                createCar.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isActive = true;

                    createCar.enabled = false;
                    Insufficient.enabled = false;
                    inProcess.enabled = true;
                }

                if (isActive)
                {
                    timer += Time.deltaTime;

                    if (timer >= 3)
                    {
                        Electricity.electricityCounter -= 2;
                        Miner.ironCounter -= 10;
                        WoodPicker.woodCounter -= 5;
                        carCounter++;
                        Cars[carCounter - 1].gameObject.SetActive(true);
                        createCar.enabled = false;
                        Insufficient.enabled = false;
                        inProcess.enabled = false;

                        isActive = false;
                        timer = 0;
                    }
                }
            }
            else if (carCounter >= 6)
            {
                fullPark.enabled = true;
            }
            else if (Electricity.electricityCounter <= 2 || Miner.ironCounter <= 10 || WoodPicker.woodCounter <= 5)
            {
                Insufficient.enabled = true;
            }


        }
        else
        {
            createCar.enabled = false;
            Insufficient.enabled = false;
            fullPark.enabled = false;
            inProcess.enabled = false;
        }
    }

}
