using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool collided = false;
    private float delay = 1.5f;
    float countdown;
    public static float damage = 15.0f;
    float slowTimer = 3.0f;

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (!collided && countdown <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision co) {
        PlayerProjectiles playerProjectiles = FindObjectOfType<PlayerProjectiles>();
        if (co.gameObject.CompareTag("Enemy") || co.gameObject.CompareTag("Enemy1") || co.gameObject.CompareTag("Enemy2")) {
            playerProjectiles.hitAudio.PlayOneShot(playerProjectiles.hitSound);
            collided = true;
            Destroy(gameObject);
            EnemyAI enemy = co.gameObject.GetComponent<EnemyAI>();
            enemy.Enemy1TakeDamage(damage);
            EnemyAI.hasTakenDamage = true;
            if(playerController.isSlowed)
            {
                EnemyAI.agent.speed -= 0.5f;
                if (slowTimer > 0)
                {
                    slowTimer -= Time.deltaTime;
                }
                else
                {
                    EnemyAI.agent.speed += 0.5f;
                    slowTimer = 3.0f;
                }
            }
        }
        else
        {
            EnemyAI.hasTakenDamage = false;
        }
    }
}
