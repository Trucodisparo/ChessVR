using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonMovement : MonoBehaviour
{
    public GameObject C1;
    Vector3 pos_ini;

    // Start is called before the first frame update
    void Start()
    {
        pos_ini = gameObject.transform.position;    
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("z"))
        {
            Debug.Log("Hola");
            StartCoroutine(StartMovement(1f));
            pos_ini = gameObject.transform.position;
        }    
    }


    private IEnumerator StartMovement(float time)
    {
        float elapsedtime = 0;
        Vector3 pos_fin = C1.transform.position;
        pos_fin.y = pos_ini.y;

        while(elapsedtime < time)
        {
            transform.position = Vector3.Lerp(pos_ini, pos_fin, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }
    }
}
