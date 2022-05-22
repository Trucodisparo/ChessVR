using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    PhotonView view;
    public string color;
    GameObject vrSettings;

    // Start is called before the first frame update
    void Start()
    {
        vrSettings = GameObject.Find("SettingsManager");
        view = GetComponent<PhotonView>();
        if(!(view.IsMine)) {
            this.gameObject.GetComponent<SC_FPSController>().enabled = false;
            this.gameObject.transform.GetChild(2).gameObject.SetActive(false); //desactivamos camara de los otros jugadores
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false); //desactivamos xrrig de los otros jugadores
        }
        if(view.IsMine && !vrSettings.GetComponent<MainMenu>().VR){ //por defecto el jugador usa el xrrig, no la cámara
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true); 
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false); 
            GameObject.Find("GUI").GetComponent<Canvas>().worldCamera = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Camera>(); // Renderizamos la UI en la cámara
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
