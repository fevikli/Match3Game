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
    [SerializeField] int score;
    [SerializeField] int moveCount = 10;
    [SerializeField] int pointPerCandy = 5;
    private int combo;
    private static int boardWidth = 7;
    private static int boardHeight = 7;
    private int row;
    private int col;
    private bool hit;

    private bool isGameRunning;
    // end of variables


    // game objects
    public GameObject candyBackground;
    public GameObject[,] candies = new GameObject[boardWidth, boardHeight];
    public GameObject[,] destroyer = new GameObject[boardWidth, boardHeight];
    public GameObject[] prefabs;
    public GameObject explosionPrefab;
    public GameObject selectedCandy;
    public GameObject selectedNeighbor;
    // end of game objects



    void Start()
    {

        isGameRunning = true;
        score = 0;

        UIManager.Instance.UpdateMoveCount(moveCount);

        GenerateBoard();

    }

    void Update()
    {

        if(isGameRunning)
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
                    //row = (int)selectedCandyPosition.x;
                    //col = (int)selectedCandyPosition.y;
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
                    postMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    distanceBetweenMousePositions = postMousePos - preMousePos;

                    swipe.x = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.x), -1, 1);
                    swipe.y = (int)Mathf.Clamp(Mathf.Round(distanceBetweenMousePositions.y), -1, 1);
                    Debug.Log("Swipe" + swipe);

                    Vector2 selectedNeighborPosition = selectedCandyPosition + swipe;
                    Debug.Log("Selectet neighbor" + selectedNeighborPosition);

                    selectedNeighbor = candies[(int)selectedNeighborPosition.x, (int)selectedNeighborPosition.y];

                    Debug.Log("x : " + swipe.x + ", y : " + swipe.y);

                    if(Mathf.Abs(distanceBetweenMousePositions.x ) > 0.5f || Mathf.Abs(distanceBetweenMousePositions.y) > 0.5f)
                    {

                        SwipeOperations(swipe);

                    }
                }
            }

            CheckBoardForCombos();

        }
        else
        {

            UIManager.Instance.ActivateGameOverPanel();

        }
        

    }


    private void CheckBoardForCombos()
    {

        GenerateBoard();
        VerticalComboCheck();
        HorizontalComboCheck();
        DestroyCombos();
        DropTheCandies();

    }


    // This method generates boardWidth x boardHeight gameboard
    public void GenerateBoard()
    {

        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {

                if (candies[i, j] == null)
                {

                    FillTheGaps(i, j);

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
                combo = 0;

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

                    }

                    AddScore(combo);

                }
            }
        }
    }


    // This method checks combos which are at horizontal
    private void HorizontalComboCheck()
    {
        // horizontal check
        for (int i = 0; i < boardWidth; i++)
        {

            for (int j = 0; j < boardHeight; j++)
            {

                combo = 0;
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

                    }

                    AddScore(combo);

                }
            }
        }
    }


    // This method destroys combos and fills in the gaps
    private void DestroyCombos()
    {

        for (int j = 0; j < boardHeight; j++)
        {

            for (int i = 0; i < boardWidth; i++)
            {

                if (destroyer[i, j] != null)
                {

                    Destroy(destroyer[i, j].gameObject);

                    // Instantiate explosion animation for explosions
                    GameObject explosionAnim = Instantiate(explosionPrefab, destroyer[i, j].transform.position, explosionPrefab.transform.rotation);
                    Destroy(explosionAnim.gameObject, 0.667f); // Destroy time equals animation time for smoother transition

                    destroyer[i, j] = null;
                    candies[i, j] = null;

                }
            }
        }
    }


    // This method manages drop operations
    private void DropTheCandies()
    {

        for (int i = 0; i < boardWidth; i++)
        {

            for (int j = 0; j < boardHeight; j++)
            {

                if (candies[i, j] != null)
                {

                    // a and b are placeholders for i and j
                    int a = i;
                    int b = j;

                    while (b - 1 > -1 && candies[a, b - 1] == null)
                    {

                        SlideDown(a, b);

                        b -= 1;

                    }
                }
            }
        }
    }


    // This method shifts the candy by one unit and changes the position in the candy[,] matrix
    private void SlideDown(int a, int b)
    {

        candies[a, b].transform.position -= new Vector3(0, 1, 0);
        candies[a, b - 1] = candies[a, b];
        candies[a, b] = null;

    }


    // This method fills the gaps
    private void FillTheGaps(int i, int j)
    {

        int randomCandyIndex = Random.Range(0, prefabs.Length);

        GameObject newCandy = Instantiate(prefabs[randomCandyIndex], transform.position, prefabs[randomCandyIndex].transform.rotation);
        newCandy.transform.parent = transform;
        newCandy.transform.localPosition = new Vector2(i, j);

        candies[i, j] = newCandy;

    }


    // This method swipes selected candy wþth neighbor candy
    private void SwipeOperations(Vector2 move)
    {

        selectedCandy.transform.Translate(move);
        selectedNeighbor.transform.Translate(-move);

        Debug.Log(" Deðiþmiþ konum selected candy :  " + selectedCandy.transform.localPosition + " Deðiþmiþ konum neighbor candy :  " + selectedNeighbor.transform.localPosition);

        candies[(int)selectedCandy.transform.localPosition.x, (int)selectedCandy.transform.localPosition.y] = selectedCandy;
        candies[(int)selectedNeighbor.transform.localPosition.x, (int)selectedNeighbor.transform.localPosition.y] = selectedNeighbor;


        moveCount--;
        UIManager.Instance.UpdateMoveCount(moveCount);
        if (moveCount <=0 )
        {

            isGameRunning = false;

        }

    }


    // This method increases score and gives info to UIManager
    private void AddScore(int combo)
    {

        combo++;
        score += (combo * pointPerCandy);

        UIManager.Instance.UpdateScore(score);

    }
}
