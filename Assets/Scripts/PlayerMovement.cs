using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RunningAnimation();
    }

    void FixedUpdate()
    {
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        Vector2 velocity = new Vector2(moveInput.x * 5f, rb.velocity.y);
        rb.velocity = velocity;
    }

    private void RunningAnimation()
    {
        if (moveInput.x != 0)
        {
            SetIsRunning(true);
        }
        else
        {
            SetIsRunning(false);
        }
    }

    private void SetIsRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }
}
