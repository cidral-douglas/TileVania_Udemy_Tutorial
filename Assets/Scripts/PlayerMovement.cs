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
    private Vector2 moveInput;

    private const string IS_RUNNING = "isRunning";
    private const string GROUND_LAYER = "Ground";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        Run();
        RunningAnimation();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER))) return;

      if(value.isPressed)
        {
            rb.velocity = new Vector2(0f, jumpAmount);
        }
    }

    private void Run()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        rb.velocity = velocity;
    }

    private void RunningAnimation()
    {
        if (HasHorizontalSpeed())
        {
            SetIsRunning(true);
            ChangePlayerDirection();
        }
        else
        {
            SetIsRunning(false);
        }
    }

    private bool HasHorizontalSpeed()
    {
        return Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    }

    private void ChangePlayerDirection()
    {
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private void SetIsRunning(bool isRunning)
    {
        animator.SetBool(IS_RUNNING, isRunning);
    }
}
