using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    //Onclick function to quit game
    public void QuitGameButton() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else  
            Application.Quit();
        #endif
    }

    public void toStartScreen() {
        SceneManager.LoadScene("Start");
    }

    public void ToMainGame() {
        PlayerHealth.health = 100.0f;
        Timer.elapsedTime = 0;
        Projectile.damage = 15.0f;
        EnemyProjectile.damage = 5.0f;
        ProjectileAddOn.damage = 20.0f;
        PlayerProjectiles.attackTime = 1.0f;
        SceneManager.LoadScene("MainGame");
    }

    public void ToTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void ToCredits() {
        SceneManager.LoadScene("Credits");
    }
}
