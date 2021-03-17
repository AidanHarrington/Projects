using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Text WinningStatus;
    public static Text player1Username, player2Username;
    public static string username, username2;

    private Image player1, player2;
    public Sprite green, yellow;
    private Scene current;

    // Start is called before the first frame update
    void Start()
    {
        current = SceneManager.GetActiveScene();

        if (current.name.Equals("Game"))
        {
            WinningStatus = GameObject.Find("WinningStatus").GetComponent<Text>();

            //Token Change sprite
            player1 = GameObject.Find("Player1").GetComponent<Image>();
            player2 = GameObject.Find("Player2").GetComponent<Image>();

            player1Username = GameObject.Find("Player1Username").GetComponent<Text>();
            player2Username = GameObject.Find("Player2Username").GetComponent<Text>();

            if (NetworkLayer.player1Colour == 1)
            {
                player1.sprite = green;
            }
            else if (NetworkLayer.player1Colour == 2)
            {
                player1.sprite = yellow;
            }

            if (NetworkLayer.player2Colour == 1)
            {
                player2.sprite = green;
            }
            else if(NetworkLayer.player2Colour == 2)
            {
                player2.sprite = yellow;
            }
        }
        else
        {
            player1Username = GameObject.Find("Null1").GetComponent<Text>();
            player2Username = GameObject.Find("Null2").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This is going to show who is the winner of the game
    /// </summary>
    /// <param name="status">player1/player2 as the winner</param>
    public void ChangeStatusText(string status)
    {
        WinningStatus.text = status;
    }

    private void OnGUI()
    {
        player1Username.text = "Player 1: " + username;
        player2Username.text = "Player 2: " + username2;
    }
}
