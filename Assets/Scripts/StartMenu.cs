using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{


    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void QuitGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void StartGameButton()
    {
        Debug.Log("StartButton");
        SceneManager.LoadScene("Hub");
    }

    public void InfoGameButton()
    {
        Debug.Log("InfoButton");
        SceneManager.LoadScene("InfoScene");
    }

    public void Return()
    {
        Debug.Log("Return");
        SceneManager.LoadScene("StartScreen");
    }




}