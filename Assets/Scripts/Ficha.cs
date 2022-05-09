using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    public enum Color {Black, White};
    public Color pieceColor;
    public Vector2 position; 

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

    private IEnumerator StartMovement(float time, GameObject desiredMove)
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

    public void move(GameObject desiredMove){
        desiredMove.GetComponent<Square>().newPiece(this.gameObject);
        setPosition(desiredMove.GetComponent<Square>().matrixPosition);
        StartCoroutine(StartMovement(1f, desiredMove));
    }
}
