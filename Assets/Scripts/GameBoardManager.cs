using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{

    // variables
    private Vector2 preMousePos;
    private Vector2 postMousePos;
    private Vector2 swipe;
    public Vector2 selectedCandyPosition;

    private static int boardWidth = 7;
    private static int boardHeight = 7;
    private int row;
    private int col;
    private bool hit;
    // end of variables

    // game objects
    public GameObject candyBackground;

    public GameObject[,] candies = new GameObject[boardWidth, boardHeight];
    public GameObject[,] destroyer = new GameObject[boardWidth, boardHeight];
    public GameObject[] prefabs;
    public GameObject selectedCandy;
    public GameObject selectedNeighbor;
    // end of game objects





    void Start()
    {

        GenerateBoard();
        CheckBoardForCombos();

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
                row = (int)selectedCandyPosition.x;
                col = (int)selectedCandyPosition.y;
                //Debug.Log("Selected candy is " + selectedCandy.name + ". Position is " + hitInfo.transform.localPosition);
                Debug.Log("Selected pos" + selectedCandyPosition);

                hit = true;

            }
            else
            {
                hit = false;
                Debug.Log("No hit");

            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            if (hit)
            {

                Vector2 distanceBetweenMousePositions;
                //postMousePos = Input.mousePosition;
                postMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                distanceBetweenMousePositions = postMousePos - preMousePos;
                //Debug.Log("pre : " + preMousePos + ", post : " + postMousePos);
                //Debug.Log("Raw x : " + distanceBetweenMousePositions.x + ", Raw y : " + distanceBetweenMousePositions.y);


                swipe.x = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.x), -1, 1);
                swipe.y = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.y), -1, 1);
                Debug.Log("Swipe" + swipe);

                Vector2 selectedNeighborPosition = selectedCandyPosition + swipe;
                Debug.Log("Selectet neighbor" + selectedNeighborPosition);

                selectedNeighbor = candies[(int)selectedNeighborPosition.x, (int)selectedNeighborPosition.y];

                Debug.Log("x : " + swipe.x + ", y : " + swipe.y);

                SwipeOperations(swipe);

            }

        }

    }

    private void CheckBoardForCombos()
    {

        VerticalComboCheck();
        HorizontalComboCheck();

    }


    // This method checks combos which are at horizontal
    private void HorizontalComboCheck()
    {
        // horizontal check
        for (int i = 0; i < boardWidth; i++)
        {

            for (int j = 0; j < boardHeight; j++)
            {

                int combo = 0;
                int postY = j + 1;

                while (postY < boardHeight && candies[i, j].name == candies[i, postY].name)
                {

                    combo++;

                    postY += 1;

                }
                if (combo >= 2)
                {

                    int endOfCombo = postY - 1;

                    for (int k = endOfCombo; k > j; k--)
                    {

                        destroyer[i, j] = candies[i, j];
                        destroyer[i, k] = candies[i, k];

                        GameObject go = Instantiate(candyBackground, candies[i, k].transform.position, candyBackground.transform.rotation);
                        go.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        go = Instantiate(candyBackground, candies[i, j].transform.position, candyBackground.transform.rotation);
                        go.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    }
                }
            }
        }
    }

    // This method checks combos which are at vertical
    private void VerticalComboCheck()
    {
        // vertical check
        for (int j = 0; j < boardHeight; j++)
        {

            for (int i = 0; i < boardWidth; i++)
            {
                int combo = 0;

                int postX = i + 1;

                while (postX < boardWidth && candies[i, j].name == candies[postX, j].name)
                {
                    combo++;

                    postX += 1;

                }
                if (combo >= 2)
                {

                    int endOfCombo = postX - 1;

                    for (int k = endOfCombo; k > i; k--)
                    {

                        destroyer[i, j] = candies[i, j];
                        destroyer[k, j] = candies[k, j];

                        GameObject go = Instantiate(candyBackground, candies[k, j].transform.position, candyBackground.transform.rotation);
                        go.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        go = Instantiate(candyBackground, candies[i, j].transform.position, candyBackground.transform.rotation);
                        go.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    }
                }
            }
        }
    }

    // This method generates boardWidth x boardHeight gameboard
    public void GenerateBoard()
    {

        for (int i = 0; i < boardHeight; i++)
        {

            for (int j = 0; j < boardWidth; j++)
            {

                int randomCandyIndex = Random.Range(0, prefabs.Length);


                GameObject newCandy = Instantiate(prefabs[randomCandyIndex], transform.position, prefabs[randomCandyIndex].transform.rotation);
                newCandy.transform.parent = transform;
                newCandy.transform.localPosition = new Vector2(i, j);

                candies[i, j] = newCandy;
            }

        }

    }

    private void SwipeOperations(Vector2 move)
    {

        selectedCandy.transform.Translate(move);
        selectedNeighbor.transform.Translate(-move);

        Debug.Log(" Deðiþmiþ konum selected candy :  " + selectedCandy.transform.localPosition + " Deðiþmiþ konum neighbor candy :  " + selectedNeighbor.transform.localPosition);

        candies[(int)selectedCandy.transform.localPosition.x, (int)selectedCandy.transform.localPosition.y] = selectedCandy;
        candies[(int)selectedNeighbor.transform.localPosition.x, (int)selectedNeighbor.transform.localPosition.y] = selectedNeighbor;


        CheckBoardForCombos();
    }

}
