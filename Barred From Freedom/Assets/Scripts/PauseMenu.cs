using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    public static bool GameIsPaused = false;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }
    
    public void Pause() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pause.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Resume() {
        pause.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart() {
        GameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");
    }

    public void Tutorial() {
        GameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else  
            Application.Quit();
        #endif
    }
}
