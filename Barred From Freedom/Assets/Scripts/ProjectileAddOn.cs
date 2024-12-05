using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddOn : MonoBehaviour
{
    [SerializeField] public static float damage = 20.0f;
    public float delay = 3f;
    float countdown;
    bool hasExploded = false;

    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;

    private bool targetHit;
    private GameObject explosion;

    public AudioSource myAudio;
    public AudioClip explosionSound;

    private float timer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;

        if (myAudio == null)
        {
            myAudio = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (!hasExploded && countdown <= 0f)
        {
            myAudio.PlayOneShot(explosionSound);
            Explode();
            hasExploded = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Enemy1") || collision.collider.CompareTag("Enemy2"))
        {
            myAudio.PlayOneShot(explosionSound);
            hasExploded = true;
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            enemy.Enemy1TakeDamage(damage);
            EnemyAI.hasTakenDamage = true;
            Explode();
        }
        else
        {
            EnemyAI.hasTakenDamage = false;
        }
    }

    private void Explode()
    {
        explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider col = GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        col.enabled = false;
        rb.isKinematic = false;

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(DestroyExplosion());
    }

    private IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(explosion);
    }
}
