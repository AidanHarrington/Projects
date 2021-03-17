using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    //Dice
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    public static float timer1 = 10f, timer2 = 10f;
    public static int finalSide;
    public int randomDiceNo;

    //cheats
    private float holdTime;
    public Button win;
    public InputField overRide;
    public static bool isWon;

    //turn based
    public static bool play1 = true, play2 = false;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        diceSides = Resources.LoadAll<Sprite>("");

        win.gameObject.SetActive(false);
        overRide.gameObject.SetActive(false);

        isWon = false;
    }

    private void Update()
    {
        if (play1 && timer1 > 0f)
        {
            setTime1();
        }
        else if (play1 && timer1 <= 0f)
        {
            StartCoroutine(RollDice());
            timer1 = 10f;
        }
        else if (!play1)
        {
            timer1 = 10f;
        }

        if (play2 && timer2 > 0f)
        {
            setTime2();
        }
        else if (play2 && timer2 <= 0f)
        {
            StartCoroutine(RollDice());
            timer2 = 10f;
        }
        else if (!play2)
        {
            timer2 = 10f;
        }

        //Enter Cheats
        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
        }
        else
        {
            holdTime = 0;
        }

        if (holdTime >= 5)
        {
            win.gameObject.SetActive(true);
            overRide.gameObject.SetActive(true);
        }

    }

    public void setTime1()
    {
        if (timer1 > 0 && !isWon)
        {
            timer1 -= Time.deltaTime;
        }
        else if (isWon)
        {
            timer1 = 10;
        }
        else
        {
            StartCoroutine(RollDice());
            timer1 = 10f;
        }
    }

    public void setTime2()
    {
        if (timer2 > 0 && !isWon)
        {
            timer2 -= Time.deltaTime;
        }
        else if (isWon)
        {
            timer2 = 10;
        }
        else
        {
            StartCoroutine(RollDice());
            timer2 = 10f;
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(RollDice());
    }

    public void Win()
    {
        isWon = true;
        NetworkLayer.MoveFinish(NetworkManager.MyGamePlayerId);
        StopAllCoroutines();
    }

    private IEnumerator RollDice()
    {
        //timer1 = 10f;
        randomDiceNo = 0;
        finalSide = 0;

        if (overRide.text == "")
        {
            for (int i = 0; i <= 20; i++)
            {
                randomDiceNo = Random.Range(1, 7);
                //randomDiceNo = 6;
                rend.sprite = diceSides[randomDiceNo - 1];
                yield return new WaitForSeconds(0.05f);

            }

            finalSide = randomDiceNo;

            NetworkLayer.Turn(NetworkManager.MyGamePlayerId, randomDiceNo);

            Debug.Log(NetworkManager.MyGamePlayerId);
        }
        else
        {
            for (int i = 0; i <= 20; i++)
            {
                randomDiceNo = int.Parse(overRide.text);
                rend.sprite = diceSides[randomDiceNo - 1];
                yield return new WaitForSeconds(0.05f);

            }

            finalSide = randomDiceNo;

            NetworkLayer.Turn(NetworkManager.MyGamePlayerId, randomDiceNo);

            Debug.Log(NetworkManager.MyGamePlayerId);
        }

        if (play1 == true && randomDiceNo == 6)
        {
            timer1 = 10;
        }
        else if (play2 && randomDiceNo == 6)
        {
            timer2 = 10;
        }
    }
}
