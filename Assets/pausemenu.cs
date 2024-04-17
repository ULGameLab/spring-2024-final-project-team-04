using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject Panel; 
    public static bool paused;
    [SerializeField] Health HOBJ;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        Panel.SetActive(!Panel.activeSelf);

        // Optionally pause/unpause the game when the pause menu is activated/deactivated
        Time.timeScale = pauseMenuUI.activeSelf ? 0 : 1;
        paused = paused ? false : true;
        if (paused) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        Debug.Log($"Paused: {paused}");
    }

    public void QuitGameButton()
    {
        Debug.Log($"Q");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void MainMenuButton()
    {
        Debug.Log("MainMenuButton");
        TogglePauseMenu();
        SceneManager.LoadScene("Start Screen");
    }

    public void RestartGameButton()
    {
        Debug.Log("RestartButton");
        TogglePauseMenu();
        SceneManager.LoadScene("Terrain");
    }

    public void fullHeal() {
        HOBJ.SetHealth(100.0f);
    }

    public bool IsPaused()
    {
        return paused;
    }
}
