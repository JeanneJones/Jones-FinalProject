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
    public float maxJumpHeight = 11f; // Adjust this value for your desired jump height.

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;




    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset triggers to default state
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("Left");
        anim.ResetTrigger("Right");
       

        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the new velocity based on the input
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontalInput * moveSpeed;
        rb.velocity = newVelocity;

        if (horizontalInput > 0f)
        {
            // Right movement
            anim.SetTrigger("Right");
            transform.localScale = new Vector3(1, 1, 1); // Flip sprite to face right
        }

        else if (horizontalInput < 0f)
        {
            // Left movement
            anim.SetTrigger("Left");
            transform.localScale = new Vector3(-1, 1, 1); // Flip sprite to face left
        }
        else
        {
            // Idle state when not moving horizontally
            anim.SetTrigger("Idle");
        }

        // Handle jumping
        if (Input.GetKeyDown("space") && isGrounded)
        {
            // Start jumping
            anim.SetTrigger("Jump");
            float actualJumpForce = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * maxJumpHeight);
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jumping state when landing on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

