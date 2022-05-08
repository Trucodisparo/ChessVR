using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonMovement : MonoBehaviour
{
    private bool firstMove;
    private GameObject desiredMove;

    // Start is called before the first frame update
    void Start()
    {
        firstMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z"))
        {
            Debug.Log("z");
            StartCoroutine(StartMovement(1f));
        }    
    }


    private IEnumerator StartMovement(float time)
    {
        float elapsedtime = 0;
        Vector3 pos_fin = getSquarePosition("C2");
        pos_fin.y = transform.position.y;

        while(elapsedtime < time)
        {
            transform.position = Vector3.Lerp(transform.position, pos_fin, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }
    }

    private Vector3 getSquarePosition(string square){
        desiredMove = GameObject.Find(square);
        return desiredMove.transform.position;
    }

    private bool isLegalMove(Vector3 destination){
        if((Abs(destination - transform.position) == new Vector3(1,0,1)) && (desiredMove.GetComponent<Square>().hasEnemy(this.gameObject))) return true;
        if(firstMove)
            if(Abs(destination - transform.position) == new Vector3(0,0,2) || Abs(destination - transform.position) == new Vector3(0,0,2)) return true;
        else
            if(Abs(destination - transform.position) == new Vector3(0,0,2)) return true;
        
        return false;
    }

    private Vector3 Abs(Vector3 vector){
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    private Vector3 move(){
        Square.GetComponent<Square>().newPiece(this.gameObject);
    }
}
