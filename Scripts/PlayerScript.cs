using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float horizontal;
    bool isFacingRight;

    bool isJumping;

    float coyote;
    float buffer;

    Rigidbody2D rigidBody2d;
    BoxCollider2D boxCollider2d;
    [SerializeField] LayerMask groundLayer;

    RaycastHit2D raycastHit2d;

    Vector2 forceAway;
    [HideInInspector] public bool canShoot;
    [HideInInspector] public float timeBtwShots;

    void Awake()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        isFacingRight = true;
        canShoot = true;
        timeBtwShots = 0.5f;
    }

    void Update()
    {
        // Input Movement
        horizontal = Input.GetAxis("Horizontal") * 75f;

        timeBtwShots -= Time.deltaTime;

        // Jump
        if (IsGrounded())
        {
            coyote = 0.1f;
            if (timeBtwShots < 0)
            {
                canShoot = true;
            }
        }
        else
        {
            coyote -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            buffer = 0.1f;
        }
        else
        {
            buffer -= Time.deltaTime;
        }

        if (coyote > 0f && buffer > 0f && !isJumping)
        {
            rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, 150f);

            buffer = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rigidBody2d.velocity.y > 0f)
        {
            rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, rigidBody2d.velocity.y * 0.85f); 

            coyote = 0f;
        }

        // Direction facing
        Flip();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(forceAway.x) > 0 || Mathf.Abs(forceAway.y) > 0)
        {
            rigidBody2d.velocity = forceAway;
            forceAway.x *= 0.85f;
            forceAway.y *= 0.85f;
            if (Mathf.Abs(forceAway.x) < 37.5 * 25 && Mathf.Abs(forceAway.y) < 37.5f)
            {
                forceAway = new Vector2(0, 0);
            }
        }

        // Movement
        rigidBody2d.velocity = new Vector2((rigidBody2d.velocity.x * Time.deltaTime) + horizontal, rigidBody2d.velocity.y);
    }

    bool IsGrounded()
    {
        raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit2d.collider != null;
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }

    public void Knockback(float force, float rotation)
    {
        if (canShoot == true && timeBtwShots < 0)
        {
            timeBtwShots = 0.5f;
            forceAway = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad) * force * -45f, Mathf.Sin(rotation * Mathf.Deg2Rad) * force);
            canShoot = false;
        }
    }
}
