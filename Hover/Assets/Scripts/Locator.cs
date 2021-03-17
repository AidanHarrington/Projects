using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locator : MonoBehaviour
{
    public static Image iconImg;

    public Transform target;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        iconImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            checkOnScreen(target.position);
        }
        else
        {
            checkOnScreen(new Vector3(0,1000,0));
            print("Entered");
        }
    }

    private void checkOnScreen(Vector3 pos)
    {
        transform.position = cam.WorldToScreenPoint(pos);
        
    }
}
