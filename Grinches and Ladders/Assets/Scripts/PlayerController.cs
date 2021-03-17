using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private List<GameObject> pathList = new List<GameObject>();
    public GameObject paths;
    public int currentPos;
    public Text timer1;
    public Text timer2;

    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < paths.transform.childCount + 1; i++)
        {
            pathList.Add(GameObject.Find("Path" + i));
        }

        currentPos = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timer1.text = "Player 1 Time: " + Dice.timer1.ToString("F0");
        timer2.text = "Player 2 Time: " + Dice.timer2.ToString("F0");
    }

    public void Move(int numberofSteps)
    {
        StartCoroutine(MoveToPath(numberofSteps));
    }

    public void win(string playerName)
    {
        StartCoroutine(MoveToFinish());
    }

    IEnumerator MoveToFinish()
    {
        Vector2 Finish = GameObject.Find("Path70").transform.position;
        yield return StartCoroutine(Lerping(Finish, 1f));
        
        currentPos = 70;
    }

    IEnumerator MoveToPath(int numberofSteps)
    {
        for (int i = currentPos; i < currentPos + numberofSteps; i++)
        {
            if (currentPos + numberofSteps >= 71)
            //if the dice roll is bigger than  path60, he wont move
            {
                numberofSteps = 0;
                break;
            }

            else if (currentPos + numberofSteps <= 70)
            {
                GameObject path = pathList[i];
                yield return StartCoroutine(Lerping(path.transform.position, 1f));

            }
        }

        currentPos += numberofSteps;

        switch (currentPos)
        {
            //ladders
            case 7:
                Vector2 ladder1 = GameObject.Find("Path13").transform.position;
                yield return StartCoroutine(Lerping(ladder1, 1f));
                currentPos = 13;
                break;
            case 25:
                Vector2 ladder2 = GameObject.Find("Path66").transform.position;
                yield return StartCoroutine(Lerping(ladder2, 1f));
                currentPos = 66;
                break;
            case 37:
                Vector2 ladder3 = GameObject.Find("Path63").transform.position;
                yield return StartCoroutine(Lerping(ladder3, 1f));
                currentPos = 63;
                break;
            case 51:
                Vector2 ladder4 = GameObject.Find("Path70").transform.position;
                yield return StartCoroutine(Lerping(ladder4, 1f));
                currentPos = 70;
                break;
            case 60:
                Vector2 ladder5 = GameObject.Find("Path61").transform.position;
                yield return StartCoroutine(Lerping(ladder5, 1f));
                currentPos = 61;
                break;

            //grinches
            case 19:
                Vector2 snake1 = GameObject.Find("Path5").transform.position;
                yield return StartCoroutine(Lerping(snake1, 1f));
                currentPos = 5;
                break;
            case 32:
                Vector2 snake2 = GameObject.Find("Path15").transform.position;
                yield return StartCoroutine(Lerping(snake2, 1f));
                currentPos = 15;
                break;
            case 68:
                Vector2 snake3 = GameObject.Find("Path23").transform.position;
                yield return StartCoroutine(Lerping(snake3, 1f));
                currentPos = 23;
                break;
            case 62:
                Vector2 snake4 = GameObject.Find("Path56").transform.position;
                yield return StartCoroutine(Lerping(snake4, 1f));
                currentPos = 56;
                break;
        }
                
    }

    IEnumerator Lerping(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "finish")
        {
            print("Finished:" + gameObject.name);
            if(NetworkManager.MyGamePlayerId == gameObject.name) //if I won the game inform everyone
            {
                NetworkLayer.WinningStatus(NetworkManager.MyGamePlayerId); //inform everyone that this player won the game
            }
        }
    }
}
