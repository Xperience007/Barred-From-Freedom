using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        healthbarSprite.fillAmount= currentHealth / maxHealth;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
