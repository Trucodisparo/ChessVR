using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMovement : Ficha
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool isLegalMove(GameObject square, bool hasEnemy = false)
    {
        Vector2 destination = square.GetComponent<Square>().matrixPosition;

        if((Abs(destination - position) == new Vector2(1,2)) || (Abs(destination - position) == new Vector2(2, 1)))
        {
            if(square.GetComponent<Square>().hasEnemy(this.gameObject) || !square.GetComponent<Square>().hasPiece())    // Se puede mover si hay un enemigo o está vacía la casilla desitno.
            {
                return true;
            }
        }


        return false;
    }


     protected override IEnumerator StartMovement(float time, GameObject desiredMove)
    {
        float elapsedtime = 0;
        Vector3 pos_fin = desiredMove.transform.position;
        pos_fin.z = transform.position.z;
        Vector3 pos_fin2 = desiredMove.transform.position;


        while(elapsedtime < time/2)
        {
            transform.position = Vector3.Lerp(transform.position, pos_fin, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        elapsedtime = 0;


         while(elapsedtime < time/2)
        {
            transform.position = Vector3.Lerp(transform.position, pos_fin2, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }
    }


/*
    public override void move(GameObject desiredMove){
        currentSquare.GetComponent<Square>().pieceMoved();  // Como la pieza se mueve, la posición antigua queda a null, sin pieza.
        desiredMove.GetComponent<Square>().newPiece(this.gameObject);   // A la posición destino se le guarda la ficha que realizará el movimiento hacia ella.
        setPosition(desiredMove.GetComponent<Square>().matrixPosition);
        currentSquare = desiredMove;
        GameObject desiredmoved1 = desiredMove;
        GameObject desiredmoved2 = desiredMove;
        desiredmoved1.transform.position = new Vector3(desiredMove.transform.position.x,desiredMove.transform.position.y,currentSquare.transform.position.z);
        desiredmoved2.transform.position = new Vector3(currentSquare.transform.position.x,desiredMove.transform.position.y,desiredMove.transform.position.z);

        if(Abs(currentSquare.GetComponent<Square>().matrixPosition - desiredMove.GetComponent<Square>().matrixPosition) == new Vector2(2,1))
        {
            StartCoroutine(StartMovement(1f, desiredmoved1));   // Primero, movimiento en vertical.
            StartCoroutine(StartMovement(1f, desiredMove));     // Luego, pasa a la posición final.
        }
        else
        {
            StartCoroutine(StartMovement(1f, desiredmoved2));   // Primero, movimiento en horizontal.
            StartCoroutine(StartMovement(1f, desiredMove));     // Luego, pasa a la posición final.
        }     
    }
    */
}
