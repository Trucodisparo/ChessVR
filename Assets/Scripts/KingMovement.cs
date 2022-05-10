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
        if(Input.GetKeyDown("z"))
        {
            Debug.Log("z");
            string square = "B4";
            commandIssued(square);
        }

        if(Input.GetKeyDown("x"))
        {
            Debug.Log("x");
            string square = "C3";
            commandIssued(square);
        }

        if(Input.GetKeyDown("c"))
        {
            Debug.Log("c");
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

    private bool isLegalMove(){
        Vector2 destination = desiredMove.GetComponent<Square>().matrixPosition;
        
        if(!desiredMove.GetComponent<Square>().hasAlly(this.gameObject))
            if((Abs(destination - position) == new Vector2(1,0)) || (Abs(destination - position) == new Vector2(0,1))) return true;

        return false;
    }

    private bool CheckMate(){
        return true;
    }

}