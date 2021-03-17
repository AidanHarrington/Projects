using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GUIStyle TextStyle = new GUIStyle();

    public static string MyGamePlayerId = "";

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJoinedLobby()
    {
        //when connected to lobby auto join random room
        PhotonNetwork.JoinRandomRoom();        
    }

    void OnPhotonRandomJoinFailed()
    {
        //if room does not exist, create one automatically
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        print("Connected to room");
        int numberOfPlayers = PhotonNetwork.playerList.Length;
        print("Number Of Players:" + numberOfPlayers);
        
        MyGamePlayerId = "Player" + numberOfPlayers;        
        
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString()+":"+MyGamePlayerId,TextStyle);
    }
}
