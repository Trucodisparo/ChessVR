using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject ogPiece;

    public GameObject piece;

    public Vector2 matrixPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if(ogPiece != null){
            piece = Instantiate(ogPiece,transform.position,ogPiece.transform.rotation);
            piece.GetComponent<Ficha>().setPosition(matrixPosition);
            piece.GetComponent<Ficha>().currentSquare = this.gameObject;
            piece.name = ogPiece.name;
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
        if (hasPiece() && piece.tag != proposedPiece.tag) return true;
        else return false;
    }

    public bool hasAlly(GameObject proposedPiece){
        if (hasPiece() && piece.tag == proposedPiece.tag) return true;
        else return false;
    }

    public void newPiece(GameObject proposedPiece){
        if(piece != null) 
        {
            //Se tarda demasiado en destruir la pieza, así que notificamos al manager si se ha asesinado a un rey para acabar el juego
            if(piece.name == "BlackKing") GameObject.Find("GameManager").gameObject.GetComponent<Manager>().endGame("Victoria para White");
            else if (piece.name == "WhiteKing") GameObject.Find("GameManager").gameObject.GetComponent<Manager>().endGame("Victoria para Black");
            piece.GetComponent<Ficha>().Die();
            Destroy(piece);   
        }
        piece = proposedPiece;
        piece.GetComponent<Ficha>().currentSquare = this.gameObject;
    }

    public void pieceMoved(){
        piece = null;
    }
}