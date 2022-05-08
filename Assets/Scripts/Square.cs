using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject piece;

    // Start is called before the first frame update
    void Start()
    {
        if(piece != null)
            Instantiate(piece,transform.position,transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hasEnemy(GameObject proposedPiece){
       if(piece != null){
           if (piece.GetComponent<Ficha>().pieceColor != proposedPiece.GetComponent<Ficha>().pieceColor) return true;
           else return false;
       }
       else return false;
    }

    public void newPiece(GameObject proposedPiece){
        if(piece != null) Destroy(piece);
        piece = proposedPiece;
    }
}
