using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{


    // gameobjects
    public GameObject menuPanel;
    public GameObject creditPanel;
    // end of gameobjects

    public void Strart()
    {

        SceneManager.LoadScene("GameScene");

    }


    public void OpenCreditPanel()
    {

        menuPanel.gameObject.SetActive(false);
        creditPanel.gameObject.SetActive(true);

    }

    public void CloseCreditPanel()
    {

        creditPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);

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

}
