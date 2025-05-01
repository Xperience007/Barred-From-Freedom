using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Animator animator;
    public Camera mainCamera;
    public float rotationSpeed = 10f;
    public playerController player;

    bool hasAttacked = false;
    public float attackTime = 1.0f;
    public static float damage = 15.0f;
    float slowTimer = 3.0f;
    private bool collided = false;

    public Transform FirePoint;
    public GameObject Fire;
    public GameObject HitPoint;

    public AudioSource myAudio;
    public AudioSource hitAudio;
    public AudioClip shootSound;
    public AudioClip hitSound;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ItemMenu itemMenu = FindObjectOfType<ItemMenu>();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0) && !hasAttacked && !itemMenu.inMenu && !PauseMenu.GameIsPaused)
        {
            myAudio.PlayOneShot(shootSound);
            Hitscan();

            hasAttacked = true;
            player.isShooting = true;
            animator.SetBool("isShooting", true);
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            player.isShooting = false;
            Invoke(nameof(AttackReset), attackTime);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void AttackReset()
    {
        hasAttacked = false;
    }

    public void Hitscan()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, 999f, layerMask))
        {
            GameObject a = Instantiate(Fire, FirePoint.position, Quaternion.identity);
            GameObject b = Instantiate(HitPoint, hit.point, Quaternion.identity);

            Destroy(a, 1);
            Destroy(b, 1);

            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Enemy1") || hit.collider.CompareTag("Enemy2"))
            {
                hitAudio.PlayOneShot(hitSound);
                collided = true;
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                enemy.Enemy1TakeDamage(damage);
                EnemyAI.hasTakenDamage = true;
                if (playerController.isSlowed)
                {
                    enemy.agent.speed -= 0.5f;
                    if (slowTimer > 0)
                    {
                        slowTimer -= Time.deltaTime;
                    }
                    else
                    {
                        enemy.agent.speed += 0.5f;
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
}
