using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCharacterMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    private bool isGrounded = true;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float maxJumpHeight = 11f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Transform originalParent;  // To store the original parent of the player

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalParent = transform.parent;  // Store the original parent
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        anim.ResetTrigger("Jump");
        anim.ResetTrigger("Left");
        anim.ResetTrigger("Right");

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalInput * moveSpeed;
        rb.velocity = newVelocity;

        if (horizontalInput > 0f)
        {
            anim.SetTrigger("Right");
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0f)
        {
            anim.SetTrigger("Left");
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            anim.SetTrigger("Idle");
        }

        if (Input.GetKeyDown("space") && isGrounded)
        {
            anim.SetTrigger("Jump");
            float actualJumpForce = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * maxJumpHeight);
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);

            // Unparent the player when they jump
            transform.parent = originalParent;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // Reassign the original parent when landing on the ground
            transform.parent = originalParent;
        }

        // Check if colliding with a moving platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // Parent the player to the moving platform
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        // Unparent the player if they leave the moving platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = originalParent;
        }
    }
}
