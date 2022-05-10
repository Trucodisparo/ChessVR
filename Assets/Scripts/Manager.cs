using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using UnityEngine.UI;
using System;


public class Manager : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords;
    private string[] squares;
    private int squaresRecognized;

    public KeywordRecognizer m_Recognizer;
    // Start is called before the first frame update
    void Start()
    {
        m_Keywords = new string[65];
        GameObject board = GameObject.Find("Tablero");
        int k = 0;
        for(int i = 0; i<8; i++){
            for(int j=0;j<8;j++){
                m_Keywords[k] = board.GetComponent<Board>().squares[i].name[j];
                k++;
            }
        }
        m_Keywords[64] = "Cancelar";

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();

        squaresRecognized = 0;

        squares = new string[2];
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

        if(args.text == "Cancelar") squaresRecognized = 0;
        else{
            squares[squaresRecognized] = args.text;
            squaresRecognized++;
            Debug.Log(squaresRecognized + " // " + squares[squaresRecognized-1]);
            if(squaresRecognized == 2){
                squaresRecognized = 0;
                makeCommand();
            }
        }
    }

    private void makeCommand(){
        GameObject square = GameObject.Find(squares[0]);
        GameObject piece = square.GetComponent<Square>().piece;
        if(piece != null && piece.tag == "White"){
            piece.GetComponent<Ficha>().commandIssued(squares[1]);
        }
    }


}