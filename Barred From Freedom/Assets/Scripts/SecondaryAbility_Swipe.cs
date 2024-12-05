using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondaryAbility_Swipe : MonoBehaviour
{
    playerController player;
    private Animator animator;
    public float swipeCooldown = 3f;
    public float abilityTimer;
    public GameObject hitbox;
    public float damage = 15.0f;

    public RawImage rawImage;
    public TMP_Text timerText;
    private string hexColor = "#383838";
    private string hexColor2 = "#FFFFFF";
    private MeshRenderer meshRenderer;

    public AudioSource myAudio;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<playerController>();
        animator = GetComponent<Animator>();
        meshRenderer = hitbox.GetComponent<MeshRenderer>();
        timerText.gameObject.SetActive(false);
        hitbox.SetActive(false);
        meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        abilityTimer -= Time.deltaTime;

        if (Input.GetMouseButton(1) && abilityTimer <= 0)
        {
            animator.SetBool("SecondaryAbilityUse", true);
            abilityTimer = swipeCooldown;
            StartCoroutine(ActivateHitbox());
            Color usedColor;
            if (ColorUtility.TryParseHtmlString(hexColor, out usedColor))
            {
                rawImage.color = usedColor;
            }
        }
        else
        {
            animator.SetBool("SecondaryAbilityUse", false);
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

    private IEnumerator ActivateHitbox()
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitbox.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Enemy1") || collider.gameObject.CompareTag("Enemy2"))
        {
            myAudio.PlayOneShot(hitSound);
            EnemyAI enemy = collider.gameObject.GetComponent<EnemyAI>();
            enemy.Enemy1TakeDamage(damage);
            EnemyAI.hasTakenDamage = true;
        }
        else
        {
            EnemyAI.hasTakenDamage = false;
        }
    }
}