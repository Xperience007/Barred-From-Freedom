using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerController : MonoBehaviour
{
    public CharacterController controller;
    private Animator animator;
    public Transform camera;
    public float speed = 1.75f;
    public float turnSmoothTime = 0.1f;
    public float healthGain = 0.0f;
    public float daggerHealthGain = 5.0f;
    float turnSmoothVelocity;
    public bool isShooting = false;
    public Vector3 moveDirection;
    public static bool isSlowed = false;

    public AudioSource mainMusic;
    public AudioSource healMusic;
    public AudioSource itemSounds;
    public AudioSource healSounds;

    public GameObject healTeleporter;
    public TMP_Text healText;

    private int killCounter = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, 0.215f, 0.215f); // Adjust range based on your scene
        transform.position = position;
    }

    public void ApplyItem(string item)
    {
        switch (item)
        {
            case "Cutthroat Dagger":
                daggerHealthGain += 0.2f;
                StartCoroutine(Dagger());
                break;

            case "Forgiveness":
                healthGain += 0.1f;
                StartCoroutine(Heal());
                break;

            case "Ice Cube":
                StartCoroutine(Ice());
                break;

            case "Loaf Of Bread":
                PlayerHealth.health += 20f;
                break;

            case "Mighty Shield":
                EnemyProjectile.damage -= 1.0f;
                PlayerHealth.enemyContactDamage -= 1.0f;
                break;

            case "Monster Gulp":
                speed += 0.5f;
                break;

            case "The Bomb":
                ProjectileAddOn.damage += 5.0f;
                break;

            case "Trigger Ring":
                PlayerProjectiles.attackTime -= 0.1f;
                break;

            case "Urban Prophecy":
                StartCoroutine(Prophecy());
                break;
        }
    }

    IEnumerator Heal()
    {
        while (true)
        {
            PlayerHealth.health += healthGain;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Dagger()
    {
        while (true)
        {
            if (EnemyAI.hasTakenDamage)
            {
                EnemyAI.hasTakenDamage = false;
                PlayerHealth.health += daggerHealthGain;
            }

            yield return null;
        }
    }

    IEnumerator Prophecy()
    {
        while (true)
        {
            if (SpawnEnemies.enemyCounter != killCounter)
            {
                Projectile.damage += 1.0f;
                killCounter = SpawnEnemies.enemyCounter;
            }

            yield return null;
        }
    }

    IEnumerator Ice()
    {
        isSlowed = true;

        yield return null;
    }
}
