using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonMovement : MonoBehaviour
{
    private bool firstMove;
    private GameObject desiredMove;
    private Ficha ficha;
    GameObject promotion;

    // Start is called before the first frame update
    void Start()
    {
        firstMove = true;
        ficha = this.GetComponent<Ficha>();
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
                    ficha.move(desiredMove);
                        if(ficha.pieceColor == Ficha.Color.Black && ficha.position.y == 1) promote(ficha.pieceColor);
                        else if(ficha.pieceColor == Ficha.Color.White && ficha.position.y == 8) promote(ficha.pieceColor);
                }
                else{
                    Debug.Log("Illegal move: " + desiredMove.GetComponent<Square>().matrixPosition + " // " + ficha.position);
                }
            }
        }
    }

    private GameObject getSquare(string square){
        return GameObject.Find(square);
    }

    private bool isLegalMove(){
        Vector2 destination = desiredMove.GetComponent<Square>().matrixPosition;
        if((Abs(destination - ficha.position) == new Vector2(1,1)) && (desiredMove.GetComponent<Square>().hasEnemy(this.gameObject))) return true;
        if(!pathBlocked(destination)){
        if(firstMove)
            if((Abs(destination - ficha.position) == new Vector2(0,2)) || Abs(destination - ficha.position) == new Vector2(0,1)) return true;
        else
            if(Abs(destination - ficha.position) == new Vector2(0,1)) return true;
        }
        return false;
    }

    private Vector2 Abs(Vector2 vector){
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    private void promote(Ficha.Color c){
        GameObject newPiece = Instantiate(promotion, transform.position, transform.rotation);
        newPiece.GetComponent<Ficha>().position = ficha.position;
        Destroy(this.gameObject);
    }

    private bool pathBlocked(Vector2 destination){
        GameObject board = GameObject.Find("Tablero");
        GameObject square;
        if(ficha.pieceColor == Ficha.Color.White)
            for(int i=(int)ficha.position.y+1;i<=destination.y; i++){
                square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)ficha.position.x-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        else if(ficha.pieceColor == Ficha.Color.Black)
            for(int i=(int)ficha.position.y-1;i>=destination.y; i++){
                square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)ficha.position.x-1]); // position [x,y] = array [x-1,y-1]
                if(square.GetComponent<Square>().hasPiece()) return true;
            }
        return false;
    }
}