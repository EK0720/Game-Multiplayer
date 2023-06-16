using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int numPlayers;
    public static NetworkManager instance;
    public void Awake()
    {
        if(instance != null&& instance != this){
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("you joined master server");
    }

    public void CreateRooms(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)numPlayers;

        PhotonNetwork.CreateRoom(roomName, options);
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("you joined room");
    }

    
    [PunRPC] public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
