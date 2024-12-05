using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashScript : MonoBehaviour
{
    playerController player;
    private Animator animator;
    public float dashSpeed = 10f;
    public float dashTime = 0.25f;
    public float dashCooldown = 3f;
    public float abilityTimer;

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

        if(Input.GetKeyDown(KeyCode.LeftShift) && abilityTimer <= 0)
        {
            animator.SetBool("isDashing", true);
            Color usedColor;
            if (ColorUtility.TryParseHtmlString(hexColor, out usedColor))
            {
                rawImage.color = usedColor;
            }
            StartCoroutine(Dash());
            abilityTimer = dashCooldown;
        }
        else
        {
            animator.SetBool("isDashing", false);
            if (abilityTimer <= 0)
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

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            player.controller.Move(player.moveDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }

        
    }
}
