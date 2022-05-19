using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    PhotonView view;
    public string color;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if(!(view.IsMine)) {
            this.gameObject.GetComponent<SC_FPSController>().enabled = false;
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newTurn(string turn){
        if (turn == color){

        }
    }
}
