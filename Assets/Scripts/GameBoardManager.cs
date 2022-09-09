using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{

    // variables
    private Vector2 preMousePos;
    private Vector2 postMousePos;
    // end of variables

    // game objects
    public GameObject[,] candies = new GameObject[7, 7];
    public int[,] array = new int[7, 7];
    public GameObject[] prefabs;
    public GameObject selectedCandy;
    public GameObject selectedNeighbor;
    public Vector2 selectedCandyPosition;
    // end of game objects





    void Start()
    {

        GenerateBoard();

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            preMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo.collider != null)
            {

                Debug.Log(hitInfo.transform.gameObject.tag);
                selectedCandy = hitInfo.transform.gameObject;
                selectedCandyPosition = selectedCandy.transform.localPosition;
                //Debug.Log("Selected candy is " + selectedCandy.name + ". Position is " + hitInfo.transform.localPosition);
                Debug.Log("Selected pos" + selectedCandyPosition);

            }
            else
            {

                Debug.Log("No hit");

            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            Vector2 distanceBetweenMousePositions;
            //postMousePos = Input.mousePosition;
            postMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            distanceBetweenMousePositions = postMousePos - preMousePos;
            //Debug.Log("pre : " + preMousePos + ", post : " + postMousePos);
            //Debug.Log("Raw x : " + distanceBetweenMousePositions.x + ", Raw y : " + distanceBetweenMousePositions.y);

            Vector2 swipe;
            swipe.x = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.x), -1, 1);
            swipe.y = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.y), -1, 1);
            Debug.Log("Swipe" + swipe);

            Vector2 selectedNeighborPosition =  selectedCandyPosition + swipe;
            Debug.Log("Selectet neighbor" + selectedNeighborPosition);

            selectedNeighbor = candies[(int)selectedNeighborPosition.x, (int)selectedNeighborPosition.y];

            Debug.Log("x : " + swipe.x + ", y : " + swipe.y);

            SwipeOperations(swipe);
        }

    }

    // This method generates 7x7 gameboard
    public void GenerateBoard()
    {

        for (int i = 0; i < 7; i++)
        {

            for (int j = 0; j < 7; j++)
            {

                int randomCandyIndex = Random.Range(0, prefabs.Length);


                GameObject newCandy = Instantiate(prefabs[randomCandyIndex], transform.position, prefabs[randomCandyIndex].transform.rotation);
                newCandy.transform.parent = transform;
                newCandy.transform.localPosition = new Vector2(i, j);

                candies[i, j] = newCandy;
            }

        }

        //for (int i = 0; i < 7; i++)
        //{

        //    for (int j = 0; j < 7; j++)
        //    {
        //        Debug.Log(" i : " + i + ", j : " + j +  "----" + candies[i, j].gameObject.name);
        //    }

        //}

    }

    private void SwipeOperations(Vector2 move)
    {

        selectedCandy.transform.Translate(move);
        selectedNeighbor.transform.Translate(-move);

        Debug.Log(" Deðiþmiþ konum selected candy :  " + selectedCandy.transform.localPosition + " Deðiþmiþ konum neighbor candy :  " + selectedNeighbor.transform.localPosition);

        candies[(int)selectedCandy.transform.localPosition.x, (int)selectedCandy.transform.localPosition.y] = selectedCandy;
        candies[(int)selectedNeighbor.transform.localPosition.x, (int)selectedNeighbor.transform.localPosition.y] = selectedNeighbor;

    }

}
