using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KingMovement : Ficha
{
    private GameObject desiredMove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("a"))
        {
            Debug.Log("a");
            string square = "B4";
            commandIssued(square);
        }

        if(Input.GetKeyDown("s"))
        {
            Debug.Log("s");
            string square = "C3";
            commandIssued(square);
        }

        if(Input.GetKeyDown("d"))
        {
            Debug.Log("d");
            string square = "C4";
            commandIssued(square);
        }
            
    }

    void commandIssued(string square){
        desiredMove = getSquare(square);
        if(isLegalMove()){
            move(desiredMove);
        }
        else{
            Debug.Log("Illegal move: " + desiredMove.GetComponent<Square>().matrixPosition + " // " + position);
        }
    }

    private GameObject getSquare(string square){
        return GameObject.Find(square);
    }

    public override bool isLegalMove(GameObject square = null, bool hasEnemy = false){
        if(square == null) square = desiredMove;
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

    private bool CheckMate(){
        GameObject board = GameObject.Find("Tablero");
        if(isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y].name[(int)position.x-1])) //North
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-2].name[(int)position.x-1])) //South
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-1].name[(int)position.x])) //East
            && isDangerous(getSquare(board.GetComponent<Board>().squares[(int)position.y-1].name[(int)position.x-2]))) //West
            return true;
        else return false;
    }

}