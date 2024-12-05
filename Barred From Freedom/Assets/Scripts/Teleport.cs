using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleport : MonoBehaviour
{
    //Allows the SpawnEnemies script to be assigned in editor
    public SpawnEnemies spawn;
    public GameObject player;
    public bool inHealRoom = false;
    public AudioClip healingSound;

    //Wave counter
    public int waveCounter = 0;
    public TMP_Text textCounter;

    private void Awake() {
        if(spawn == null) {
            spawn = gameObject.GetComponent<SpawnEnemies>();
        }
    }

    private void Update() {
        playerController player = FindObjectOfType<playerController>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //check for objects on selectable layer
        int layer = LayerMask.GetMask("Selectable");

        ItemMenu item = FindObjectOfType<ItemMenu>();
        
        if (!item.inMenu)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                GameObject selected = hit.collider.gameObject;
                if (hit.collider.CompareTag("SelectableHeal"))
                {
                    //T to teleport to heal room
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        player.mainMusic.mute = true;
                        player.healMusic.Play();
                        //check if healing portal is clicked on
                        player.transform.position = new Vector3(-14.147f, .285f, 1.744049f);
                        inHealRoom = true;
                    }
                }
                else if (hit.collider.CompareTag("Selectable"))
                {
                    //T to teleport back to arena
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        PlayerHealth.health += 50.0f;
                        player.healMusic.mute = true;
                        player.mainMusic.mute = false;
                        player.healSounds.PlayOneShot(healingSound);
                        Destroy(player.healTeleporter);
                        Destroy(player.healText);
                        //Check if teleportable object is clicked
                        player.transform.position = new Vector3(0.212f, 0.215f, -1.438177f);
                        inHealRoom = false;
                    }
                }
                else if (hit.collider.CompareTag("SelectableSpawn"))
                {
                    //T to spawn new wave of enemies
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        //Check if spawn gate is clicked and increment wave counter
                        waveCounter++;
                        textCounter.text = $"Wave Counter = {waveCounter}";

                        //Starts new enemy spawn
                        spawn.SpawnWave();
                    }
                }
            }
        }
    }
}
