using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QueenMovement : MonoBehaviour
{
    private GameObject desiredMove;
    private Ficha ficha;

    private enum Direction{N,E,S,W,NE,SE,NW,SW, NOT_VALID}; //Dirección del movimiento

    // Start is called before the first frame update
    void Start()
    {
        ficha = this.GetComponent<Ficha>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z"))
        {
            Debug.Log("z");
            string square = "B4";
            commandIssued(square);
        }

        if(Input.GetKeyDown("x"))
        {
            Debug.Log("x");
            string square = "B7";
            commandIssued(square);
        }

        if(Input.GetKeyDown("c"))
        {
            Debug.Log("c");
            string square = "H5";
            commandIssued(square);
        }
            
    }

    void commandIssued(string square){
        desiredMove = getSquare(square);
        if(isLegalMove()){
            ficha.move(desiredMove);
        }
        else{
            Debug.Log("Illegal move: " + desiredMove.GetComponent<Square>().matrixPosition + " // " + ficha.position);
        }
    }

    private GameObject getSquare(string square){
        return GameObject.Find(square);
    }

    private bool isLegalMove(){
        Vector2 destination = desiredMove.GetComponent<Square>().matrixPosition;

        if(getDirection(destination) == Direction.NOT_VALID) return false;

        if( !pathBlocked(destination))
            return true;

        return false;
    }

    private Vector2 Abs(Vector2 vector){
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    private bool pathBlocked(Vector2 destination){
        Direction direction = getDirection(destination);
        GameObject board = GameObject.Find("Tablero");
        GameObject square;

        switch(direction){
            case Direction.N:
                for(int i=(int)ficha.position.y+1;i<destination.y; i++){
                    square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)ficha.position.x-1]); // position [x,y] = array [x-1,y-1]
                    if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.S:
                for(int i=(int)ficha.position.y-1;i>destination.y; i--){
                    square = getSquare(board.GetComponent<Board>().squares[i-1].name[(int)ficha.position.x-1]); // position [x,y] = array [x-1,y-1]
                    if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.E:
                for(int j=(int)ficha.position.x+1;j<destination.x; j++){
                    square = getSquare(board.GetComponent<Board>().squares[(int)ficha.position.y-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                    if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.W:
                for(int j=(int)ficha.position.x-1;j>destination.x; j--){
                    square = getSquare(board.GetComponent<Board>().squares[(int)ficha.position.y-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                    if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.NE:
                for(int i=(int)ficha.position.y+1;i<destination.y; i++)
                    for(int j=(int)ficha.position.x+1;j<destination.x; j++){
                        square = getSquare(board.GetComponent<Board>().squares[i-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.SE:
                for(int i=(int)ficha.position.y-1;i>destination.y; i--)
                    for(int j=(int)ficha.position.x+1;j<destination.x; j++){
                        square = getSquare(board.GetComponent<Board>().squares[i-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.NW:
                for(int i=(int)ficha.position.y+1;i<destination.y; i++)
                    for(int j=(int)ficha.position.x-1;j<destination.x; j--){
                        square = getSquare(board.GetComponent<Board>().squares[i-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.SW:
                for(int i=(int)ficha.position.y-1;i>destination.y; i--)
                    for(int j=(int)ficha.position.x-1;j<destination.x; j--){
                        square = getSquare(board.GetComponent<Board>().squares[i-1].name[j-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
        }

        return false;
    }

    private Direction getDirection(Vector2 destination){
        Direction direction;
        float x_dist, y_dist;
        x_dist = destination.x - ficha.position.x;
        y_dist = destination.y - ficha.position.y;

        if(x_dist == 0 && y_dist > 0) direction = Direction.N;
        else if(x_dist == 0 && y_dist < 0) direction = Direction.S;
        else if(x_dist > 0 && y_dist == 0) direction = Direction.E;
        else if(x_dist < 0 && y_dist == 0) direction = Direction.W;
        else if(x_dist > 0 && y_dist > 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.NE;
        else if(x_dist > 0 && y_dist < 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.SE;
        else if(x_dist < 0 && y_dist > 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.NW;
        else if(x_dist < 0 && y_dist < 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.SW;
        else direction = Direction.NOT_VALID;

        return direction;
    }

}