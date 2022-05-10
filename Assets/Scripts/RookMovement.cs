using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RookMovement : Ficha
{
    private enum Direction{NE,SE,NW,SW, NOT_VALID}; //Dirección del movimiento

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public override bool isLegalMove(GameObject square = null, bool hasEnemy = false){
        if(square == null) square = desiredMove;
        Vector2 destination = square.GetComponent<Square>().matrixPosition;

        if(getDirection(destination) == Direction.NOT_VALID) return false;

        if(!square.GetComponent<Square>().hasAlly(this.gameObject))
            if(!pathBlocked(destination))
                return true;

        return false;
    }

    private bool pathBlocked(Vector2 destination){
        Direction direction = getDirection(destination);
        GameObject board = GameObject.Find("Tablero");
        GameObject square;

        int i,j;

        switch(direction){
            case Direction.NE:
                for(i=(int)position.y+1, j = (int)position.x+1;i<destination.y && j<destination.x; i++, j++){
                        square = getSquare(board.GetComponent<Board>().squares[j-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.SE:
                for(i=(int)position.y-1, j = (int)position.x+1;i>destination.y && j<destination.x; i--, j++){
                        square = getSquare(board.GetComponent<Board>().squares[j-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.NW:
                for(i=(int)position.y+1, j = (int)position.x-1;i<destination.y && j>destination.x; i++, j--){
                        square = getSquare(board.GetComponent<Board>().squares[j-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
            case Direction.SW:
                for(i=(int)position.y-1, j = (int)position.x-1;i>destination.y && j>destination.x; i--, j--){
                        square = getSquare(board.GetComponent<Board>().squares[j-1].name[i-1]); // position [x,y] = array [x-1,y-1]
                        if(square.GetComponent<Square>().hasPiece()) return true;
                }
                break;
        }

        return false;
    }

    private Direction getDirection(Vector2 destination){
        Direction direction;
        float x_dist, y_dist;
        x_dist = destination.x - position.x;
        y_dist = destination.y - position.y;

        if(x_dist > 0 && y_dist > 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.NE;
        else if(x_dist > 0 && y_dist < 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.SE;
        else if(x_dist < 0 && y_dist > 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.NW;
        else if(x_dist < 0 && y_dist < 0 && Math.Abs(x_dist) == Math.Abs(y_dist)) direction = Direction.SW;
        else direction = Direction.NOT_VALID;

        return direction;
    }

}