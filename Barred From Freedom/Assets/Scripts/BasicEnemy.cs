using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<RedEnemy>() != null)
        {
            RedEnemy enemy = gameObject.GetComponent<RedEnemy>();
            if (enemy != null)
            {
                health = enemy.health;
            }
        }
        else if(gameObject.GetComponent<GreenEnemy>() != null)
        {
            GreenEnemy enemy = gameObject.GetComponent<GreenEnemy>();
            if (enemy != null)
            {
                health = enemy.health;
            }
        }
        else if(gameObject.GetComponent<BlueEnemy>() != null)
        {
            BlueEnemy enemy = gameObject.GetComponent<BlueEnemy>();
            if (enemy != null)
            {
                health = enemy.health;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
