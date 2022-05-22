using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using UnityEngine.Windows.Speech;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.UI;
using UnityEngine.Subsystems;

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

    public  bool VR = false;

    XRDisplaySubsystem display;

    void Awake()
    {
        //StartCoroutine(StopXR());
        Debug.Log(display);
        StartCoroutine(StopXR());
        DontDestroyOnLoad(this.gameObject);
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

    public void switchVR(){
        VR = !VR;
    }

    public void VRButton(){
        switchVR();
        if(VR){
            GameObject.Find("VRText").GetComponent<Text>().text = "VR: ON";
        }
        else{
            GameObject.Find("VRText").GetComponent<Text>().text = "VR: OFF";
        }

    }

    public void enableVR(){
        if(!VR)
            StartCoroutine(StopXR());
        else
            StartCoroutine(InitXR());
    }

     public IEnumerator InitXR()
    {
        if(display == null) loadXR("oculus");
        else display.Start();
        yield return null;
    }

    public IEnumerator StopXR()
    {
        if(display != null) display.Stop();
        yield return null;
    }

    void loadXR(string device)
    {
        List<XRDisplaySubsystemDescriptor> displays = new List<XRDisplaySubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(displays);
        Debug.Log("Number of display providers found: " + displays.Count);

        foreach (var d in displays)
        {
            Debug.Log("Scanning display id: " + d.id);

            if (d.id.Contains(device))
            {
                Debug.Log("Creating display " + d.id);
                XRDisplaySubsystem dispInst = d.Create();

                if (dispInst != null)
                {
                    Debug.Log("Starting display " + d.id);
                    dispInst.Start();
                    display = dispInst;
                }
            }
        }
    }
}