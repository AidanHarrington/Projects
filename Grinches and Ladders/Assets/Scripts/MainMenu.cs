using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static Button green, yellow;
    private bool isChosen;
    public static InputField inputUser;
    public Text incorrect;

    public void Start()
    {
        green = GameObject.Find("Green").GetComponent<Button>();
        yellow = GameObject.Find("Yellow").GetComponent<Button>();
        inputUser = GameObject.Find("UserInput").GetComponent<InputField>();
        isChosen = false;
        incorrect.gameObject.SetActive(false);
    }

    public void setYellow()
    {
        isChosen = true;
        NetworkLayer.chooseColour(NetworkManager.MyGamePlayerId, "Yellow");
    }

    public void setGreen()
    {
        isChosen = true;
        NetworkLayer.chooseColour(NetworkManager.MyGamePlayerId, "Green");
    }

    public void setUsername()
    {

        NetworkLayer.getUsername(NetworkManager.MyGamePlayerId, inputUser.text);
    }

    public void playGame()
    {
        if(inputUser.text != "" && isChosen == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (inputUser.text == "" && isChosen == false)
        {
            incorrect.gameObject.SetActive(true);
        } 
    }

}
