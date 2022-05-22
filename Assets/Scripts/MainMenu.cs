using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using UnityEngine.Windows.Speech;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_Recognizer;
    // Start is called before the first frame update
    void Start()
    {
        m_Keywords = new string[2];
        m_Keywords[0] = "Jugar";
        m_Keywords[1] = "Salir";

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
    }

    public void EndGame(){
        Application.Quit();
    }


     private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        switch(args.text)
        {
            case "Jugar": StartGame(); break;
            case "Salir": EndGame(); break;
            default: break;
        }

    }

}
