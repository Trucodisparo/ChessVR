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
            if((Abs(destination - position) == new Vector2(1,0)) || (Abs(destination - position) == new Vector2(0,1))
            || (Abs(destination - position) == new Vector2(1,1))) 
                return true;

        return false;
    }

}