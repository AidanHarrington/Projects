using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public FirebaseScript db;
    public gameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.gameObject.AddComponent<FirebaseScript>();
        db = Camera.main.GetComponent<FirebaseScript>();

        Camera.main.gameObject.AddComponent<gameManager>();
        gm = Camera.main.GetComponent<gameManager>();
    }

    public void SpawnShapes()
    {
        StartCoroutine(createShapes());
    }

    public void SpawnShapesScene3()
    {
        StartCoroutine(createShapesScene3());
    }

    public IEnumerator createShapes()
    {
        yield return StartCoroutine(db.getDataFromFirebase(gm));
        yield return StartCoroutine(db.downloadAndSaveCircle());
        yield return StartCoroutine(db.downloadAndSaveSquare());
    }

    public IEnumerator createShapesScene3()
    {
        yield return StartCoroutine(db.getDataFromFirebase(gm));
        yield return StartCoroutine(db.downloadAndSaveCircle());
        yield return StartCoroutine(db.downloadAndSaveSquare());
        yield return StartCoroutine(gm.moveUpdate());
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene("Scene3");
    }
}
