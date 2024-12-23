using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpAmount = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D playerCollider;
    private BoxCollider2D playerFeetCollider;
    private Vector2 moveInput;
    private float startGravity;

    private const string IS_RUNNING = "isRunning";
    private const string IS_IDLE_CLIMBING = "isIdleClimbing";
    private const string IS_CLIMBING = "isClimbing";
    private const string GROUND_LAYER = "Ground";
    private const string CLIMBING_LAYER = "Climbing";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        startGravity = rb.gravityScale;
    }

    void Update()
    {
        RunningAnimation();
    }

    void FixedUpdate()
    {
        Climb();
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!IsTouchingTheGround()) return;

      if(value.isPressed)
        {
            rb.velocity = new Vector2(0f, jumpAmount);
        }
    }

    private void Climb()
    {
        if (!IsPossibleToClimb())
        {
            SetClimbingAnimations(false, false);
            rb.gravityScale = startGravity;
            return;
        }

        ClimbAnimation();
        rb.velocity = new Vector2(
           moveInput.y == 0f ? rb.velocity.x : 0f,
           moveInput.y == 0f ? 0f : moveInput.y * speed
           );
        rb.gravityScale = 0f;

    }

    private bool IsPossibleToClimb()
    {
        return playerFeetCollider.IsTouchingLayers(LayerMask.GetMask(CLIMBING_LAYER));
    }

    private void SetClimbingAnimations(bool isIdleClimbing, bool isClimbing)
    {
        animator.SetBool(IS_IDLE_CLIMBING, isIdleClimbing);
        animator.SetBool(IS_CLIMBING, isClimbing);
    }

    private void ClimbAnimation()
    {
        if (HasVerticalSpeed())
        {
            SetClimbingAnimations(true, true);
            return;
        }
        SetClimbingAnimations(true, false);
    }

    private bool HasVerticalSpeed()
    {
        return Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
    }

    private void Run()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        rb.velocity = velocity;
    }

    private bool IsTouchingTheGround()
    {
        return playerFeetCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER));
    }

    private void RunningAnimation()
    {
        if(HasHorizontalSpeed())
        {
            ChangePlayerDirection();
            SetRunningAnimation(true);
            return;
        }
        SetRunningAnimation(false);
    }

    private bool HasHorizontalSpeed()
    {
        return moveInput.x != 0;
    }

    private void ChangePlayerDirection()
    {
        transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
    }

    private void SetRunningAnimation(bool isRunning)
    {
        animator.SetBool(IS_RUNNING, isRunning);
    }
}
