using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    //Health bar variables
    public GameObject healthBar;
    private static Image healthBarImage;
    [SerializeField] public static float health = 100.0f;
    public static float enemyContactDamage = 5.0f;
    private bool isTakingDamage = false;

    void Start() {
        //Check if healthBarImage is null
        if (healthBarImage == null) { 
            healthBarImage = healthBar.GetComponent<Image>();
        }

        if(healthBar != null) {
            healthBarImage = healthBar.transform.GetComponent<Image>();
        }

        SetHealthBarValue(health);
    }

    void Update() {
        if(health > 100)
        {
            health = 100.0f;
        }

        if(health == 0) {
            SceneManager.LoadScene("GameOver");
        }

        SetHealthBarValue(health / 100);

        //Set win condition to load you win screen
    }

    public void TakeDamage(float damage) {
        health -= damage;
        health = Mathf.Max(health, 0); //Prevents health from dropping below zero
        SetHealthBarValue(health / 100.0f);
    }

    public static void SetHealthBarValue(float value) {
        healthBarImage.fillAmount = value;
    }

    public static float GetHealthBarValue() {
        return healthBarImage.fillAmount;
    }

    void OnCollisionEnter(Collision other) {
        //Enemy contact damage
        if(other.gameObject.CompareTag("Enemy1")) {
            Debug.Log("Player is touching an enemy");
            TakeDamage(enemyContactDamage);
        }
    }

    void OnCollisionStay(Collision other) {
        if(!isTakingDamage && (other.gameObject.CompareTag("Enemy1"))) {
            Debug.Log("Player is touching an enemy again");
            StartCoroutine(Wait(2));
        }
    }

    private IEnumerator Wait(int seconds) {
        isTakingDamage = true;
        yield return new WaitForSeconds(seconds);
        TakeDamage(enemyContactDamage);
        isTakingDamage = false;
    }
}