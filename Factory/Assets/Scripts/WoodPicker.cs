using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodPicker : MonoBehaviour
{
    public Text pickWood;
    public Text wood;
    public static int woodCounter;
    public Transform player;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        pickWood.enabled = false;
        woodCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        wood.text = "WOOD: " + woodCounter;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= radius)
        {
            pickWood.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                woodCounter++;
            }
        }
        else
        {
            pickWood.enabled = false;
        }
    }
}
