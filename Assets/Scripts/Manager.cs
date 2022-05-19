using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using UnityEngine.UI;
using System;
using TMPro;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords;
    private GameObject pieceToMove;
    private string turn;

    private GameObject player;

    public KeywordRecognizer m_Recognizer;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();

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
        
        turn = "Black";
        newTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

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
            Debug.Log(turn + " lost");
            if(m_Recognizer.IsRunning) m_Recognizer.Stop();
        }
    }

    public void setPlayer(GameObject player){
        this.player = player;
    }


}
