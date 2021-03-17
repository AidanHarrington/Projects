using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using Firebase.Unity.Editor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.IO;
public class FirebaseScript : MonoBehaviour
{
    
    DatabaseReference reference;

    //reference to the storage bucket
    FirebaseStorage storage;

    //main data dictionary
    Dictionary<string, object> myDataDictionary;

    FirebaseAuth auth;

    string email = "aidanh@gmail.com";
    string password = "User123";

    public bool signedin = false;

    //list data from firebase
    void Start()
    {



    }

    public IEnumerator addDataClass(Shapes shape,gameManager gm)
    {
        //create a unique ID
        string newkey = reference.Push().Key;
        Debug.Log(newkey);
        //the key for the current player
        shape.id = newkey;
        //Update the unique key with the data I want to insert
        yield return StartCoroutine(updateDataClass(newkey, shape, gm));
    }

    public IEnumerator updateDataClass(string childlabel, Shapes shape, gameManager gm)
    {
        string newData = JsonUtility.ToJson(shape);
        //find the child of player4 that corresponds to playername and set the value to whatever is inside newdata
        Task updateJsonValueTask =  reference.Child(childlabel).SetRawJsonValueAsync(newData).ContinueWithOnMainThread(
            updJsonValueTask =>
            {
                if (updJsonValueTask.IsCompleted)
                {
                   // dataupdated = true;
                }

            });

        yield return new WaitUntil(() => updateJsonValueTask.IsCompleted);

        DatabaseReference updatedPos = reference.Child(shape.id);
        updatedPos.ValueChanged += (sender, args) => handlePos(sender, args, gm);

    }

    public IEnumerator clearFirebase()
    {
        Task removeAllRecords = reference.RemoveValueAsync().ContinueWithOnMainThread(
            rmAllRecords =>
            {
                if (rmAllRecords.IsCompleted)
                {
                    Debug.Log("Database clear");
                }
            });

        yield return new WaitUntil(() => removeAllRecords.IsCompleted);

    }

    //sign in to the firebase instance so we can read some data
    //Coroutine Number 1
    IEnumerator signInToFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        //the outside task is a DIFFERENT NAME to the anonymous inner class
        Task signintask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
             signInTask =>
             {
                 if (signInTask.IsCanceled)
                 {
                     //write cancelled in the console
                     Debug.Log("Cancelled!");
                     return;
                 }
                 if (signInTask.IsFaulted)
                 {
                     //write the actual exception in the console
                     Debug.Log("Something went wrong!" + signInTask.Exception);
                     return;
                 }

                 Firebase.Auth.FirebaseUser loggedInUser = signInTask.Result;
                 Debug.Log("User " + loggedInUser.DisplayName + " has logged in!");
               
             }
            );

        yield return new WaitUntil(() => signintask.IsCompleted);

        Debug.Log("User has signed in");
    }

    public IEnumerator getDataFromFirebase(gameManager gm)
    {

        Task getdatatask = reference.GetValueAsync().ContinueWithOnMainThread(
            getValueTask =>
            {
                if (getValueTask.IsFaulted)
                {
                    Debug.Log("Error getting data " + getValueTask.Exception);
                }

                if (getValueTask.IsCompleted)
                {
                    DataSnapshot snapshot = getValueTask.Result;
                    //Debug.Log(snapshot.Value.ToString());

                    //snapshot object is casted to an instance of its type
                    myDataDictionary = (Dictionary<string, object>)snapshot.Value;


                    //    Debug.Log("Data received");
                    
                }


            }
            );

        yield return new WaitUntil(() => getdatatask.IsCompleted);

        //the data has been saved to snapshot here
        yield return StartCoroutine(displayData(gm));
    }

    IEnumerator displayData(gameManager gm)
    {
        foreach (var element in myDataDictionary)
        {
            Dictionary<string, object> details = (Dictionary<string, object>)element.Value;
            string name = (details["name"]).ToString();
            int instance = Convert.ToInt32(details["instance"]);
            string time = (details["time"]).ToString();
            string x = (details["x"]).ToString();
            string y = (details["y"]).ToString();

            Shapes tmp = new Shapes(element.Key, instance, name, time, x, y);

            if (instance == 0)
            {
                yield return gm.spawnCircle(tmp);
            }
            else
            {
                yield return gm.spawnSquare(tmp);
            }
        }

        yield return null;
    }

    public IEnumerator initFirebase(string path)
    {
        if (!signedin) { 
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(path);
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            yield return signInToFirebase();
            Debug.Log("Firebase Initialized!");
            yield return true;
            signedin = true;
        } else
        {
            yield return null;
        }
    }

    void handlePos(object sender, ValueChangedEventArgs args, gameManager gm)
    {
        DataSnapshot snapshot = args.Snapshot;

        Dictionary<string, object> details = (Dictionary<string, object>)snapshot.Value;
        List<string> detailList = new List<string>();

        foreach (var element in details)
        {
            string detail = element.Value.ToString();
            detailList.Add(detail);
        }

        Shapes shape = new Shapes(detailList[0], int.Parse(detailList[1]), detailList[2], detailList[3], detailList[4], detailList[5]);

        if (detailList[1] == "0")
        {
            gm.updateMovement(shape);
        }
        else
        {
            gm.updateMovement(shape);
        }

    }

    private Sprite loadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;

        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        return null;
    }

    public IEnumerator downloadAndSaveSquare()
    {

        string pathToSaveIn = Application.persistentDataPath;

        storage = FirebaseStorage.DefaultInstance;

        // Create local filesystem URL

        string filename = Application.persistentDataPath + "/square.png";

        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://aidanharringtonunity.appspot.com/square.png");

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(filename,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {
              // called periodically during the download
              Debug.Log(String.Format(
                "Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred,
                state.TotalByteCount
              ));
          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
            }
        });

        Debug.Log(filename);

        yield return new WaitUntil(() => task.IsCompleted);


        Sprite square = loadSprite(filename);
        GameObject.Find("Square(Clone)").transform.localScale = new Vector2(0.1f, 0.1f);
        GameObject.Find("Square(Clone)").GetComponent<SpriteRenderer>().sprite = square;

        yield return null;
    }
    
    public IEnumerator downloadAndSaveCircle()
    {

        string pathToSaveIn = Application.persistentDataPath;

        storage = FirebaseStorage.DefaultInstance;

        // Create local filesystem URL

        string filename = Application.persistentDataPath + "/circle.png";

        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://aidanharringtonunity.appspot.com/circle.png");

        // Start downloading a file
        Task task = storage_ref.GetFileAsync(filename,
          new Firebase.Storage.StorageProgress<DownloadState>((DownloadState state) => {
              // called periodically during the download
              Debug.Log(String.Format(
                "Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred,
                state.TotalByteCount
              ));
          }), CancellationToken.None);

        task.ContinueWith(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Download finished.");
            }
        });

        Debug.Log(filename);

        yield return new WaitUntil(() => task.IsCompleted);

        Sprite circle = loadSprite(filename);
        GameObject.Find("Circle(Clone)").transform.localScale = new Vector2(0.1f, 0.1f);
        GameObject.Find("Circle(Clone)").GetComponent<SpriteRenderer>().sprite = circle;

        yield return null;
    }
}
