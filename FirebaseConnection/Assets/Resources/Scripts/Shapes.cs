using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapes : MonoBehaviour
{
    public string name, id, time, x, y;
    public int instance;

    public Shapes(string _id, int _instance, string _name, string _time, string _x, string _y)
    {
        instance = _instance;
        id = _id;
        name = _name;
        time = _time;
        x = _x;
        y = _y;
    }
}
