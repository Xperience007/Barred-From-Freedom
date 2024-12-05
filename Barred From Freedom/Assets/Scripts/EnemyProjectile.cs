using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private bool collided = false;
    private float delay = 1.5f;
    float countdown;
    public static float damage = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (!collided && countdown <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.CompareTag("Player"))
        {
            collided = true;
            Destroy(gameObject);
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            player.TakeDamage(damage);
        }
    }
}
