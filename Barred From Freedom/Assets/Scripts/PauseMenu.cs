using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    public static bool GameIsPaused = false;

    public void Update() {
        if (GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    
    public void Pause() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pause.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;

        //Disable keys that need to be disabled
        FindObjectOfType<SecondaryAbility_Swipe>().enabled = false;
        FindObjectOfType<UtilityAbility_Grenade>().enabled = false;
        FindObjectOfType<DashScript>().enabled = false;
        FindObjectOfType<Teleport>().enabled = false;
    }

    public void Resume() {
        pause.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Enable keys that need to be enabled
        FindObjectOfType<SecondaryAbility_Swipe>().enabled = true;
        FindObjectOfType<UtilityAbility_Grenade>().enabled = true;
        FindObjectOfType<DashScript>().enabled = true;
        FindObjectOfType<Teleport>().enabled = true;
    }

    public void Restart() {
        GameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");

        //Enable keys that need to be enabled
        FindObjectOfType<SecondaryAbility_Swipe>().enabled = true;
        FindObjectOfType<UtilityAbility_Grenade>().enabled = true;
        FindObjectOfType<DashScript>().enabled = true;
        FindObjectOfType<Teleport>().enabled = true;
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
