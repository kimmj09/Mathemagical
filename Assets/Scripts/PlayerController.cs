using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // movement
    [Header("Movement")]
    private float horizontal;
    public bool isFacingRight = true;
    private bool doubleJump;
    private bool isRunning;
    public float walkSpeed = 7f;
    public float runSpeed = 10f;
    public float jumpingPower = 16f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        //move control
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        //jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

        //double jump
        if (Input.GetButtonDown("Jump") && !IsGrounded() && doubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            doubleJump = false;
        }
        
        //jump power
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

        isRunning = Input.GetButton("Run");

        //can double jump
        if (IsGrounded())
            doubleJump = true;
    }

    private void FixedUpdate()
    {
        float dx = horizontal * (isRunning && IsGrounded() ? runSpeed : walkSpeed);
        rb.linearVelocity = new Vector2(dx, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}