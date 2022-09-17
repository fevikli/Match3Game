using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{

    // components
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI moveCountText;
    // end of components


    // gameobjects
    public GameObject gameOverPanel;
    // end of gameobjects


    public static UIManager Instance;
    void Awake()
    {

        if (Instance != null)
        {

            Destroy(gameObject);

        }
        else
        {

            Instance = this;

        }

    }

    public void Restrart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void MainMenu()
    {

        SceneManager.LoadScene("MainMenu");

    }

    public void Quitgame()
    {
#if UNITY_EDITOR
        Debug.Log("Quit game");
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ActivateGameOverPanel()
    {

        gameOverPanel.gameObject.SetActive(true);

    }
    public void UpdateScore(int score)
    {

        scoreText.text = "Score :  " + score;

    }

    public void UpdateMoveCount(int moveCount)
    {

        moveCountText.text = "Move :  " + moveCount;

    }

}
