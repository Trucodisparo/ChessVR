using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingMovement : Ficha
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public override bool isLegalMove(GameObject square, bool hasEnemy = false){
        Vector2 destination = square.GetComponent<Square>().matrixPosition;
        
        if(!square.GetComponent<Square>().hasAlly(this.gameObject))
            if((Abs(destination - position) == new Vector2(1,0)) || (Abs(destination - position) == new Vector2(0,1))) 
                if(!isDangerous()) return true;

        return false;
    }

    private bool isDangerous(GameObject square = null){
        if(square == null) square = desiredMove;
        GameObject[] enemies;
        if(gameObject.tag == "White") enemies = GameObject.FindGameObjectsWithTag("Black");
        else enemies = GameObject.FindGameObjectsWithTag("White");
        foreach(GameObject enemy in enemies){
            if(enemy.GetComponent<Ficha>().isLegalMove(square, true)){
                Debug.Log("King would be in danger!");
                return true;
            }
        }
        return false;
    }

    public bool CheckMate(){
        GameObject board = GameObject.Find("Tablero");
        if(isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y].name[(int)position.x-1])) //North
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-2].name[(int)position.x-1])) //South
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-1].name[(int)position.x])) //East
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-1].name[(int)position.x-2]))) //West
            return true;
        else return false;
    }

}