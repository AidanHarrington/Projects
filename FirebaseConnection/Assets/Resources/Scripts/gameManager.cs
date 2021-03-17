using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class gameManager : MonoBehaviour
{
    public FirebaseScript db;
    public Shapes circle, square;
    public GameObject sphere;
    string path = "https://aidanharringtonunity.firebaseio.com/";
    public InputField pathInput;
    public Button signInButton;

    string time = DateTime.Now.ToString();
    string x = "0", y = "0";

    private void Start()
    {
        circle = new Shapes("", 0, "Circle", time, x, y);
        square = new Shapes("", 1, "Square", time, x, y);

        Camera.main.gameObject.AddComponent<FirebaseScript>();
        db = Camera.main.GetComponent<FirebaseScript>();
        DontDestroyOnLoad(this);
        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            pathInput.text = path;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene1") {
            if (pathInput.text != "https://aidanharringtonunity.firebaseio.com/")
            {
                signInButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                signInButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void LoadScene()
    {
        path = pathInput.text;
        StartCoroutine(initDB(path));
        StartCoroutine(clearDB());
        StartCoroutine(addCircle());
        StartCoroutine(addSquare());

        SceneManager.LoadScene("Scene2");
    }

    public IEnumerator moveUpdate()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                GameObject.Find(sphere.name + "(Clone)").transform.position += Vector3.up * 2f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                GameObject.Find(sphere.name + "(Clone)").transform.position += Vector3.down * 2f;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                GameObject.Find(sphere.name + "(Clone)").transform.position += Vector3.left * 2f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                GameObject.Find(sphere.name + "(Clone)").transform.position += Vector3.right * 2f;
            }

            circle.x = (GameObject.Find(sphere.name + "(Clone)").transform.position.x).ToString();
            circle.y = (GameObject.Find(sphere.name + "(Clone)").transform.position.y).ToString();

            yield return StartCoroutine(db.updateDataClass(circle.id, circle, this));
        }
        
    }

    IEnumerator addCircle()
    {
        yield return db.addDataClass(circle, this);
    }
    
    IEnumerator addSquare()
    {
        yield return db.addDataClass(square, this);
    }

    IEnumerator clearDB()
    {
        yield return db.clearFirebase();
    }

    IEnumerator initDB(string path)
    {
        yield return db.initFirebase(path);
    }

    public void updateMovement(Shapes shape)
    {
        try
        {
            GameObject.Find(shape.name + "(Clone)").transform.position = new Vector2(float.Parse(shape.x), float.Parse(shape.y));

        }
        catch (Exception e)
        {

        }
    }

    public IEnumerator spawnCircle(Shapes shape)
    {
        int x = int.Parse(shape.x);
        int y = int.Parse(shape.y);
        circle = shape;
        yield return Instantiate(Resources.Load("Prefabs/Circle"), new Vector2(x, y), Quaternion.identity);
    }
    
    public IEnumerator spawnSquare(Shapes shape)
    {
        int x = int.Parse(shape.x);
        int y = int.Parse(shape.y);
        square = shape;
        yield return Instantiate(Resources.Load("Prefabs/Square"), new Vector2(x, y), Quaternion.identity);
    }
}
