using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject ogPiece;

    public GameObject piece;

    public Vector2 matrixPosition;

    public AudioSource audiosource;
    public AudioClip soundDestroyed;

    public GameObject particlesWhite, particlesBlack;

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

            //Instanciar particulas
            GameObject particles;
            if(piece.tag == "White") particles = particlesWhite;
            else particles = particlesBlack;
            Instantiate(particles, piece.transform.position, piece.transform.rotation);
            if (particles.GetComponent<ParticleSystem>().isStopped)
                particles.GetComponent<ParticleSystem>().Play();
            //Reproducimos sonido de muerte de ficha
            audiosource.clip = soundDestroyed;
            audiosource.Play();
            //Destruimos la ficha
            Destroy(piece);
        }
        piece = proposedPiece;
        piece.GetComponent<Ficha>().currentSquare = this.gameObject;
    }

    public void pieceMoved(){
        piece = null;
    }
}