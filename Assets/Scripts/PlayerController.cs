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
    public float jumpingPower = 10f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Animator 컴포넌트 변수 추가
    private Animator anim;

    void Start()
    {
        // Animator 컴포넌트 가져오기
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // move control
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        // jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);

        /* double jump
        if (Input.GetButtonDown("Jump") && !IsGrounded() && doubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            doubleJump = false;
        }
        */

        // jump power
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

        isRunning = Input.GetButton("Run");

        // can double jump
        if (IsGrounded())
            doubleJump = true;

        // [추가] 애니메이션 파라미터 제어
        UpdateAnimation();
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

    // [추가] 애니메이션 상태 업데이트 함수
    private void UpdateAnimation()
    {
        if (anim == null) return;

        // 좌우 입력이 있으면 isMoving을 true로 전달
        bool isMoving = Mathf.Abs(horizontal) > 0f;
        anim.SetBool("isMoving", isMoving);

        // 캐스팅/공격 테스트 (마우스 우클릭)
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("charge");
        }
    }
}