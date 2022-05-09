using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject ogPiece;

    public GameObject piece;

    public Vector2 matrixPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(ogPiece != null){
            piece = Instantiate(ogPiece,transform.position,ogPiece.transform.rotation);
            piece.GetComponent<Ficha>().setPosition(matrixPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hasPiece(){
       if(piece != null){
           return true;
       }
       else return false;
    }

    public bool hasEnemy(GameObject proposedPiece){
        if (hasPiece() && piece.GetComponent<Ficha>().pieceColor != proposedPiece.GetComponent<Ficha>().pieceColor) return true;
        else return false;
    }

    public void newPiece(GameObject proposedPiece){
        if(piece != null) Destroy(piece);
        piece = proposedPiece;
    }
}
