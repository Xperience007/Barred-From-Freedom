using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    public Camera camera;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30.0f;
    public AudioSource myAudio;
    public AudioSource hitAudio;
    public AudioClip shootSound;
    public AudioClip hitSound;

    bool hasAttacked = false;
    public static float attackTime = 1.0f;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();

    void Update() {
        ItemMenu itemMenu = FindObjectOfType<ItemMenu>();
        if(Input.GetMouseButtonDown(0) && !hasAttacked && !itemMenu.inMenu && !PauseMenu.GameIsPaused) {
            myAudio.PlayOneShot(shootSound);
            ShootProjectile();
        }
    }

    void ShootProjectile() {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = camera.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;

        Vector3 destination;

        if(Physics.Raycast(ray, out hit, 999f, aimColliderLayerMask)) {
            destination = hit.point;
        }
        else {
            destination = ray.GetPoint(1000);
        }

        Vector3 worldAimtarget = destination;
        worldAimtarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimtarget - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        Vector3 direction = (destination - firePoint.position).normalized;

        hasAttacked = true;
        GameObject projectileObj = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(direction, Vector3.up));
        projectileObj.transform.forward = direction;
        projectileObj.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);

        Invoke(nameof(AttackReset), attackTime);
    }

    private void AttackReset()
    {
        hasAttacked = false;
    }
}
