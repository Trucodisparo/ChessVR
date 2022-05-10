using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonMovement : Ficha
{
    private bool firstMove;
    private GameObject desiredMove;
    GameObject promotion;

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
            string square = "C2";
            desiredMove = getSquare(square);
            if(desiredMove != null){
                if(isLegalMove()){
                    move(desiredMove);
                        if(gameObject.tag == "Black" && position.y == 1) promote();
                        else if(gameObject.tag == "White" && position.y == 8) promote();
                }
                else{
                    Debug.Log("Illegal move: " + desiredMove.GetComponent<Square>().matrixPosition + " // " + position);
                }
            }
        }
    }

    private GameObject getSquare(string square){
        return GameObject.Find(square);
    }

    private bool isLegalMove(){
        Vector2 destination = desiredMove.GetComponent<Square>().matrixPosition;
        if((Abs(destination - position) == new Vector2(1,1)) && (desiredMove.GetComponent<Square>().hasEnemy(this.gameObject))) return true;
        if(!pathBlocked(destination)){
        if(firstMove)
            if((Abs(destination - position) == new Vector2(0,2)) || Abs(destination - position) == new Vector2(0,1)) return true;
        else
            if(Abs(destination - position) == new Vector2(0,1)) return true;
        }
        return false;
    }

    private void promote(){
        GameObject newPiece = Instantiate(promotion, transform.position, transform.rotation);
        newPiece.GetComponent<Ficha>().position = position;
        newPiece.GetComponent<Ficha>().currentSquare = currentSquare;
        Destroy(this.gameObject);
    }

    private bool pathBlocked(Vector2 destination){
        GameObject board = GameObject.Find("Tablero");
        GameObject square;
        if(gameObject.tag == "White")
            for(int i=(int)position.y+1;i<=destination.y; i++){
                square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)position.x-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        else if(gameObject.tag == "Black")
            for(int i=(int)position.y-1;i>=destination.y; i--){
                square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)position.x-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        return false;
    }
}