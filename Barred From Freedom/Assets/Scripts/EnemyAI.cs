using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    //Refernece variables from scripts
    private SpawnEnemies spawnScript;
    private Teleport teleportScript;

    //Variables used in script
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whereIsGround, whereIsPlayer;
    private Animator animator;

    //For patrolling
    public Vector3 patrol;
    bool walkPoints;
    public float walkPointsRange;

    //For attacking
    public float attackTime = 3f;
    bool haveAttacked;
    public GameObject projectile;

    //States
    public float sight, attack;
    public bool inSight, inRange;

    //For health
    public float enemy1Health = 100.0f;
    public float maxHealth = 100.0f;
    public static float playerDamage = 100.0f;
    public static bool hasTakenDamage = false;
    public bool enemyDied = false;
    [SerializeField] private EnemyHealthbar healthbar;
    [SerializeField] private Canvas healthbarCanvas;

    //For audio
    public AudioSource myAudio;
    public AudioClip enemyAttackSound;

    private bool hasRun = false;

    //Portal tags
    [SerializeField] private string[] portalTags = {"SelectableHeal", "SelectablePower", "SelectableSpawn"};

    private void Awake() {
        //Disable portals initally
        DisablePortals(portalTags);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    void Start() {
        //Find object with scripts
        spawnScript = FindObjectOfType<SpawnEnemies>();
        teleportScript = FindObjectOfType<Teleport>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        animator.SetBool("hasDied", false);
        healthbar.UpdateHealthbar(maxHealth, enemy1Health);

        if (myAudio == null)
        {
            myAudio = GetComponent<AudioSource>();
        }
    }

    void Update() {
        if (enemyDied)
        {
            agent.isStopped = true;
            agent.enabled = false;
            return;  // Stops execution of Update() after death
        }
        //Checks enemy health
        if (enemy1Health <= 0 && !hasRun)
        {
            enemyDied = true;
            StartCoroutine(EnemyDeath());
        }
        else
        {
            enemyDied = false;
        }

        //Checks if player is in enemy sight or attack range
        inSight = Physics.CheckSphere(transform.position, sight, whereIsPlayer);
        inRange = Physics.CheckSphere(transform.position, attack, whereIsPlayer);

        if (!inSight && !inRange && !enemyDied)
        {
            Patrol();
        }
        else if (inSight && !inRange && !enemyDied)
        {
            Chase();
        }
        else if (inSight && inRange && !enemyDied)
        {
            Attack();
        }
    }

    private void Patrol() {
        animator.SetBool("isAttacking", false);
        if (!walkPoints) {
            addWalkPoints();
        }

        if(walkPoints) {
            agent.SetDestination(patrol);
        }

        Vector3 distanceToWalkPoints = transform.position - patrol;

        //Reached walkPoint
        if(distanceToWalkPoints.magnitude < 1f) {
            walkPoints = false;
        }
    }

    private void addWalkPoints() {
        float randX = Random.Range(-walkPointsRange, walkPointsRange);
        float randZ = Random.Range(-walkPointsRange, walkPointsRange);

        patrol = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if(Physics.Raycast(patrol, -transform.up, 2f, whereIsGround)) {
            walkPoints = true;
        }
    }

    private void Chase()
    {
        animator.SetBool("isAttacking", false);
        agent.SetDestination(player.position);
    }

    private void Attack() {
        //Stop enemy from moving while attacking
        agent.SetDestination(transform.position);

        //Get direction of player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        //Rotate to player
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        animator.SetBool("isAttacking", true);
        //Check if attacked already and if so reset attack
        if (!haveAttacked && !enemyDied) {
            myAudio.PlayOneShot(enemyAttackSound);
            //Instantiating projectile
            if(gameObject.tag == "Enemy" || gameObject.tag == "Enemy2")
            {
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
                rb.AddForce(Vector3.down * 0.15f, ForceMode.Impulse);
            }

            if(gameObject.tag == "Enemy1")
            {
                PlayerHealth.health -= 5.0f;
            }

            //Reset attack after attackTime
            haveAttacked = true;
            Invoke(nameof(attackReset), attackTime);
        }
    }

    private void attackReset() {
        haveAttacked = false;
        animator.SetBool("isAttacking", true);
    }

    public void Enemy1TakeDamage(float damage) {
        enemy1Health -= damage;
        enemy1Health = Mathf.Max(enemy1Health, 0); //Prevents health from dropping below zero
        healthbar.UpdateHealthbar(maxHealth, enemy1Health);
    }

    private IEnumerator EnemyDeath() {
        enemyDied = true;
        hasRun = true;
        SpawnEnemies.enemyCounter += 1;
        ItemMenu items = FindObjectOfType<ItemMenu>();
        agent.enabled = false;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(8, 9, true);
        agent.isStopped = true;
        healthbarCanvas.enabled = false;
        animator.SetBool("hasDied", true);
        CancelInvoke(nameof(attackReset));

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);

        if (teleportScript.waveCounter == 1) {
            //Check if it is last enemy in first wave
            if(SpawnEnemies.enemyCounter == spawnScript.firstWave) {
                items.SelectItems();
                EnablePortals(portalTags);
                SpawnEnemies.enemyCounter = 0;
            }
        }
        else if(teleportScript.waveCounter == 2) {
            //Check if it is last enemy in second wave
            if(SpawnEnemies.enemyCounter == spawnScript.secondWave) {
                items.SelectItems();
                EnablePortals(portalTags);
                SpawnEnemies.enemyCounter = 0;
            }
        }
        else if (teleportScript.waveCounter == 3)
        {
            //Check if it is last enemy in second wave
            if (SpawnEnemies.enemyCounter == spawnScript.thirdWave)
            {
                items.SelectItems();
                EnablePortals(portalTags);
                SpawnEnemies.enemyCounter = 0;
            }
        }
        else if (teleportScript.waveCounter == 4)
        {
            //Check if it is last enemy in second wave
            if (SpawnEnemies.enemyCounter == spawnScript.fourthWave)
            {
                items.SelectItems();
                EnablePortals(portalTags);
                SpawnEnemies.enemyCounter = 0;
            }
        }
        else if (teleportScript.waveCounter == 5)
        {
            //Check if it is last enemy in second wave
            if (SpawnEnemies.enemyCounter == spawnScript.bossWave)
            {
                SceneManager.LoadScene("YouWin");
            }
        }
        hasRun = false;
        enemyDied = false;
    }

    void DisablePortals(string[] tags) {
        foreach(string tag in tags) {
            GameObject[] portals = GameObject.FindGameObjectsWithTag(tag);
            foreach(GameObject obj in portals) {
                obj.SetActive(false);
            }
        }
    }

    void EnablePortals(string[] tags) {
        foreach(string tag in tags) {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
            foreach(GameObject obj in allObjects) {
                if(obj.CompareTag(tag) && !obj.activeInHierarchy) {
                    obj.SetActive(true);
                }
            }
        }
    }
}
