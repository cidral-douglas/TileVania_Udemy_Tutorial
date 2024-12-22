using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        Debug.Log(Mathf.Sign(rb.velocity.x));
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private void SetIsRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }
}
