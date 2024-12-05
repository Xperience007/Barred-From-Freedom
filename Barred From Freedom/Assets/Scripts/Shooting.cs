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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !hasAttacked)
        {
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
}
