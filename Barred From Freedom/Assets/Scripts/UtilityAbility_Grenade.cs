using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UtilityAbility_Grenade : MonoBehaviour
{
    playerController player;
    private Animator animator;
    public float grenadeCooldown = 5f;
    public float abilityTimer;
    public GameObject grenadePrefab;
    public Transform camera;
    public Transform throwPoint;
    public float throwForce;
    public float upwardForce;

    public RawImage rawImage;
    public TMP_Text timerText;
    private string hexColor = "#383838";
    private string hexColor2 = "#FFFFFF";

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<playerController>();
        animator = GetComponent<Animator>();
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        abilityTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftControl) && abilityTimer <= 0)
        {
            animator.SetBool("UtilityAbilityUse", true);
            Color usedColor;
            if(ColorUtility.TryParseHtmlString(hexColor, out usedColor))
            {
                rawImage.color = usedColor;
            }
            ThrowGrenade();
            abilityTimer = grenadeCooldown;
        }
        else
        {
            animator.SetBool("UtilityAbilityUse", false);
            if(abilityTimer <= 0)
            {
                Color activeColor;
                if (ColorUtility.TryParseHtmlString(hexColor2, out activeColor))
                {
                    rawImage.color = activeColor;
                }
                timerText.gameObject.SetActive(false);
            }
            else
            {
                timerText.gameObject.SetActive(true);
                float time = Mathf.Round(abilityTimer * 10f) / 10f;
                timerText.text = time.ToString("0.0");
            }
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, camera.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        Vector3 forceToAdd = camera.transform.forward * throwForce + transform.up * upwardForce;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
