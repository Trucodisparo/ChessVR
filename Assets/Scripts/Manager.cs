using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords;
    private GameObject pieceToMove;
    private string turn;

    private GameObject player, exitButton, middleText, continueButton;

    public KeywordRecognizer m_Recognizer;

    PhotonView view;

    public GameObject settingsManager;

    private bool started;
    private bool paused;
    private bool ended;
    
    public AudioSource audioPlayer;
    public AudioClip winning, losing, error;


    // Start is called before the first frame update
    void Start()
    {
        settingsManager = GameObject.Find("SettingsManager");
        settingsManager.GetComponent<MainMenu>().enableVR();

        view = GetComponent<PhotonView>();

        started = false;
        paused = false;
        ended = false;

        //Reconocedor de movimientos
        m_Keywords = new string[68];
        GameObject board = GameObject.Find("Tablero");
        int k = 0;

        //Cargar nombres de casillas en el recognizer
        for(int i = 0; i<8; i++){
            for(int j=0;j<8;j++){
                m_Keywords[k] = board.GetComponent<Board>().squares[i].name[j];
                k++;
            }
        }
        m_Keywords[64] = "Cancelar";
        m_Keywords[65] = "Salir";
        m_Keywords[66] = "Continuar";
        m_Keywords[67] = "Pausa";

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

        m_Recognizer.Start();

        //Preparamos la GUI
        middleText = GameObject.Find("MiddleText");

        continueButton = GameObject.Find("Continuar");
        continueButton.GetComponent<Button>().onClick.AddListener(unpause); //Botón de salir de pausa
        continueButton.SetActive(false);

        exitButton = GameObject.Find("Salir");
        exitButton.GetComponent<Button>().onClick.AddListener(exitGame); //Salimos si clickamos el botón

        turn = "Black";

        Destroy(settingsManager); //Eliminamos tras cargar la config, si lo hacemos al salir de la escena no da tiempo a destruirlo antes del cambio
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsConnected){
            if(!started && PhotonNetwork.CurrentRoom.PlayerCount == 2) startGame();
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1 && started && !ended){
                endGame("Victoria por abandono");
                //Tocamos sonido de victoria
                audioPlayer.clip = winning;
                audioPlayer.Play();
            }
        }

        //Pausamos el juego
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!ended){
                if(!paused){ //Si no está en pausa, pausamos
                    pause();
                }
                else{ //Si está en pausa, volvemos al juego
                    unpause();
                }
            }
        }
    }

    private void startGame(){
        middleText.SetActive(false);
        exitButton.SetActive(false);
        started = true;
        newTurn();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        if(!started || ended){
            if(args.text == "Salir") exitGame();
        }

        if(!paused){
            if(args.text == "Pausa") pause();
            else if(turn == player.GetComponent<Player>().color){ //Si no nos toca, no aceptamos inputs de movimientos (sólo pausa)
                if(args.text == "Cancelar" && pieceToMove != null)
                {
                    pieceToMove.GetComponent<Renderer>().material = pieceToMove.GetComponent<Ficha>().unSelected;
                    pieceToMove = null;
                } 
                else if(args.text == "Cancelar" && pieceToMove == null)
                {
                    pieceToMove = null;
                }
                else{
                    if(pieceToMove == null) 
                    {
                        if(GameObject.Find(args.text).GetComponent<Square>().piece != null){
                            if(GameObject.Find(args.text).GetComponent<Square>().piece.tag == turn){
                                pieceToMove = GameObject.Find(args.text).GetComponent<Square>().piece;
                                pieceToMove.GetComponent<Renderer>().material = pieceToMove.GetComponent<Ficha>().Selected;
                            }
                            else{
                                Debug.Log("No es tu pieza, tramposo!");
                                audioPlayer.clip = error;
                                audioPlayer.Play();
                            }
                        }
                    }
                    else{
                        view.RPC("makeCommand", RpcTarget.All, pieceToMove.GetComponent<Ficha>().currentSquare.name, args.text);
                    }
                }
            }
        }
        else{ //En menú de pausa
            if(args.text == "Salir") exitGame();
            if(args.text == "Continuar") unpause();
        }
    }

    [PunRPC]
    private void makeCommand(string pieceToMoveSquare, string square){
        GameObject piece = GameObject.Find(pieceToMoveSquare).GetComponent<Square>().piece;
        if(piece.GetComponent<Ficha>().commandIssued(square)) newTurn();
        else{ //Comando sin éxito, se vuelve a intentar
            if(piece.tag == player.GetComponent<Player>().color){ //Solo oimos el error si es nuestra ficha
                audioPlayer.clip = error;
                audioPlayer.Play();
            }
        }
    }

    public void newTurn(){
        if(pieceToMove != null)
            pieceToMove.GetComponent<Renderer>().material = pieceToMove.GetComponent<Ficha>().unSelected;
        pieceToMove = null;
        if(turn == "White") turn = "Black";
        else turn = "White";

        GameObject.Find("TurnUI").GetComponent<TextMeshProUGUI>().text = "Turno: " + turn;
    }

    public void setPlayer(GameObject player){
        this.player = player;
    }

    public void endGame(string message){
        middleText.GetComponent<TextMeshProUGUI>().text = message;
        GameObject.Find("TurnUI").gameObject.SetActive(false);
        middleText.SetActive(true);
        exitButton.SetActive(true);

        if(turn != player.GetComponent<Player>().color)
            audioPlayer.clip = losing;
        else
            audioPlayer.clip = winning;

        audioPlayer.Play();

        ended = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void exitGame(){
        m_Recognizer.Stop();
        m_Recognizer.Dispose();
        Destroy(GameObject.Find("BGMusicPlayer"));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(BackToMain());
    }

    IEnumerator BackToMain(){
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
            yield return null;
        Debug.Log("Disconnected from Photon");
        SceneManager.LoadScene(0);
    }

    private void pause(){
        paused = true;
        player.GetComponent<SC_FPSController>().enabled = false;
        middleText.GetComponent<TextMeshProUGUI>().text = "En pausa";
        middleText.SetActive(true);
        exitButton.SetActive(true);
        continueButton.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void unpause(){
        paused = false;
        player.GetComponent<SC_FPSController>().enabled = true;
        if(started) exitButton.SetActive(false);
        continueButton.SetActive(false);

        if(!started) middleText.GetComponent<TextMeshProUGUI>().text = "Esperando a un contrincante...";
        else middleText.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
