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


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //gMS = gameManager.GetComponent<GameManagerScript>();
    }
    
    bool isGrounded()
    {
        raycast = Physics2D.BoxCast(new Vector3(transform.position.x, transform.position.y - 0.55f), new Vector3(1, 0.1f, 1), 0f, Vector2.down, 0f, collisionLayers);
        return raycast.collider != null;
    }
    
    void Update()
    {
        if (isGrounded())
        {
            coyote = 0.10f;
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
        }
        
        velX = Input.GetAxisRaw("Horizontal") * speedX;

        screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + 1;
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        mousePos = new Vector3((Mathf.Round(worldPos.x)), (Mathf.Round(worldPos.y)), 0);

        if(Input.GetMouseButtonDown(0))
        {
            playerToMouse = (mousePos - transform.position) * -1;
            angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x);
            rb.velocity += new Vector2(power * (Mathf.Cos(angle)), power * (Mathf.Sin(angle)));
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, velX, resistance), rb.velocity.y);
    }
}
