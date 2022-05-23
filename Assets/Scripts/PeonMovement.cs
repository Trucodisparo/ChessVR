using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PeonMovement : Ficha
{
    private bool firstMove;
    public GameObject promotion;

    // Start is called before the first frame update
    void Start()
    {
        firstMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool isLegalMove(GameObject square, bool hasEnemy = false){
        Vector2 firstMoveOK, moveOK, diagonalMoveOK;
        if(gameObject.tag == "Black"){
            firstMoveOK = new Vector2(0,-2);
            moveOK = new Vector2(0,-1);
            diagonalMoveOK = new Vector2(1,-1);
        }
        else{
            firstMoveOK = new Vector2(0,2);
            moveOK = new Vector2(0,1);
            diagonalMoveOK = new Vector2(1,1);
        }

        Vector2 destination = square.GetComponent<Square>().matrixPosition;
        Vector2 distance = destination - position;
        distance.x = Math.Abs(distance.x);

        if(hasEnemy == false) hasEnemy = (square.GetComponent<Square>().hasEnemy(this.gameObject));

        if((distance == diagonalMoveOK && hasEnemy)) return true;
        Debug.Log("Path blocked: " + pathBlocked(destination));
        if(!pathBlocked(destination)){
            if(firstMove)
                if((distance == firstMoveOK) || distance == moveOK) return true;
            else
                if(distance == moveOK) return true;
        }
        return false;
    }

    private void promote(){
        GameObject newPiece = Instantiate(promotion, transform.position, transform.rotation);
        newPiece.GetComponent<Ficha>().position = position;
        newPiece.GetComponent<Ficha>().currentSquare = currentSquare;
        currentSquare.GetComponent<Square>().piece = newPiece;
        Destroy(this.gameObject);
    }

    protected override IEnumerator StartMovement(float time, GameObject desiredMove)
    {
        float elapsedtime = 0;
        Vector3 pos_fin = desiredMove.transform.position;

        while(elapsedtime < time)
        {
            transform.position = Vector3.Lerp(transform.position, pos_fin, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        if(gameObject.tag == "Black" && position.y == 1) promote();
        else if(gameObject.tag == "White" && position.y == 8) promote();

    }

    private bool pathBlocked(Vector2 destination){
        GameObject board = GameObject.Find("Tablero");
        GameObject square;
        Debug.Log(destination);
        if(gameObject.tag == "White")
            for(int i=(int)position.y+1;i<=destination.y; i++){
                square = getSquare(board.GetComponent<Board>().squares[(int)position.x-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        else if(gameObject.tag == "Black")
            for(int i=(int)position.y-1;i>=destination.y; i--){
                square = getSquare(board.GetComponent<Board>().squares[(int)position.x-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        return false;
    }
}