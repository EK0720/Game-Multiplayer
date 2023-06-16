using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoorScript : MonoBehaviourPunCallbacks
{
    
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(collision2D.gameObject);
        }
        
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            string sceneName = "";
            if (gameObject.name == "door1") 
            {
                sceneName = "GameScene2"; 
            }
            else if (gameObject.name == "door2") 
            {
                sceneName = "GameScene3"; 
            }
            else if (gameObject.name == "door3") 
            {
                sceneName = "GameScene4";
            }
            NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, sceneName);
        }
    }
}