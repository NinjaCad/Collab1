using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //[SerializeField] GameObject gameManager;
    //GameManagerScript gMS;
    Rigidbody2D rb;
    RaycastHit2D raycast;
    [SerializeField] LayerMask collisionLayers;

    Vector3 screenPos;
    Vector3 worldPos;
    Vector3 mousePos;
    Vector2 mousePos2D;

    Vector2 playerToMouse;
    float angle;
    
    float coyote;
    float buffer;
    [SerializeField] float jumpSpeed;
    [SerializeField] float speedX;
    [SerializeField] float power;
    [SerializeField] float resistance;
    float velX;
    float reloadTime;
    int currentAmmo;
    [SerializeField] int maxAmmo;

    [SerializeField] float wallSlidingSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //gMS = gameManager.GetComponent<GameManagerScript>();
    }

    void Update()
    {
        screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + 1;
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        mousePos = new Vector3((Mathf.Round(worldPos.x)), (Mathf.Round(worldPos.y)), 0);

        velX = Input.GetAxisRaw("Horizontal");

        if (isGrounded())
        {
            coyote = 0.10f;
            currentAmmo = maxAmmo;
        } else
        {
            coyote -= Time.deltaTime;
        }
            
        if (Input.GetButtonDown("Jump"))
        {
            buffer = 0.15f;
        } else
        {
            buffer -= Time.deltaTime;
        }

        if (coyote > 0f && buffer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            buffer = 0f;
            reloadTime = 0f;
        }

        if(Input.GetMouseButtonDown(0) && currentAmmo > 0 && reloadTime <= 0f)
        {
            playerToMouse = (mousePos - transform.position) * -1;
            angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x);
            rb.velocity += new Vector2(power * (Mathf.Cos(angle)), power * (Mathf.Sin(angle)));
            if(rb.velocity.y > 25)
            {
                rb.velocity = new Vector2(rb.velocity.x, 25f);
            }
            reloadTime = 0.3f;
            currentAmmo -= 1;
        } else
        {
            reloadTime -= Time.deltaTime;
        }

        if (isWalled() == true && isGrounded() == false && velX != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(-15f * velX, jumpSpeed);
            }
        }

        if (velX > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        if (velX < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    bool isGrounded()
    {
        raycast = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.505f), new Vector3(1f, 0.01f, 1f), 0f, Vector2.down, 0f, collisionLayers);
        return raycast.collider != null;
    }

    bool isWalled()
    {
        raycast = Physics2D.BoxCast(new Vector2(transform.position.x + (0.505f * velX > 0 ? 1 : -1), transform.position.y), new Vector3(0.01f, 1f, 1f), 0f, Vector2.down, 0f, collisionLayers);
        return raycast.collider != null;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, velX * speedX, resistance), rb.velocity.y);
    }
}