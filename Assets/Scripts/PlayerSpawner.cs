using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject whitePlayer;
    public GameObject blackPlayer;

    Vector3 whiteSpawn = new Vector3(-0.2f,1.75f,-17.8f);
    Vector3 blackSpawn = new Vector3(-0.2f,1.75f,17.8f);

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            GameObject.Find("GameManager").GetComponent<Manager>().setPlayer(PhotonNetwork.Instantiate(whitePlayer.name, whiteSpawn, Quaternion.identity));
        else if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
            GameObject.Find("GameManager").GetComponent<Manager>().setPlayer(PhotonNetwork.Instantiate(blackPlayer.name, blackSpawn, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}