using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string PlayerPrefabPath1;
    public string PlayerPrefabPath2;    
    public Transform[] spawnPoints;
    public float respawnTime;
    private int playersInGame;
    //instance
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if(playersInGame ==PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }
    [PunRPC]
     public void RespawnPlayer()
    {
        playersInGame = 0;
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        string playerPrefabPath = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? PlayerPrefabPath1 : PlayerPrefabPath2;
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0,spawnPoints.Length )].position, Quaternion.identity);
    }
    
}
