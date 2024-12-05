using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    //Prefab and number of enemies to spawn
    public GameObject[] prefab;
    public int firstWave = 1;
    public int secondWave = 3;
    public int thirdWave = 5;
    public int fourthWave = 5;
    public int bossWave = 1;

    //For spawning purposes
    public static int enemyCounter = 0;

    //Spawning area size
    public Vector3 spawn = new Vector3(5f, 5f, 5f);

    //Instance of teleport to check wave counter
    private Teleport teleportScript;

    void Start() {
        teleportScript = FindObjectOfType<Teleport>();
        SpawnWave();
    }

    public void SpawnWave() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        //Reset enemy counter at the beginning of wave
        enemyCounter = 0;
        
        if(teleportScript.waveCounter == 1) {
            for(int i = 0; i < firstWave; i++) {
                Vector3 spawnPos = new Vector3(
                    transform.position.x + Random.Range(-spawn.x / 2, spawn.x /2),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawn.z /2, spawn.z /2)
                );

                GameObject enemy = prefab[0];

                Instantiate(enemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
        else if(teleportScript.waveCounter == 2) {
            for(int i = 0; i < secondWave; i++) {
                Vector3 spawnPos = new Vector3(
                    transform.position.x + Random.Range(-spawn.x / 2, spawn.x /2),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawn.z /2, spawn.z /2)
                );

                GameObject enemy = prefab[1];

                Instantiate(enemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
        else if (teleportScript.waveCounter == 3)
        {
            for (int i = 0; i < thirdWave; i++)
            {
                Vector3 spawnPos = new Vector3(
                    transform.position.x + Random.Range(-spawn.x / 2, spawn.x / 2),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawn.z / 2, spawn.z / 2)
                );

                GameObject enemy = prefab[Random.Range(0, 2)];

                Instantiate(enemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
        else if (teleportScript.waveCounter == 4)
        {
            for (int i = 0; i < fourthWave; i++)
            {
                Vector3 spawnPos = new Vector3(
                    transform.position.x + Random.Range(-spawn.x / 2, spawn.x / 2),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawn.z / 2, spawn.z / 2)
                );

                GameObject enemy = prefab[Random.Range(0, 2)];

                Instantiate(enemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
        else if (teleportScript.waveCounter == 5)
        {
            for (int i = 0; i < bossWave; i++)
            {
                Vector3 spawnPos = new Vector3(
                    transform.position.x + Random.Range(-spawn.x / 2, spawn.x / 2),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawn.z / 2, spawn.z / 2)
                );

                GameObject enemy = prefab[2];

                Instantiate(enemy, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
    }
}
