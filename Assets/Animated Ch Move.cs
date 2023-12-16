using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCharacterMovement : MonoBehaviour
{
    private string previousState;



    Rigidbody2D rb;
    Animator anim;
    private bool isGrounded = true;
    private Transform originalParent;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float maxJumpHeight = 11f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private bool isOnMovingPlatform = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalParent = transform.parent;
    }

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
            // Check if the player has stopped moving horizontally and was previously walking right
            if (Mathf.Approximately(rb.velocity.x, 0f) && previousState == "Right")
            {
                anim.SetTrigger("Idle Right");
            }
            else
            {
                anim.SetTrigger("Idle");
            }
        }

        // Handle jumping
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded)
            {
                anim.SetTrigger("Jump");

                // Calculate jump force considering the moving platform's velocity
                float actualJumpForce = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * maxJumpHeight);
                rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
            }
            else if (isOnMovingPlatform)
            {
                // Jump when on a moving platform
                transform.parent = null;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isOnMovingPlatform = false;
            }
        }

        // Update the previous state
        UpdatePreviousState();
    }

    // Method to update the previous animation state
    void UpdatePreviousState()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Right"))
        {
            previousState = "Right";
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Left"))
        {
            previousState = "Left";
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            previousState = "Idle";
        }
        // Add more conditions as needed for other animation states
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
            isOnMovingPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = originalParent;
            isOnMovingPlatform = false;
        }
    }
}