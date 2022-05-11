using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ficha : MonoBehaviour
{
    public Vector2 position; 

    public GameObject currentSquare;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPosition(Vector2 pos){
        position = pos;
    }

    protected GameObject getSquare(string square){
        return GameObject.Find(square);
    }

    protected IEnumerator StartMovement(float time, GameObject desiredMove)
    {
        float elapsedtime = 0;
        Vector3 pos_fin = desiredMove.transform.position;

        while(elapsedtime < time)
        {
            transform.position = Vector3.Lerp(transform.position, pos_fin, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }
    }

    public virtual void move(GameObject desiredMove){
        currentSquare.GetComponent<Square>().pieceMoved();
        desiredMove.GetComponent<Square>().newPiece(this.gameObject);
        setPosition(desiredMove.GetComponent<Square>().matrixPosition);
        currentSquare = desiredMove;
        StartCoroutine(StartMovement(1f, desiredMove));
    }

    protected Vector2 Abs(Vector2 vector){
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    public virtual bool commandIssued(string square){
        GameObject desiredMove = getSquare(square);
        if(isLegalMove(getSquare(square))){
            move(desiredMove);
            return true;
        }
        else{
            Debug.Log("Illegal move: " + desiredMove.GetComponent<Square>().matrixPosition + " // " + position);
            return false;
        }
    }

    public abstract bool isLegalMove(GameObject square = null, bool hasEnemy = false);
}
