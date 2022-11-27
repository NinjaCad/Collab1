using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float offset;

    public GameObject projectile;
    public Transform shotPoint;


    [SerializeField] GameObject player;
    PlayerScript playerScript;

    void Awake()
    {
        playerScript = player.GetComponent<PlayerScript>();
    }

    private void Update()
    {
        // Handles the weapon rotation
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerScript.transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (Input.GetMouseButtonDown(0) && playerScript.canShoot == true && playerScript.timeBtwShots < 0)
        {
            Instantiate(projectile, shotPoint.position, transform.rotation);
            playerScript.Knockback(300f, -rotZ);
        }
    }
}
