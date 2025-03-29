using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ItemMenu : MonoBehaviour
{
    public GameObject itemMenuUI;
    public Texture2D[] images;
    public Button item1Button;
    public Button item2Button;
    public Button item3Button;
    public RawImage item1Image;
    public RawImage item2Image;
    public RawImage item3Image;
    public TMP_Text item1Name;
    public TMP_Text item2Name;
    public TMP_Text item3Name;
    public TMP_Text item1Description;
    public TMP_Text item2Description;
    public TMP_Text item3Description;
    private (string name, string description)[] items;
    public bool inMenu = false;

    public AudioClip itemChoice;
    public AudioClip itemPick;

    // Start is called before the first frame update
    void Start()
    {
        itemMenuUI.SetActive(false);
        items = new (string, string)[]
        {
            ("Cutthroat Dagger", "Heal a small amount of health when dealing damage"),
            ("Forgiveness", "Constantly heal by a small amount"),
            ("Ice Cube", "Primary attacks apply slow to an enemy"),
            ("Loaf Of Bread", "Heal 20 health"),
            ("Mighty Shield", "Block incoming damage for a certain amount"),
            ("Monster Gulp", "Gain an increase in move speed"),
            ("The Bomb", "Explosive damage is increased"),
            ("Trigger Ring", "Gain an increase in attack speed"),
            ("Urban Prophecy", "Gain extra damage for every kill"),
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectItems()
    {
        playerController player = FindObjectOfType<playerController>();
        player.itemSounds.PlayOneShot(itemChoice);
        inMenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Disable keys that need to be disabled
        FindObjectOfType<SecondaryAbility_Swipe>().enabled = false;
        FindObjectOfType<UtilityAbility_Grenade>().enabled = false;
        FindObjectOfType<DashScript>().enabled = false;

        //Picking random items
        int randomIndex = Random.Range(0, images.Length);
        item1Image.texture = images[randomIndex];
        item1Name.text = items[randomIndex].name;
        item1Description.text = items[randomIndex].description;
        item1Button.onClick.RemoveAllListeners(); // Clear previous listeners
        item1Button.onClick.AddListener(() => SelectItem(items[randomIndex].name));

        int randomIndex2;
        do
        {
            randomIndex2 = Random.Range(0, images.Length);
        }
        while (randomIndex2 == randomIndex);
        item2Image.texture = images[randomIndex2];
        item2Name.text = items[randomIndex2].name;
        item2Description.text = items[randomIndex2].description;
        item2Button.onClick.RemoveAllListeners();
        item2Button.onClick.AddListener(() => SelectItem(items[randomIndex2].name));

        int randomIndex3;
        do
        {
            randomIndex3 = Random.Range(0, images.Length);
        }
        while (randomIndex3 == randomIndex || randomIndex3 == randomIndex2);
        item3Image.texture = images[randomIndex3];
        item3Name.text = items[randomIndex3].name;
        item3Description.text = items[randomIndex3].description;
        item3Button.onClick.RemoveAllListeners();
        item3Button.onClick.AddListener(() => SelectItem(items[randomIndex3].name));

        itemMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectItem(string itemName)
    {
        // Find the player and apply the selected item's effect.
        playerController player = FindObjectOfType<playerController>();
        player.itemSounds.PlayOneShot(itemPick);
        if (player != null)
        {
            player.ApplyItem(itemName);
        }

        Resume();
    }

    public void Resume()
    {
        itemMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inMenu = false;

        //Enable keys that need to be enabled
        FindObjectOfType<SecondaryAbility_Swipe>().enabled = true;
        FindObjectOfType<UtilityAbility_Grenade>().enabled = true;
        FindObjectOfType<DashScript>().enabled = true;
    }
}
