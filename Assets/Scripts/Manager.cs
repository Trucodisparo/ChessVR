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

    private GameObject player, exitButton, middleText;

    public KeywordRecognizer m_Recognizer;

    PhotonView view;

    public GameObject settingsManager;

    private bool started;
    private bool paused;
    private bool ended;

    // Start is called before the first frame update
    void Start()
    {
        settingsManager = GameObject.Find("SettingsManager");
        settingsManager.GetComponent<MainMenu>().enableVR();

        view = GetComponent<PhotonView>();

        started = false;
        paused = false;
        ended = false;

        m_Keywords = new string[65];
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

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

        //Preparamos la GUI
        middleText = GameObject.Find("MiddleText");
        exitButton = GameObject.Find("Salir");
        exitButton.GetComponent<Button>().onClick.AddListener(exitGame);
        exitButton.SetActive(false);
        
        turn = "Black";
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsConnected){
            if(!started && PhotonNetwork.CurrentRoom.PlayerCount == 2) startGame();
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1 && started) endGame("Victoria por abandono");
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

        if(!paused){
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
                        pieceToMove = GameObject.Find(args.text).GetComponent<Square>().piece;
                        pieceToMove.GetComponent<Renderer>().material = pieceToMove.GetComponent<Ficha>().Selected;
                    }
                }
                else{
                    pieceToMove.GetComponent<Renderer>().material = pieceToMove.GetComponent<Ficha>().unSelected;
                    view.RPC("makeCommand", RpcTarget.All, pieceToMove.GetComponent<Ficha>().currentSquare.name, args.text);
                }
            }
        }
    }

    [PunRPC]
    private void makeCommand(string pieceToMoveSquare, string square){
        GameObject piece = GameObject.Find(pieceToMoveSquare).GetComponent<Square>().piece;
        if(piece.GetComponent<Ficha>().commandIssued(square)) newTurn();
    }

    public void newTurn(){
        pieceToMove = null;
        if(turn == "White") turn = "Black";
        else turn = "White";

        GameObject.Find("TurnUI").GetComponent<TextMeshProUGUI>().text = "Turno: " + turn;

        //si no le toca al jugador de nuestra sesión, no grabamos.
        if(turn == player.GetComponent<Player>().color) m_Recognizer.Start();
        else if(m_Recognizer.IsRunning) m_Recognizer.Stop();

        GameObject king = GameObject.Find(turn+"King");
        if(king == null){
            if(turn == "White") endGame("Victoria de Black");
            else if(turn == "Black") endGame("Victoria de White");
            if(m_Recognizer.IsRunning) m_Recognizer.Stop();
        }
    }

    public void setPlayer(GameObject player){
        this.player = player;
    }

    private void endGame(string message){
        middleText.GetComponent<TextMeshProUGUI>().text = message;
        middleText.SetActive(true);
        exitButton.SetActive(true);
        ended = true;
    }

    private void exitGame(){
        StartCoroutine(BackToMain());
    }

    IEnumerator BackToMain(){
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
            yield return null;
        Debug.Log("Disconnected from Photon");
        Destroy(GameObject.Find("SettingsManager"));
        SceneManager.LoadScene(0);
    }

    private void pause(){
        paused = true;
        player.GetComponent<SC_FPSController>().enabled = false;
        if(m_Recognizer.IsRunning) m_Recognizer.Stop();
        middleText.GetComponent<TextMeshProUGUI>().text = "En pausa";
        middleText.SetActive(true);
        exitButton.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void unpause(){
        paused = false;
        player.GetComponent<SC_FPSController>().enabled = true;
        if(turn == player.GetComponent<Player>().color && started) m_Recognizer.Start();
        exitButton.SetActive(false);

        if(!started) middleText.GetComponent<TextMeshProUGUI>().text = "Esperando a un contrincante...";
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
